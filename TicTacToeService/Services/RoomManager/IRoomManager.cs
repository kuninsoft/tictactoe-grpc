using Grpc.Core;
using TicTacToeService.Services.RoomManager.Models;

namespace TicTacToeService.Services.RoomManager;

public interface IRoomManager
{
    event EventHandler<Room> CanStartGame;
    
    Player JoinGame(IServerStreamWriter<GameUpdate> playerStream);
    Task NotifyRoomWithPlayer(string playerId, GameUpdate update);
    Task CleanUp(string playerId);
}