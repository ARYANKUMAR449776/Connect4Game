
using System.ComponentModel.Design;
using System.Security.Claims;
/*
Final Project
Aryan Kumar
449776
*/

class GameBoard/*New class to implement the gameboard and check for wins and checking turns: AK*/
{

    private char[,] board;
    private const int Rows = 6;
    private const int Cols = 7;


    public GameBoard()
    {
        board = new char[Rows, Cols];
        InitializeBoard();
    }/*gameboard constructor*/

    private void InitializeBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                board[row, col] = ' ';
            }
        }
    }/*initializing game board*/

    public void PrintBoard()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                Console.Write(board[row, col] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("---------------");
        Console.WriteLine("1 2 3 4 5 6 7");
    }/*boilerplate display in cmd line*/

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
        /*DOne AK*/
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
        /*Done AK*/
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
        /*Done Ak*/
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
        /*Done AK*/
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
                // if there's an empty cell at the top row, the game is not a draw
                return false;
            }
        }
        // if all cells in the top row are filled, the game is a draw
        return true;
    }


    public bool IsGameOver()
    {
        return CheckWin() || CheckDraw();
    }
}
class Controller
{

}
/*    Class designed for keeping track of the turns    */
class Model
{

}
/*    Class Designed for player 1    */
class Player1
{

}
/*    Class Designed for player 2    */
class Player2
{

}


class program
{
    static void Main(string[] args)
    {
        Console.WriteLine("");
    }
}
