using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaimGames.Web.Models;
using KaimGames.Minesweeper.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using KaimGames.Web.Data;

namespace KaimGames.Web.Controllers
{
    [Authorize]
    public class MinesweeperController : Controller
    {
        const string SessionPrefix = "Minesweeper";

        private string SessionGameKey => $"{SessionPrefix}.Game";
        private string SessionGameStartedKey => $"{SessionPrefix}.GameStarted";
        private string SessionGameElapsedKey => $"{SessionPrefix}.GameElapsed";

        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _db;
        async public Task<ApplicationUser> GetLoggedInUser()
        {

            return await this._userManager.GetUserAsync(HttpContext.User);
        }

        public MinesweeperController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _logger = logger;
            this._userManager = userManager;
            this._db = db;
        }

        public ActionResult Show()
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (game == null)
            {
                return this.RedirectToAction("NewMedium");
            }
            if (game.IsPristine)
            {
                return View(new MinesweeperGameViewModel(game, 0.0));
            }
            DateTime created = this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey);
            if (!game.IsGameOver)
            {
                return View(new MinesweeperGameViewModel(game, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds));
            }
            else
            {
                return View(new MinesweeperGameViewModel(game, this.HttpContext.Session.Get<double>(this.SessionGameElapsedKey)));
            }


        }

        public ActionResult NewEasy()
        {
            return StartGame(8, 8, 6);
        }

        public ActionResult NewMedium()
        {
            return StartGame(10, 10, 10);
        }

        public ActionResult NewHard()
        {
            return StartGame(15, 15, 30);
        }
        public ActionResult NewExpert()
        {
            return StartGame(20, 20, 70);
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult StartGame(int rows, int columns, int mines)
        {
            this.HttpContext.Session.Set(this.SessionGameKey, new Game(rows, columns, mines));
            return this.RedirectToAction("Show");
        }

        async public Task<ActionResult> Mark(int row, int column)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (game.IsGameOver)
            {
                return this.RedirectToAction("Show");
            }

            if (game.IsPristine)
            {
                this.HttpContext.Session.Set(this.SessionGameStartedKey, DateTime.UtcNow);
            }

            game.Mark(row, column, true);
            this.HttpContext.Session.Set(this.SessionGameKey, game);


            if (game.IsWon)
            {
                this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
                ApplicationUser user = await this.GetLoggedInUser();

                this._db.CompletedGames.Add(
                    new CompletedGame()
                    {
                        User = user,
                        GameName = "Minesweeper",
                        SubGame = game.SubGame,
                        Moves = game.Moves,
                        Score = game.Score,
                        Created = this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey),
                        Completed = DateTime.UtcNow,
                        Elapsed = this.HttpContext.Session.Get<double>(this.SessionGameElapsedKey)
                    });
                await this._db.SaveChangesAsync();
            }
            if (game.IsLost)
            {
                this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }

            return this.RedirectToAction("Show");
        }

        async public Task<ActionResult> RevealSurroundings(int row, int column)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (game.IsGameOver)
            {
                return this.RedirectToAction("Show");
            }

            game.RevealSurroundings(row, column);
            this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            this.HttpContext.Session.Set(this.SessionGameKey, game);

            if (game.IsWon)
            {
                this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
                ApplicationUser user = await this.GetLoggedInUser();

                this._db.CompletedGames.Add(
                    new CompletedGame()
                    {
                        User = user,
                        GameName = "Minesweeper",
                        SubGame = game.SubGame,
                        Moves = game.Moves,
                        Score = game.Score,
                        Created = this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey),
                        Completed = DateTime.UtcNow,
                        Elapsed = this.HttpContext.Session.Get<double>(this.SessionGameElapsedKey)
                    });
                await this._db.SaveChangesAsync();
            }

            return this.RedirectToAction("Show");
        }

        public ActionResult Flag(int row, int column)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (!game.IsGameOver)
            {
                game.SetFlag(row, column);
                this.HttpContext.Session.Set(this.SessionGameKey, game);
                this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }

            return this.RedirectToAction("Show");
        }

        public ActionResult Clear(int row, int column)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (!game.IsGameOver)
            {
                game.ClearFlag(row, column);
                this.HttpContext.Session.Set(this.SessionGameKey, game);
                this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }

            return this.RedirectToAction("Show");
        }
    }
}