using Grpc.Core;

namespace TicTacToeService.Services.RoomManager.Models;

public class Player(Role role, IServerStreamWriter<GameUpdate> streamWriter)
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public Role Role { get; set; } = role;

    public Task Notify(GameUpdate update)
    {
        return streamWriter.WriteAsync(update);
    }
}