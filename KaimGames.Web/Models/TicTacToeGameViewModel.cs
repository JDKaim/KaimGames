using KaimGames.TicTacToe.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaimGames.Web.Models
{
    public class TicTacToeGameViewModel
    {
        public readonly Game Game;
        public readonly string BotName;
        public TicTacToeGameViewModel(Game game, string botName)
        {
            this.Game = game;
            this.BotName = botName;
        }
    }
}