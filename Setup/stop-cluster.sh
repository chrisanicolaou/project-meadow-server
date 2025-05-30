CLUSTER_NAME="agones"

echo "⚠️ Cleaning up: deleting Minikube cluster '$CLUSTER_NAME'"
minikube delete -p $CLUSTER_NAME || true
echo "✅ Cleanup complete"
echo -n "👉 Press any key to exit..."
read -r -n 1 -s
echo ""