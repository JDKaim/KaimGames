using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.TicTacToe.Common
{
    public class RandomBot : IBot
    {
        public string Name => "Random Bot";

        public string Description => "Takes random square";

        private Random _random;

        public RandomBot()
        {
            this._random = new Random();
        }

        public Tuple<int, int> GetMove(Game game)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (game.Board.IsEmptyAt(row, column))
                    {
                        list.Add(new Tuple<int, int>(row, column));
                    }
                }
            }

            return list[this._random.Next(list.Count)];
        }
    }
}
