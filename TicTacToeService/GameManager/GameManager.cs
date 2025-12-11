using TicTacToeService.Models;
using TicTacToeService.RoomManager;

namespace TicTacToeService.GameManager;

public class GameManager(IRoomManager roomManager) : IGameManager
{
    private readonly Dictionary<Room, GameField> _roomsFields = new();
    
    public Task<MoveResponse> MakeMove(MoveRequest moveRequest)
    {
        throw new NotImplementedException();
    }
}