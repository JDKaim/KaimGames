using KaimGames.Minesweeper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaimGames.Web.Models
{
    public class MinesweeperGameViewModel
    {
        public readonly Game Game;
        public readonly double Elapsed;

        public MinesweeperGameViewModel(Game game, double elapsed)
        {
            this.Game = game;
            this.Elapsed = elapsed;
        }
    }
}