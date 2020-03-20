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
    public class MinesweeperController : GameControllerBase
    {
        public MinesweeperController(ILogger<GameControllerBase> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db) :
            base("Minesweeper", logger, userManager, db)
        {
        }

        public ActionResult Show()
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game == null)
            {
                return this.RedirectToAction("Index");
            }
            if (game.IsPristine)
            {
                return View(new MinesweeperGameViewModel(game, 0.0));
            }
            DateTime created = this.SessionGet<DateTime>(this.SessionGameStartedKey);
            if (!game.IsGameOver)
            {
                return View(new MinesweeperGameViewModel(game, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds));
            }
            else
            {
                return View(new MinesweeperGameViewModel(game, this.SessionGet<double>(this.SessionGameElapsedKey)));
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
            this.SessionSet(this.SessionGameKey, new Game(rows, columns, mines));
            return this.RedirectToAction("Show");
        }

        async public Task<ActionResult> Mark(int row, int column)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game.IsGameOver)
            {
                return this.RedirectToAction("Show");
            }

            if (game.IsPristine)
            {
                this.SessionSet(this.SessionGameStartedKey, DateTime.UtcNow);
            }

            game.Mark(row, column, true);
            this.SessionSet(this.SessionGameKey, game);

            await this.ProcessIfEndOfGame(game);

            return this.RedirectToAction("Show");
        }

        async public Task<ActionResult> RevealSurroundings(int row, int column)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game.IsGameOver)
            {
                return this.RedirectToAction("Show");
            }

            game.RevealSurroundings(row, column);
            this.SessionSet(this.SessionGameElapsedKey, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            this.SessionSet(this.SessionGameKey, game);

            await this.ProcessIfEndOfGame(game);

            return this.RedirectToAction("Show");
        }

        async public Task ProcessIfEndOfGame(Game game)
        {
            if (game.IsWon)
            {
                this.SessionSet(this.SessionGameElapsedKey, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
                ApplicationUser user = await this.GetLoggedInUser();

                this.Db.CompletedGames.Add(
                    new CompletedGame()
                    {
                        User = user,
                        GameName = game.Name,
                        SubGame = game.SubGame,
                        Moves = game.Moves,
                        Score = game.Score,
                        Created = this.SessionGet<DateTime>(this.SessionGameStartedKey),
                        Completed = DateTime.UtcNow,
                        Elapsed = this.SessionGet<double>(this.SessionGameElapsedKey)
                    });
                await this.Db.SaveChangesAsync();
            }
            else if (game.IsLost)
            {
                this.SessionSet(this.SessionGameElapsedKey, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }
        }

        public ActionResult Flag(int row, int column)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (!game.IsGameOver)
            {
                game.SetFlag(row, column);
                this.SessionSet(this.SessionGameKey, game);
                this.SessionSet(this.SessionGameElapsedKey, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }

            return this.RedirectToAction("Show");
        }

        public ActionResult Clear(int row, int column)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (!game.IsGameOver)
            {
                game.ClearFlag(row, column);
                this.SessionSet(this.SessionGameKey, game);
                this.SessionSet(this.SessionGameElapsedKey, (DateTime.UtcNow - this.SessionGet<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }

            return this.RedirectToAction("Show");
        }
    }
}