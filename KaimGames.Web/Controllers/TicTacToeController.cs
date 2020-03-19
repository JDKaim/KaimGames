using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaimGames.Web.Models;
using KaimGames.TicTacToe.Common;
using Microsoft.AspNetCore.Authorization;

namespace KaimGames.Web.Controllers
{
    [Authorize]
    public class TicTacToeController : Controller
    {
        const string SessionPrefix = "TicTacToe";

        private string SessionGameKey => $"{SessionPrefix}.Game";
        private string SessionBotKey => $"{SessionPrefix}.Bot";

        private readonly ILogger<HomeController> _logger;

        public TicTacToeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(string bot)
        {
            Game game = new Game();
            this.HttpContext.Session.Set(this.SessionGameKey, game);
            this.HttpContext.Session.Set(this.SessionBotKey, bot);

            return this.RedirectToAction("Show");
        }

        public IActionResult Show()
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (game == null) { return this.RedirectToAction("Index"); }

            return this.View(new TicTacToeGameViewModel(game));
        }

        public IActionResult Mark(int row, int column)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            game.Mark(row, column);

            if (!game.IsOver)
            {
                IBot bot;
                switch (this.HttpContext.Session.Get<string>(this.SessionBotKey))
                {
                    case "RandomBot": bot = new RandomBot(); break;
                    case "MinMaxBot": bot = new MinMaxBot(); break;
                    default: throw new ArgumentException("Unsupported bot!");
                }

                Tuple<int, int> move = bot.GetMove(game.Copy());
                game.Mark(move.Item1, move.Item2);
            }

            this.HttpContext.Session.Set(this.SessionGameKey, game);

            return this.RedirectToAction("Show");
        }
    }
}
