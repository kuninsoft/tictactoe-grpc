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

    public Room FindRoomWithPlayer(string playerId)
    {
        return _rooms.FirstOrDefault(room => room.HasPlayer(playerId))
               ?? throw new InvalidOperationException("No room with such player id exists");
    }

    public async Task NotifyRoomWithPlayer(string playerId, GameUpdate update)
    {
        if (_rooms.FirstOrDefault(room => room.HasPlayer(playerId)) is {} foundRoom)
        {
            await foundRoom.NotifyAll(update);
        }
    }

    public void CleanUp(Room room)
    {
        _rooms.Remove(room);
    }

    public async Task CleanUpOnClientDisconnect(string playerId)
    {
        if (_rooms.FirstOrDefault(room => room.HasPlayer(playerId)) is { } foundRoom)
        {
            try
            {
                await foundRoom.TryNotifyOther(playerId,
                    new GameUpdate {GameEvent = GameEventType.GameInterrupted});
                foundRoom.CloseConnections();
            }
            finally
            {
                CleanUp(foundRoom);
            }
        }
    }
}