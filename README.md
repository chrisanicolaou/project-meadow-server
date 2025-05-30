# project-meadow-server
simple TCP listener today, Greatest Multiplayer Card Game Ever tomorrow (tm)

## Prerequisites
- A local docker engine (for Windows, Docker Desktop)
- [Minikube](https://minikube.sigs.k8s.io/docs/start/?arch=%2Fwindows%2Fx86-64%2Fstable%2F.exe+download)
- [kubectl](https://kubernetes.io/docs/tasks/tools/)
- [Helm](https://helm.sh/docs/intro/install/)
- (Optional) The [MeadowClient](https://github.com/chrisanicolaou/project-meadow-client) (which also requires Godot v4.4.1, sorry)

## Dev Setup
1. Run the setup script in `.\Setup`:
```shell
.\Setup\start-cluster.sh
```
2. Confirm the installation has run correctly:
```
kubectl get pods -o wide
```

## Test with MeadowClient
1. Navigate to [Nakama's Admin UI](http://localhost:31351) and login with `admin` & `password`.
2. Create 2 new users and remember their emails & passwords.
3. Start 2 instances of the `MeadowClient` from the `Main` scene.
> [!TIP]
> You can do this from 1 instance of the editor by going to Debug -> Customize Run Instances, and checking "enable multiple instances".
4. Login with each user and search for a match.
> [!TIP]
> You can observe logs on Nakama with `kubectl logs nakama` to see the tickets being received.
5. Once a match is found, type into the textboxes. You should see each message appearing on the other client.
6. Type "KILL". The gameserver will be killed - and your clients will crash! (sorry)

## Teardown

Once you're done, you can remove the cluster with:
```shell
.\Setup\stop-cluster.sh
```

## Troubleshooting
### "Found drivers but none were healthy"
Make sure your docker engine is online (open Docker Desktop on Windows)

## TODO's
- Introduce persistent volumes for local nakama