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
        public ActionResult Index(string gameName, string subGame, DateTime? since, int? hours)
        {
            var query = this.Db.CompletedGames.Include(item => item.User).Where((item) => (item.GameName == gameName) && (item.SubGame == subGame));
            if (hours.HasValue)
            {
                since = DateTime.UtcNow.AddHours(-hours.Value);
            }

            if (since.HasValue)
            {
                query = query.Where(item => item.Created >= since.Value);
            }
            query = query.OrderBy(item => item.Elapsed);
            query = query.Take(10);

            List<CompletedGame> games = query.ToList();

            return this.View(new LeaderboardViewModel(gameName, subGame, games));

        }
    }
}