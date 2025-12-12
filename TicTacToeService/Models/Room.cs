using Grpc.Core;

namespace TicTacToeService.Models;

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

    public CellMove GetPlayerRole(string id)
    {
        if (_players.FirstOrDefault(player => player.Id == id) is not { } foundPlayer)
        {
            throw new InvalidOperationException("Such player does not exist in the room");
        }

        return foundPlayer.Role is Role.X ? CellMove.X : CellMove.O;
    }

    public async Task NotifyAll(GameUpdate update)
    {
        foreach (Player player in _players)
        {
            await player.Notify(update);
        }
    }

    public async Task<bool> TryNotifyOther(string id, GameUpdate update)
    {
        try
        {
            if (_players.FirstOrDefault(player => player.Id != id) is { } foundPlayer)
            {
                await foundPlayer.Notify(update);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public void CloseConnections()
    {
        foreach (Player player in _players)
        {
            player.CloseConnection();
        }
    }
}