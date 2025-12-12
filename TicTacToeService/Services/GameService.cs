using Grpc.Core;
using TicTacToeService.GameManager;
using TicTacToeService.Models;
using TicTacToeService.RoomManager;

namespace TicTacToeService.Services;

public class GameService(IRoomManager roomManager, IGameManager gameManager)
    : TicTacToeService.GameService.GameServiceBase
{
    public override async Task Subscribe(SubscribeRequest request, 
        IServerStreamWriter<GameUpdate> responseStream,
        ServerCallContext context)
    {
        Player player = roomManager.JoinGame(responseStream);

        roomManager.CanStartGame += (_, room) => gameManager.SetUpGame(room);

        await player.NotifyOnConnection();
        
        Task clientDisconnectTask = Task.Run(() =>
        {
            context.CancellationToken.WaitHandle.WaitOne();
        });

        Task finishedTask = await Task.WhenAny(clientDisconnectTask, player.RequestDisconnectSource.Task);

        if (finishedTask == clientDisconnectTask) // If client disconnected first (lost internet, closed tab)
        {
            await roomManager.CleanUpOnClientDisconnect(player.Id);
        }
    }

    public override Task<MoveResponse> MakeMove(MoveRequest request,
        ServerCallContext context)
    {
        return gameManager.MakeMove(request);
    }
}