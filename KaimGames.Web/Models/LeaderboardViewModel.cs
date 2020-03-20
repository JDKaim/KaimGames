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
        public readonly string GameName;
        public readonly string SubGame;
        public LeaderboardViewModel(string gameName, string subGame, List<CompletedGame> completedGames)
        {
            this.CompletedGames = completedGames;
            this.GameName = gameName;
            this.SubGame = subGame;
        }
    }
}
