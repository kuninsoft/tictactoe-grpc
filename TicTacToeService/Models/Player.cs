using Grpc.Core;

namespace TicTacToeService.Models;

public class Player(Role role, IServerStreamWriter<GameUpdate> streamWriter)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public Role Role { get; } = role;

    public TaskCompletionSource RequestDisconnectSource { get; } = new();
    
    public Task NotifyOnConnection()
    {
        return Notify(new GameUpdate
        {
            GameEvent = GameEventType.PlayerJoined,
            PlayerToken = Id,
            AssignedRole = Role
        });
    }
    
    public Task Notify(GameUpdate update)
    {
        return streamWriter.WriteAsync(update);
    }
    
    public void CloseConnection()
    {
        RequestDisconnectSource.TrySetResult();
    }
}