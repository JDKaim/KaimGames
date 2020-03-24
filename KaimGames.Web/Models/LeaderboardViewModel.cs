using System;
using System.Collections.Generic;
using KaimGames.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace KaimGames.Web.Models
{
    public class LeaderboardViewModel
    {
        public string gameName { get; set; }
        public string subGame { get; set; }
        public string userDisplayName { get; set; }
        public int userId { get; set; }
        public bool hideDisplayName { get; set; }
        public bool hideGameName { get; set; }
        public bool hideSubGame { get; set; }
        public bool hideScore { get; set; }
        public bool hideElapsed { get; set; }
        public bool hideMoves { get; set; }
        public string orderBy { get; set; }
        public bool orderByDescending { get; set; }
        public DateTime? since { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }

        public LeaderboardViewModel()
        {
            this.page = 0;
            this.pageSize = 10;
            this.orderBy = "Completed";
        }

        public LeaderboardViewModel AdjustSince(DateTime since)
        {
            return new LeaderboardViewModel()
            {
                since = since,
                gameName = this.gameName,
                subGame = this.subGame,
                userDisplayName = this.userDisplayName,
                userId = this.userId,
                hideDisplayName = this.hideDisplayName,
                hideGameName = this.hideGameName,
                hideSubGame = this.hideSubGame,
                hideScore = this.hideScore,
                hideElapsed = this.hideElapsed,
                hideMoves = this.hideMoves,
                orderBy = this.orderBy,
                orderByDescending = this.orderByDescending,
                page = this.page,
                pageSize = this.pageSize
            };
        }
    }
}
