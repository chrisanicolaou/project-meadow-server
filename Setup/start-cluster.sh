#!/usr/bin/env bash

set -e  # Exit on any error

# Get the directory of this script
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(dirname "$SCRIPT_DIR")"
K8S_DIR="$ROOT_DIR/k8s"

CLUSTER_NAME="agones"
K8S_VERSION="v1.31.0"
MIN_PORT=7000
MAX_PORT=7100

cleanup() {
    local exit_code=$?
    local failed_command=$BASH_COMMAND

    echo "❌ Error: command '$failed_command' failed with exit code $exit_code"
    echo "⚠️ Cleaning up: deleting Minikube cluster '$CLUSTER_NAME'"
    minikube delete -p $CLUSTER_NAME || true
    echo "✅ Cleanup complete"
    echo -n "👉 Press any key to exit..."
    read -r -n 1 -s
    echo ""
    exit $exit_code
}

retry_kubectl_apply() {
    local file=$1
    local max_retries=10
    local delay=5

    for ((i=1; i<=max_retries; i++)); do
        echo "🔄 Applying $file (attempt $i/$max_retries)..."
        if kubectl apply -f "$file"; then
            echo "✅ Successfully applied $file"
            return 0
        fi
        echo "⚠️ Failed to apply $file - this is probably because the Agones controller pod is not yet ready. Retrying in $delay seconds..."
        sleep $delay
    done

    echo "❌ ERROR: Failed to apply $file after $max_retries attempts"
    return 1
}


# Trap ERR (any error) and INT (Ctrl+C) to run cleanup
trap cleanup ERR INT

echo "🚀 Starting Minikube cluster: $CLUSTER_NAME"
minikube start --kubernetes-version $K8S_VERSION -p $CLUSTER_NAME \
    --ports $MIN_PORT-$MAX_PORT:$MIN_PORT-$MAX_PORT/tcp,31350:31350/tcp,31351:31351/tcp

echo "✅ Minikube cluster started"

echo "🚀 Adding Agones Helm repo"
helm repo add agones https://agones.dev/chart/stable
helm repo update

echo "🚀 Installing Agones Helm chart"
helm install agones-sys --namespace agones-system --create-namespace \
    --set gameservers.minPort=$MIN_PORT,gameservers.maxPort=$MAX_PORT agones/agones

echo "✅ Agones installed"

echo "😴 Cooling off while the Agones controller pod is being created..."
sleep 30

# Apply fleet
echo "🚀 Applying fleet.yaml"
retry_kubectl_apply "$K8S_DIR/fleet.yaml"

# Apply fleet autoscaler
echo "🚀 Applying fleetautoscaler.yaml"
retry_kubectl_apply "$K8S_DIR/fleetautoscaler.yaml"

# Apply CockroachDB pod
echo "🚀 Applying cockroachdb-pod.yaml"
retry_kubectl_apply "$K8S_DIR/cockroachdb-pod.yaml"

# Apply CockroachDB service
echo "🚀 Applying cockroachdb-service.yaml"
retry_kubectl_apply "$K8S_DIR/cockroachdb-service.yaml"

# Apply Nakama persistent volume claim
echo "🚀 Applying nakama-data-persistentvolumeclaim.yaml"
retry_kubectl_apply "$K8S_DIR/nakama-data-persistentvolumeclaim.yaml"

echo "✅ Fleet and database setup applied"

# Apply RBAC permissions
echo "🚀 Applying gameserver-rbac.yaml"
retry_kubectl_apply "$K8S_DIR/gameserver-rbac.yaml"

echo "✅ RBAC permissions applied"

echo "😴 Cooling off while dependency containers are being created..."
sleep 10

# Apply Nakama pod
echo "🚀 Applying nakama-pod.yaml"
retry_kubectl_apply "$K8S_DIR/nakama-pod.yaml"

# Apply Nakama service
echo "🚀 Applying nakama-service.yaml"
retry_kubectl_apply "$K8S_DIR/nakama-service.yaml"

# Apply Nakama NodePort
echo "🚀 Applying nakama-nodeport.yaml"
retry_kubectl_apply "$K8S_DIR/nakama-nodeport.yaml"

# Disable trap on successful completion (no cleanup needed)
trap - ERR INT

echo "🎉 All setup complete!"
if [ -t 1 ]; then
    echo -n "👉 Press any key to exit..."
    read -r -n 1 -s
    echo ""
fi
