using Grpc.Core;
using TicTacToeService.Models;

namespace TicTacToeService.RoomManager;

public class RoomManager : IRoomManager
{
    public event EventHandler<Room> CanStartGame;
    
    private readonly List<Room> _rooms = [];

    public Player JoinGame(IServerStreamWriter<GameUpdate> playerStream)
    {
        if (_rooms.FirstOrDefault(room => room.PlayerCount < 2) is {} vacantRoom)
        {
            Player secondPlayer = vacantRoom.AddPlayer(playerStream);

            CanStartGame?.Invoke(this, vacantRoom);
            
            return secondPlayer;
        }

        var room = new Room();
        _rooms.Add(room);
        return room.AddPlayer(playerStream);
    }

    public async Task NotifyRoomWithPlayer(string playerId, GameUpdate update)
    {
        if (_rooms.FirstOrDefault(room => room.HasPlayer(playerId)) is {} foundRoom)
        {
            await foundRoom.NotifyAll(update);
        }
    }

    public Task CleanUp(string playerId)
    {
        if (_rooms.FirstOrDefault(room => room.HasPlayer(playerId)) is {} roomToDelete)
        {
            _rooms.Remove(roomToDelete);
        }

        return Task.CompletedTask;
    }
}