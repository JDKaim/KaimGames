using System;
using KaimGames.Minesweeper.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.Minesweeper.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int rows = 10;
            int columns = 20;
            int mines = 20;

            
           
            Game game = new Game(rows,columns,mines);
            for(int row = 0; row < rows; row++)
            {
                for(int column = 0; column < columns; column++)
                {
                    Cell cell = game.Board.GetAt(row, column);
                    if (cell.IsMine)
                    {
                        Console.Write("*");
                    }
                    else if(cell.SurroundingMines == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(cell.SurroundingMines);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
