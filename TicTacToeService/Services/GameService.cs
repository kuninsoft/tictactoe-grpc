using Grpc.Core;
using TicTacToeService.GameManager;
using TicTacToeService.Models;
using TicTacToeService.RoomManager;

namespace TicTacToeService.Services;

public class GameService : TicTacToeService.GameService.GameServiceBase
{
    private readonly IRoomManager _roomManager;
    private readonly IGameManager _gameManager;

    public GameService(IRoomManager roomManager, IGameManager gameManager)
    {
        _roomManager = roomManager;
        _gameManager = gameManager;
        
        _roomManager.CanStartGame += (_, room) => _gameManager.SetUpGame(room);
    }

    public override async Task Subscribe(SubscribeRequest request, 
        IServerStreamWriter<GameUpdate> responseStream,
        ServerCallContext context)
    {
        Player player = await _roomManager.JoinGame(responseStream); 
        
        Task clientDisconnectTask = Task.Run(() =>
        {
            context.CancellationToken.WaitHandle.WaitOne();
        });

        Task finishedTask = await Task.WhenAny(clientDisconnectTask, player.RequestDisconnectSource.Task);

        if (finishedTask == clientDisconnectTask) // If client disconnected first (lost internet, closed tab)
        {
            await _roomManager.CleanUpOnClientDisconnect(player.Id);
        }
    }

    public override Task<MoveResponse> MakeMove(MoveRequest request,
        ServerCallContext context)
    {
        return _gameManager.MakeMove(request);
    }
}