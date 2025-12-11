using Grpc.Core;

namespace TicTacToeService.Services.RoomManager.Models;

public class Room
{
    private readonly List<Player> _players = [];
    
    public Guid RoomId { get; } = Guid.NewGuid();

    public int PlayerCount => _players.Count;

    public Player AddPlayer(IServerStreamWriter<GameUpdate> playerStream)
    {
        if (PlayerCount >= 2)
        {
            throw new InvalidOperationException("Can't add any more players to the room, it is full");
        }

        Role assignedRole = PlayerCount is 0 ? Role.X : Role.O;
        var player = new Player(assignedRole, playerStream);
        
        _players.Add(player);

        return player;
    }

    public bool HasPlayer(string id)
    {
        return _players.Any(player => player.Id == id);
    }

    public async Task NotifyAll(GameUpdate update)
    {
        foreach (Player player in _players)
        {
            await player.Notify(update);
        }
    }
}