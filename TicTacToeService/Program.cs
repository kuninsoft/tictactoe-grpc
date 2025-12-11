using TicTacToeService.Services;
using TicTacToeService.Services.RoomManager;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<IRoomManager, RoomManager>();

WebApplication app = builder.Build();

app.MapGrpcService<GameService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();