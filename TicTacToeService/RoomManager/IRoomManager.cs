using Grpc.Core;
using TicTacToeService.Models;

namespace TicTacToeService.RoomManager;

public interface IRoomManager
{
    /// <summary>
    /// Event that is fired whan a specific room can start the game (2 players have joined)
    /// </summary>
    event EventHandler<Room> CanStartGame;
    
    /// <summary>
    /// Wrap connection stream in a Player object, assign a room and role
    /// </summary>
    /// <returns>Player object with assigned role</returns>
    Player JoinGame(IServerStreamWriter<GameUpdate> playerStream);
    
    /// <summary>
    /// Get room with specific player
    /// </summary>
    Room FindRoomWithPlayer(string playerId);
    
    
    Task NotifyRoomWithPlayer(string playerId, GameUpdate update);
    
    void CleanUp(Room room);

    Task CleanUpOnClientDisconnect(string playerId);
}