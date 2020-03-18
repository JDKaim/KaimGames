using System;
using System.Collections.Generic;
using KaimGames.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace KaimGames.Web.Models
{
    public class LeaderboardViewModel
    {
        public readonly List<CompletedGame> CompletedGames;
        public LeaderboardViewModel(List<CompletedGame> completedGames)
        {
            this.CompletedGames = completedGames;
        }
    }
}
