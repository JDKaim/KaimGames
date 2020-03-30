using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.TicTacToe.Common
{
    public class FirstBot : IBot
    {
        public string Name => "First Bot";

        public string Description => "Takes first available square";

        public Tuple<int, int> GetMove(Game game)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (game.Board.IsEmptyAt(row, column))
                    {
                        return new Tuple<int, int>(row, column);
                    }
                }
            }

            throw new Exception("No spots open!");
        }
    }
}
