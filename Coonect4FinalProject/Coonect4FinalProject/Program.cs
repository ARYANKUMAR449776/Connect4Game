using System;


/* 
   Aryan Kumar 
   449776
   Connect 4 Final Project 
*/

/*Abstract Class used to handle players or Ai*/
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

class RandomAIPlayer : AbstractPlayer
{
    public RandomAIPlayer(char symbol) : base(symbol)
    {
    }

    public override string Name => "Computer (AI)";

    public int GetRandomMove()
    {
        Random random = new Random();
        return random.Next(0, 7); // Randomly select a column index between 0 and 6
    }
}


/*Gameboard Class for printing the GameBoard and initialize it and checking empty spaces or matching symbols for the win , draw logic*/
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

/*Coonect 4 game class for the play logic*/
class ConnectFourGame
{
    private GameBoard board;
    private AbstractPlayer player1;
    private AbstractPlayer player2;
    private AbstractPlayer currentPlayer;

    public ConnectFourGame(string player1Name = null, string player2Name = null, bool playAgainstAI = false)
    {
        board = new GameBoard();
        player1 = new Player1('X', player1Name);
        if (playAgainstAI)
        {
            player2 = new RandomAIPlayer('O');
        }
        else
        {
            player2 = new Player2('O', player2Name);
        }
        currentPlayer = player1;
    }

    public void PlayGame()
    {
        while (!board.IsGameOver())
        {
            board.PrintConnect4Board();
            int column;

            if (currentPlayer is RandomAIPlayer)
            {
                RandomAIPlayer aiPlayer = currentPlayer as RandomAIPlayer;
                column = aiPlayer.GetRandomMove();
                Console.WriteLine($"{currentPlayer.Name} chose column {column + 1}");
            }
            else
            {
                while (true)
                {
                    Console.WriteLine($"{currentPlayer.Name}, enter column (1-7)(Then press enter):");
                    string input = Console.ReadLine();

                    // Checking if the input is a valid number between 1 and 7
                    if (!int.TryParse(input, out column) || column < 1 || column > 7)
                    {
                        Console.WriteLine("Please select a correct column (1-7).");
                    }
                    else
                    {
                        column--; 
                        break; 
                    }
                }
            }

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
        bool playAgain = false;
        string player1Name = null;
        string player2Name = null;
        bool playAgainstAI = false;

        do
        {
            // Asking the user whether to play against the computer initially
            string response;
            do
            {
                Console.WriteLine("Do you want to play against the computer? (yes/no)");
                response = Console.ReadLine().ToLower();
                if (response != "yes" && response != "no")
                {
                    Console.WriteLine("Please enter 'yes' or 'no'.");
                }
            } while (response != "yes" && response != "no");

            playAgainstAI = (response == "yes");

            if (!playAgainstAI)
            {
                // Asking for player names if not playing against the computer
                GetPlayerNames(out player1Name, out player2Name);
            }

            do
            {
                // Creating the ConnectFour game with the provided or default names and AI choice
                ConnectFourGame game = new ConnectFourGame(player1Name, player2Name, playAgainstAI);

                // Starting the game
                game.PlayGame();

                // Asking the user if they want to play again
                do
                {
                    Console.WriteLine("Do you want to play again? (yes/no)");
                    response = Console.ReadLine().ToLower();
                    if (response != "yes" && response != "no")
                    {
                        Console.WriteLine("Please enter 'yes' or 'no'.");
                    }
                } while (response != "yes" && response != "no");

                playAgain = (response == "yes");

                if (playAgain)
                {
                    // Asking the user if they want to play with the same person or a different one
                    do
                    {
                        Console.WriteLine("Do you want to play with the same person? (yes/no)");
                        response = Console.ReadLine().ToLower();
                        if (response != "yes" && response != "no")
                        {
                            Console.WriteLine("Please enter 'yes' or 'no'.");
                        }
                    } while (response != "yes" && response != "no");

                    if (response == "no")
                    {
                        // If not playing with the same person, ask for player names
                        GetPlayerNames(out player1Name, out player2Name);
                        playAgainstAI = false;
                    }
                }

            } while (playAgain);

            // Exiting the loop if the user chooses not to play again
        } while (false); // Change true to false to exit the loop after the first game

        Console.WriteLine("Thank you for playing!");
    }

    static void GetPlayerNames(out string player1Name, out string player2Name)
    {
        Console.WriteLine("Enter Player 1's name (or leave blank for default)(Then press enter):");
        player1Name = Console.ReadLine();
        Console.WriteLine("Enter Player 2's name (or leave blank for default)(Then press enter):");
        player2Name = Console.ReadLine();
    }
}
