using TicTacToeService.Models;
using TicTacToeService.RoomManager;

namespace TicTacToeService.GameManager;

public class GameManager(IRoomManager roomManager) : IGameManager
{
    private readonly Dictionary<Room, GameField> _roomsFields = new();

    public Task SetUpGame(Room room)
    {
        if (!_roomsFields.ContainsKey(room))
        {
            _roomsFields[room] = new GameField();
        }

        return Task.CompletedTask;
    }

    public async Task<MoveResponse> MakeMove(MoveRequest moveRequest)
    {
        Room room = roomManager.FindRoomWithPlayer(moveRequest.PlayerToken);
        CellMove move = room.GetPlayerRole(moveRequest.PlayerToken);
        GameField field = _roomsFields[room];

        if (!field.IsMoveValid(moveRequest.Row, moveRequest.Col, move))
        {
            return new MoveResponse {Accepted = false};
        }

        GameState moveResult = field.MakeMove(moveRequest.Row, moveRequest.Col, move);

        Winner gameWinner = moveResult switch
        {
            GameState.NotFinished => Winner.Unknown,
            GameState.Tie => Winner.Tie,
            GameState.WinnerX => Winner.X,
            GameState.WinnerO => Winner.O,
            _ => throw new ArgumentOutOfRangeException(nameof(moveRequest))
        };

        var update = new GameUpdate
        {
            GameEvent = GameEventType.MoveMade,
            MoveInfo = new MoveInfo
            {
                Row = moveRequest.Row,
                Col = moveRequest.Col,
                MoveInitiator = move is CellMove.X ? Role.X : Role.O,
                Winner = gameWinner
            },
            NextTurn = field.CurrentTurn is CellMove.X ? Role.X : Role.O
        };

        await room.NotifyAll(update);

        if (gameWinner is not Winner.Unknown)
        { 
            room.CloseConnections();
            roomManager.CleanUp(room);
        }
        
        return new MoveResponse {Accepted = true};
    }
}