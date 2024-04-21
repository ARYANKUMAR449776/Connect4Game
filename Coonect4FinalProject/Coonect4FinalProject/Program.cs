using System;

abstract class AbstractPlayer
{
    public char Symbol { get; }
    public abstract string Name { get; }

    protected AbstractPlayer(char symbol)
    {
        Symbol = symbol;
    }
}

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




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//class AIcomputerPlayer
//{
//    // This class is responsible for implementing an AI opponent for the Connect Four game.
//    //This method should analyze the current game state and
//    // return the column where the AI wants to drop its piece.)

//    // Parameters:
//    // - board: 2D array representing the current state of the game board.
//    // - aiSymbol: The symbol representing the AI's pieces on the board.
//    // - opponentSymbol: The symbol representing the opponent's pieces on the board.

//    // Returns:
//    // The column index where the AI wants to drop its piece.
//    //ensure that the AI's moves are valid and within the bounds of the board.

//    // Once you've implemented the GetBestMove method, integrate the AI into
//    // the ConnectFourGame class's PlayGame method. Replace the user's input with
//    // calls to the AI


//    public char Symbol { get; } // Symbol representing the AI's pieces on the board

//    // Constructor to initialize the AI's symbol
//    public bool IsPlayingAgainstHuman(string player2Name)
//    {
//        return !string.IsNullOrWhiteSpace(player2Name);
//    }
//    public AIcomputerPlayer()
//    {
//        Symbol = 'I';
//    }

//    // This method returns a random move for the AI.
//    public int GetRandomMove()
//    {
//        Random random = new Random();
//        return random.Next(0, 7); // Randomly select a column index between 0 and 6
//    }
//}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////




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

    public void PrintConnect4Board()
    {
        Console.Clear(); // Clear the console before printing the board

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

    public bool CheckingWinner()
    {
        // Check horizontally and vertically
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                char current = board[row, col];
                if (current == ' ')
                    continue;

                // Check horizontally
                if (col + 3 < Cols &&
                    current == board[row, col + 1] &&
                    current == board[row, col + 2] &&
                    current == board[row, col + 3])
                    return true;

                // Check vertically
                if (row + 3 < Rows &&
                    current == board[row + 1, col] &&
                    current == board[row + 2, col] &&
                    current == board[row + 3, col])
                    return true;

                // Check diagonally (bottom-left to top-right)
                if (col + 3 < Cols && row + 3 < Rows &&
                    current == board[row + 1, col + 1] &&
                    current == board[row + 2, col + 2] &&
                    current == board[row + 3, col + 3])
                    return true;

                // Check diagonally (top-left to bottom-right)
                if (col + 3 < Cols && row - 3 >= 0 &&
                    current == board[row - 1, col + 1] &&
                    current == board[row - 2, col + 2] &&
                    current == board[row - 3, col + 3])
                    return true;
            }
        }

        return false;
    }
    public bool CheckingGameDraw()
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
        return CheckingWinner() || CheckingGameDraw();
    }
}

class ConnectFourGame
{
    private GameBoard board;
    private AbstractPlayer player1;
    private AbstractPlayer player2;
    private AbstractPlayer currentPlayer;
    private int currentRound;

    public ConnectFourGame(string player1Name = null, string player2Name = null)
    {
        board = new GameBoard();
        player1 = new Player1('X', player1Name);
        player2 = new Player2('O', player2Name);
        currentPlayer = player1;
        currentRound = 1; // Start with round 1//theres a bug here fix it

    }

    public void PlayGame()
    {
        while (!board.IsGameOver())
        {
            board.PrintConnect4Board();
            Console.WriteLine($"Round {currentRound}");
            int column;

            do
            {
                Console.WriteLine($"{currentPlayer.Name}, enter column (1-7)(Then press enter):");
            } while (!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7);

            column--; // Adjust for 0-based indexing

            if (board.DropPiece(column, currentPlayer.Symbol))
            {
                if (board.CheckingWinner())
                {
                    board.PrintConnect4Board();
                    Console.WriteLine($"{currentPlayer.Name} wins!");
                    return;
                }
                else if (board.CheckingGameDraw())
                {
                    board.PrintConnect4Board();
                    Console.WriteLine("It's a draw!");
                    return;
                }
                else
                {
                    currentPlayer = (currentPlayer == player1) ? player2 : player1;
                    currentRound++;
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
        int numberOfGames;

        // Prompting the user to enter Player 1's name
        Console.WriteLine("Enter Player 1's name (or leave blank for default)(Then press enter):");
        player1Name = Console.ReadLine();

        // Prompting the user to enter Player 2's name
        Console.WriteLine("Enter Player 2's name (or leave blank for default)(Then press enter):");
        player2Name = Console.ReadLine();

        // Prompting the user to enter the number of games
        Console.WriteLine("Enter the number of games to play:");
        while (!int.TryParse(Console.ReadLine(), out numberOfGames) || numberOfGames <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number of games:");
        }

        for (int i = 0; i < numberOfGames; i++)
        {
            // Creating the ConnectFour game with the provided or default names
            ConnectFourGame game = new ConnectFourGame(player1Name, player2Name);

            // Starting the game
            game.PlayGame();
        }

        Console.WriteLine("Thank you for playing!");
    }
}
