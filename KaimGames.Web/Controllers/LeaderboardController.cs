using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaimGames.Web.Data;
using KaimGames.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaimGames.Web.Controllers
{
    public class LeaderboardController : Controller
    {
        protected readonly ApplicationDbContext Db;
        public LeaderboardController(ApplicationDbContext db)
        {
            this.Db = db;
        }
        public ActionResult Index(string gameName, string subGame, string userDisplayName, int userId,
            bool hideDisplayName, bool hideGameName, bool hideSubGame, bool hideScore, bool hideElapsed, bool hideMoves,
            string orderBy, bool orderByDescending, DateTime? since = null, int page = 0, int pageSize = 10)
        {
            return this.View(new LeaderboardViewModel()
            {
                gameName = gameName,
                subGame = subGame,
                userDisplayName = userDisplayName,
                userId = userId,
                hideDisplayName = hideDisplayName,
                hideGameName = hideGameName,
                hideSubGame = hideSubGame,
                hideScore = hideScore,
                hideElapsed = hideElapsed,
                hideMoves = hideMoves,
                orderBy = orderBy,
                orderByDescending = orderByDescending,
                since = since,
                page = page,
                pageSize = pageSize
            });

        }
    }
}