using System;
using System.Collections.Generic;
using KaimGames.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace KaimGames.Web.Models
{
    public class CompletedGamesViewModel
    {
        public readonly List<CompletedGame> CompletedGames;

        public bool HideDisplayName { get; set; }
        public bool HideGameName { get; set; }
        public bool HideSubType { get; set; }
        public bool HideScore { get; set; }
        public bool HideElapsed { get; set; }
        public bool HideMoves { get; set; }

        public CompletedGamesViewModel(List<CompletedGame> completedGames)
        {
            this.CompletedGames = completedGames;
        }
    }
}
