using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaimGames.Web.Models;
using KaimGames.Minesweeper.Common;

namespace KaimGames.Web.Controllers
{
    public class MinesweeperController : Controller
    {
        const string SessionPrefix = "Minesweeper";

        private string SessionGameKey => $"{SessionPrefix}.Game";
        private string SessionGameStartedKey => $"{SessionPrefix}.GameStarted";
        private string SessionGameElapsedKey => $"{SessionPrefix}.GameElapsed";

        private readonly ILogger<HomeController> _logger;

        public MinesweeperController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        public ActionResult Mark(int row, int column)
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
                //this.LoggedInUser.CompletedGames.Add(
                //    new CompletedGame()
                //    {
                //        Columns = game.Board.Columns,
                //        Rows = game.Board.Rows,
                //        Mines = game.Mines,
                //        Moves = game.Moves,
                //        Created = (DateTime)this.Session["GameStarted"],
                //        Elapsed = (double)this.Session["Elapsed"]
                //    });
                //this.UserManager.Update(this.LoggedInUser);

            }
            if (game.IsLost)
            {
                this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            }

            return this.RedirectToAction("Show");
        }

        public ActionResult RevealSurroundings(int row, int column)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (game.IsGameOver)
            {
                return this.RedirectToAction("Show");
            }

            game.RevealSurroundings(row, column);
            this.HttpContext.Session.Set(this.SessionGameElapsedKey, (DateTime.UtcNow - this.HttpContext.Session.Get<DateTime>(this.SessionGameStartedKey)).TotalMilliseconds);
            this.HttpContext.Session.Set(this.SessionGameKey, game);

            //if (game.IsWon)
            //{
            //    this.LoggedInUser.CompletedGames.Add(
            //        new CompletedGame()
            //        {
            //            Columns = game.Board.Columns,
            //            Rows = game.Board.Rows,
            //            Mines = game.Mines,
            //            Moves = game.Moves,
            //            Created = (DateTime)this.Session["GameStarted"],
            //            Elapsed = (double)this.Session["Elapsed"]
            //        });
            //    this.UserManager.Update(this.LoggedInUser);
            //}

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