# Tic-Tac-Toe gRPC Game Server

This project implements a **real-time Tic-Tac-Toe game engine** using **native gRPC streaming**.  
It is designed as a **transport-agnostic core service**, suitable for consumption by mobile clients, backend adapters, or other services.

For browser-based clients, this service is intended to be consumed through a **Backend-for-Frontend (BFF)** layer (e.g. SignalR or WebSocket adapter), rather than directly via gRPC-Web.

---

## Overview

The server is responsible for:

- Managing game rooms (maximum two players per room)
- Assigning player roles (X / O)
- Validating and applying moves
- Broadcasting game state updates via gRPC streaming
- Handling player disconnects and game termination

Each game runs in its own isolated room and is fully server-authoritative.

---

## gRPC API

### Server Streaming

#### `Subscribe`

Clients subscribe to a server-side stream to receive game events.

- Assigns the player to a game room
- Issues a server-generated player token
- Assigns a role (X or O)
- Pushes all subsequent game updates over the stream

The stream remains open until:
- the game ends
- the client disconnects
- the server terminates the game

---

### Unary RPC

#### `MakeMove`

Submits a move for validation and application.

- Validates player identity and turn order
- Updates the game state if the move is valid
- Triggers game update events for all players in the room

---

## Game Flow

1. Client calls `Subscribe`
2. Server assigns the client to a room and role
3. When two players are connected, the game starts
4. Players submit moves via `MakeMove`
5. Server broadcasts game updates via the stream
6. Game ends when a winner is determined or the game is interrupted

---

## Client Identity

- Player identity is managed via **server-issued tokens**
- No authentication or cookies are required
- Tokens are passed explicitly with each request

---

## Design Decisions

- **gRPC streaming** is used to model continuous, server-driven game updates
- The game engine is kept independent of any specific client transport
- Browser limitations are handled via an optional BFF layer, not within the core service
- REST was intentionally avoided to prevent polling-based designs

---

## Intended Clients

This service is suitable for:

- Mobile applications (native gRPC)
- Desktop applications
- Backend adapters (SignalR / WebSocket)
- Automated test clients or bots

---

## Notes

This project is intentionally structured to demonstrate:

- Event-driven backend design
- Streaming-based communication models
- Clear separation between core domain logic and client transport concerns