using Grpc.Core;
using TicTacToeService.GameManager;
using TicTacToeService.RoomManager;

namespace TicTacToeService.Services;

public class GameService(IRoomManager roomManager, IGameManager gameManager)
    : TicTacToeService.GameService.GameServiceBase
{
    public override async Task Subscribe(SubscribeRequest request, 
        IServerStreamWriter<GameUpdate> responseStream,
        ServerCallContext context)
    {
        // On connection

        context.CancellationToken.WaitHandle.WaitOne();
        
        // Post-connection cleanup
    }

    public override async Task<MoveResponse> MakeMove(MoveRequest request,
        ServerCallContext context)
    {
        await base.MakeMove(request, context);
    }
}