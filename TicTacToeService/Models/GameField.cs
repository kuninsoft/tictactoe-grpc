namespace TicTacToeService.Models;

public class GameField
{
    private readonly Cell[][] _field =
    [
        [Cell.None, Cell.None, Cell.None],
        [Cell.None, Cell.None, Cell.None],
        [Cell.None, Cell.None, Cell.None]
    ];

    public CellMove CurrentTurn { get; private set; } = CellMove.X;

    public bool IsMoveValid(int row, int col, CellMove move)
    {
        return CurrentTurn == move && _field[row][col] is Cell.None;
    }

    public GameState MakeMove(int row, int col, CellMove move)
    {
        SetCell(row, col, move);
        
        ToggleTurn();
        
        return CheckBoard();
    }

    private void SetCell(int row, int col, CellMove move)
    {
        _field[row][col] = move switch
        {
            CellMove.X => Cell.X,
            CellMove.O => Cell.O,
            _ => throw new ArgumentException("Move is invalid, must be either X or O", nameof(move))
        };
    }

    private void ToggleTurn()
    {
        CurrentTurn = CurrentTurn switch
        {
            CellMove.X => CellMove.O,
            CellMove.O => CellMove.X,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private GameState CheckBoard()
    {
        if (CheckX())
        {
            return GameState.WinnerX;
        }

        if (CheckO())
        {
            return GameState.WinnerO;
        }

        return IsFull() ? GameState.Tie : GameState.NotFinished;
    }

    private bool CheckX()
    {
        return CheckDiagonals(Cell.X) || CheckRows(Cell.X) || CheckCols(Cell.X);
    }

    private bool CheckO()
    {
        return CheckDiagonals(Cell.O) || CheckRows(Cell.O) || CheckCols(Cell.O);
    }
    
    private bool CheckDiagonals(Cell cell)
    {
        if (_field[1][1] != cell)
        {
            return false;
        }
        
        return _field[0][0] == cell && _field[2][2] == cell
               || _field[0][2] == cell && _field[2][0] == cell;
    }

    private bool CheckRows(Cell cell)
    {
        return _field.Any(row => row.All(cellType => cellType == cell));
    }

    private bool CheckCols(Cell cell)
    {
        for (var col = 0; col < 3; col++)
        {
            if (_field[col][0] == cell && _field[col][1] == cell && _field[col][2] == cell)
            {
                return true;
            }
        }

        return false;
    }
    
    private bool IsFull()
    {
        return _field.All(row => row.All(cell => cell is not Cell.None));
    }

    private enum Cell
    {
        None,
        X,
        O
    }
}