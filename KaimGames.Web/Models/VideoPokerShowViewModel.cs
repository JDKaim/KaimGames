using KaimGames.VideoPoker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaimGames.Web.Models
{
    public class VideoPokerShowViewModel
    {
        public readonly Game Game;
        public readonly double Elapsed;
        public readonly BestHand BestHand;

        public VideoPokerShowViewModel(Game game, double elapsed, BestHand bestHand)
        {
            this.Game = game;
            this.Elapsed = elapsed;
            this.BestHand = bestHand;
        }
    }
}