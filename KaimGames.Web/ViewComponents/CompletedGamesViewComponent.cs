using KaimGames.Web.Data;
using KaimGames.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KaimGames.Web.ViewComponents
{
    public class CompletedGamesViewComponent : ViewComponent
    {
        private ApplicationDbContext _db;

        public CompletedGamesViewComponent(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            string gameName, string subGame, string userDisplayName, int userId,
            bool hideDisplayName, bool hideGameName, bool hideSubType, bool hideScore, bool hideElapsed, bool hideMoves,
            string orderBy, bool orderByDescending, DateTime? since = null, int page = 0, int pageSize = 10)
        {
            IQueryable<CompletedGame> query = this._db.CompletedGames.Include(item => item.User);

            if (!string.IsNullOrWhiteSpace(gameName))
            {
                query = query.Where(item => item.GameName == gameName);
            }

            if (!string.IsNullOrWhiteSpace(subGame))
            {
                query = query.Where(item => item.SubGame == subGame);
            }

            if (!string.IsNullOrWhiteSpace(userDisplayName))
            {
                query = query.Where(item => item.User.DisplayName == userDisplayName);
            }

            if (userId != 0)
            {
                query = query.Where(item => item.User.Id == userId);
            }

            if (since.HasValue)
            {
                query = query.Where(item => item.Completed > since);
            }

            switch (orderBy.ToLower())
            {
                case "Elapsed":
                    query = orderByDescending ? query.OrderByDescending(item => item.Elapsed) : query.OrderBy(item => item.Elapsed);
                    break;
                case "Moves":
                    query = orderByDescending ? query.OrderByDescending(item => item.Moves) : query.OrderBy(item => item.Moves);
                    break;
                case "Score":
                    query = orderByDescending ? query.OrderByDescending(item => item.Score) : query.OrderBy(item => item.Score);
                    break;
                // case: "Completed";
                default:
                    query = orderByDescending ? query.OrderByDescending(item => item.Completed) : query.OrderBy(item => item.Completed);
                    break;
            }

            var results = await query.Skip(pageSize * page).Take(pageSize).ToListAsync();

            return this.View(
                new CompletedGamesViewModel(results)
                {
                    HideDisplayName = hideDisplayName,
                    HideElapsed = hideElapsed,
                    HideGameName = hideGameName,
                    HideMoves = hideMoves,
                    HideScore = hideScore,
                    HideSubType = hideSubType
                });
        }

    }
}
