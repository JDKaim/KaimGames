using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.TicTacToe.Common
{
    public class CheatBot : IBot
    {
        public string Name => "Cheat Bot";

        public string Description => "Always moves center";

        public Tuple<int, int> GetMove(Game game)
        {
            return new Tuple<int, int>(1, 1);
        }
    }
}
