using TicTacToeService.GameManager;
using TicTacToeService.RoomManager;
using TicTacToeService.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IRoomManager, RoomManager>();
builder.Services.AddSingleton<IGameManager, GameManager>();

WebApplication app = builder.Build();

app.MapGrpcService<GameService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();