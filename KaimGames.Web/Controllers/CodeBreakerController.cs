using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaimGames.Web.Models;
using KaimGames.CodeBreaker.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KaimGames.Web.Data;

namespace KaimGames.Web.Controllers
{
    [Authorize]
    public class CodeBreakerController : GameControllerBase
    {        
        public CodeBreakerController(ILogger<GameControllerBase> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db) :
            base("CodeBreaker", logger, userManager, db)
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

        public IActionResult Create(int codeLength, int codeOptionsLength)
        {
            this.SessionSet(this.SessionGameKey, Game.Create(codeLength, codeOptionsLength));
            return this.RedirectToAction("Show");
        }

        public IActionResult Show()
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game == null) { return this.RedirectToAction("Index"); }

            DateTime created = this.SessionGet<DateTime>(this.SessionGameStartedKey);
            if (!game.IsGameOver)
            {
                return View(new CodeBreakerGameViewModel(game, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds));
            }
            else
            {
                return View(new CodeBreakerGameViewModel(game, this.SessionGet<double>(this.SessionGameElapsedKey)));
            }
        }
               
        async public Task<IActionResult> Guess(char[] code)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);

            if (!game.Guesses.Any())
            {
                this.SessionSet(this.SessionGameStartedKey, DateTime.UtcNow);
            }

            game.Guess(code);
            this.SessionSet(this.SessionGameKey, game);

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
                        Moves = game.Guesses.Count,
                        Score = game.Guesses.Count,
                        Created = this.SessionGet<DateTime>(this.SessionGameStartedKey),
                        Completed = DateTime.UtcNow,
                        Elapsed = this.SessionGet<double>(this.SessionGameElapsedKey)
                    });
                await this.Db.SaveChangesAsync();
            }
        }
    }
}
