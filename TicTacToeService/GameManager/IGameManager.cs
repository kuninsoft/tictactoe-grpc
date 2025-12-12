using TicTacToeService.Models;

namespace TicTacToeService.GameManager;

public interface IGameManager
{
    Task SetUpGame(Room room);
    Task<MoveResponse> MakeMove(MoveRequest moveRequest);
}