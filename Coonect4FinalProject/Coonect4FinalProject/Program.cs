using System;

abstract class AbstractPlayer
{
    public char Symbol { get; }
    public abstract string Name { get; }

    protected AbstractPlayer(char symbol)
    {
        Symbol = symbol;
    }
}/*Class to customize player name*/

class Player1 : AbstractPlayer
{
    private string playerName;

    public Player1(char symbol, string name) : base(symbol)
    {
        playerName = string.IsNullOrWhiteSpace(name) ? "Player 1 (X)" : name;
    }

    public override string Name => playerName;
}

class Player2 : AbstractPlayer
{
    private string playerName;

    public Player2(char symbol, string name) : base(symbol)
    {
        playerName = string.IsNullOrWhiteSpace(name) ? "Player 2 (O)" : name;
    }

    public override string Name => playerName;
}

class GameBoard
{
    private char[,] board;
    private const int Rows = 6;
    private const int Cols = 7;

    public GameBoard()
    {
        board = new char[Rows, Cols];
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                board[row, col] = ' ';
            }
        }
    }

    public void PrintBoard()
    {
        // Print top border
        Console.WriteLine(" ---------------------------");

        // Print board content with side borders
        for (int row = 0; row < Rows; row++)
        {
            Console.Write("|");
            for (int col = 0; col < Cols; col++)
            {
                Console.Write($" {board[row, col]} |");
            }
            Console.WriteLine();
            Console.WriteLine(" ---------------------------");
        }

        // Print numbers at the bottom
        Console.WriteLine("  1   2   3   4   5   6   7");
    }

    public bool DropPiece(int column, char symbol)
    {
        for (int row = Rows - 1; row >= 0; row--)
        {
            if (board[row, column] == ' ')
            {
                board[row, column] = symbol;
                return true;
            }
        }
        return false;
    }

    public bool CheckWin()
    {
        // Check horizontally
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col <= Cols - 4; col++)
            {
                if (board[row, col] != ' ' &&
                    board[row, col] == board[row, col + 1] &&
                    board[row, col] == board[row, col + 2] &&
                    board[row, col] == board[row, col + 3])
                {
                    return true;
                }
            }
        }

        // Check vertically
        for (int col = 0; col < Cols; col++)
        {
            for (int row = 0; row <= Rows - 4; row++)
            {
                if (board[row, col] != ' ' &&
                    board[row, col] == board[row + 1, col] &&
                    board[row, col] == board[row + 2, col] &&
                    board[row, col] == board[row + 3, col])
                {
                    return true;
                }
            }
        }

        // Check diagonally (bottom-left to top-right)
        for (int row = 0; row <= Rows - 4; row++)
        {
            for (int col = 0; col <= Cols - 4; col++)
            {
                if (board[row, col] != ' ' &&
                    board[row, col] == board[row + 1, col + 1] &&
                    board[row, col] == board[row + 2, col + 2] &&
                    board[row, col] == board[row + 3, col + 3])
                {
                    return true;
                }
            }
        }

        // Check diagonally (top-left to bottom-right)
        for (int row = 3; row < Rows; row++)
        {
            for (int col = 0; col <= Cols - 4; col++)
            {
                if (board[row, col] != ' ' &&
                    board[row, col] == board[row - 1, col + 1] &&
                    board[row, col] == board[row - 2, col + 2] &&
                    board[row, col] == board[row - 3, col + 3])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CheckDraw()
    {
        for (int col = 0; col < Cols; col++)
        {
            if (board[0, col] == ' ')
            {
                // If there's an empty cell at the top row, the game is not a draw
                return false;
            }
        }
        // If all cells in the top row are filled, the game is a draw
        return true;
    }

    public bool IsGameOver()
    {
        return CheckWin() || CheckDraw();
    }
}

class ConnectFourGame
{
    private GameBoard board;
    private AbstractPlayer player1;
    private AbstractPlayer player2;
    private AbstractPlayer currentPlayer;

    public ConnectFourGame(string player1Name = null, string player2Name = null)
    {
        board = new GameBoard();
        player1 = new Player1('X', player1Name);
        player2 = new Player2('O', player2Name);
        currentPlayer = player1;
    }

    public void PlayGame()
    {
        while (!board.IsGameOver())
        {
            board.PrintBoard();
            int column;

            do
            {
                Console.WriteLine($"{currentPlayer.Name}, enter column (1-7):");
            } while (!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7);

            column--; // Adjust for 0-based indexing

            if (board.DropPiece(column, currentPlayer.Symbol))
            {
                if (board.CheckWin())
                {
                    board.PrintBoard();
                    Console.WriteLine($"{currentPlayer.Name} wins!");
                    return;
                }
                else if (board.CheckDraw())
                {
                    board.PrintBoard();
                    Console.WriteLine("It's a draw!");
                    return;
                }
                else
                {
                    currentPlayer = (currentPlayer == player1) ? player2 : player1;
                }
            }
            else
            {
                Console.WriteLine("Column is full.");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string player1Name;
        string player2Name;

        // Prompt the user to enter Player 1's name
        Console.WriteLine("Enter Player 1's name (or leave blank for default):");
        player1Name = Console.ReadLine();

        // Prompt the user to enter Player 2's name
        Console.WriteLine("Enter Player 2's name (or leave blank for default):");
        player2Name = Console.ReadLine();

        // Create the ConnectFour game with the provided or default names
        ConnectFourGame game = new ConnectFourGame(player1Name, player2Name);

        // Start the game
        game.PlayGame();
    }
}
