namespace TicTacToeService.GameManager;

public interface IGameManager
{
    Task<MoveResponse> MakeMove(MoveRequest moveRequest);
}