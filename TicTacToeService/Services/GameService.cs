using Grpc.Core;

namespace TicTacToeService.Services;

public class GameService : TicTacToeService.GameService.GameServiceBase
{
    public override async Task Subscribe(SubscribeRequest request, 
        IServerStreamWriter<GameUpdate> responseStream,
        ServerCallContext context)
    {
        await base.Subscribe(request, responseStream, context);
    }

    public override async Task<MoveResponse> MakeMove(MoveRequest request,
        ServerCallContext context)
    {
        await base.MakeMove(request, context);
    }
}