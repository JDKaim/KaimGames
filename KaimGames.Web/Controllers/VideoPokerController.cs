using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaimGames.Web.Models;
using KaimGames.Common;
using KaimGames.VideoPoker.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KaimGames.Web.Data;

namespace KaimGames.Web.Controllers
{
    [Authorize]
    public class VideoPokerController : GameControllerBase
    {
        private string SessionBestHandKey => $"{this.GamePrefix}.BestHand";

        public VideoPokerController(ILogger<GameControllerBase> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db) :
            base("VideoPoker", logger, userManager, db)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateSubGame(string subGame)
        {
            int[] parts = subGame.Split('-').Select(item => int.Parse(item)).ToArray();
            return this.Create(parts[0], parts[1]);
        }

        public IActionResult Create(int handSize, int totalRounds)
        {
            this.SessionSet(this.SessionGameKey, new Game(handSize, totalRounds));
            this.SessionSet(this.SessionGameStartedKey, DateTime.UtcNow);
            this.SessionSet(this.SessionBestHandKey, (BestHand) null);
            return this.RedirectToAction("Show");
        }

        public IActionResult Show()
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game == null) { return this.RedirectToAction("Index"); }

            DateTime created = this.SessionGet<DateTime>(this.SessionGameStartedKey);
            if (!game.IsGameOver)
            {
                return View(new VideoPokerShowViewModel(game, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds, this.SessionGet<BestHand>(this.SessionBestHandKey)));
            }
            else
            {
                return View(new VideoPokerShowViewModel(game, this.SessionGet<double>(this.SessionGameElapsedKey), this.SessionGet<BestHand>(this.SessionBestHandKey)));
            }
        }

        public IActionResult Deal()
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game.IsGameOver) { return this.RedirectToAction("Show"); }

            game.Deal();
            this.SessionSet(this.SessionGameKey, game);
            this.SessionSet(this.SessionBestHandKey, (BestHand)null);

            return this.RedirectToAction("Show");
        }

        async public Task<IActionResult> Keep(int[] cardIndexes)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            BestHand bestHand = game.Keep(cardIndexes.Where(item => item >= 0).Select(item => game.CurrentHand.Cards[item]));

            this.SessionSet(this.SessionGameKey, game);
            this.SessionSet(this.SessionBestHandKey, bestHand);

            await this.ProcessIfEndOfGame(game);

            return this.RedirectToAction("Show");
        }

        async public Task ProcessIfEndOfGame(Game game)
        {
            if (game.IsGameOver)
            {
                this.SessionSet(this.SessionGameElapsedKey, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
                ApplicationUser user = await this.GetLoggedInUser();

                this.Db.CompletedGames.Add(
                    new CompletedGame()
                    {
                        User = user,
                        GameName = game.Name,
                        SubGame = game.SubGame,
                        Moves = game.TotalRounds,
                        Score = game.Score,
                        Created = this.SessionGet<DateTime>(this.SessionGameStartedKey),
                        Completed = DateTime.UtcNow,
                        Elapsed = this.SessionGet<double>(this.SessionGameElapsedKey)
                    });
                await this.Db.SaveChangesAsync();
            }
        }
    }
}
