using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaimGames.TicTacToe.Common
{
    public interface IBot
    {
        string Name { get; }
        string Description { get; }

        Tuple<int, int> GetMove(Game game);
    }
}
