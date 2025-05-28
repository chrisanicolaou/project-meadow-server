# project-meadow-server
simple TCP listener today, Greatest Multiplayer Card Game Ever tomorrow (tm)

## Prerequisites
A local docker engine (for Windows, Docker Desktop)

## Setup
From root, run 
```docker compose up --build```

Observe docker logs for errors. Verify that the game server is sending health pings to the SDK

Verify you can access Nakama at http://127.0.0.1:7351 (login with "admin" & "password")

Verify you can send pings to the gameserver:

```
nc localhost 7654
Hello
```
You should see a response like:
`Received: Hello!`