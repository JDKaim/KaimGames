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
using Microsoft.AspNetCore.Identity;
using KaimGames.Web.Data;

namespace KaimGames.Web.Controllers
{
    [Authorize]
    public class TicTacToeController : GameControllerBase
    {
        private string SessionBotKey => $"{this.GamePrefix}.Bot";

        public TicTacToeController(ILogger<GameControllerBase> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db) :
            base("TicTacToe", logger, userManager, db)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(string bot)
        {
            Game game = new Game();
            this.SessionSet(this.SessionGameKey, game);
            this.SessionSet(this.SessionBotKey, bot);

            return this.RedirectToAction("Show");
        }

        public IActionResult Show()
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            if (game == null) { return this.RedirectToAction("Index"); }

            return this.View(new TicTacToeGameViewModel(game, this.SessionGet<string>(this.SessionBotKey)));
        }

        public IActionResult Mark(int row, int column)
        {
            Game game = this.SessionGet<Game>(this.SessionGameKey);
            game.Mark(row, column);

            if (!game.IsOver)
            {
                IBot bot;
                switch (this.SessionGet<string>(this.SessionBotKey))
                {
                    case "RandomBot": bot = new RandomBot(); break;
                    case "MinMaxBot": bot = new MinMaxBot(); break;
                    default: throw new ArgumentException("Unsupported bot!");
                }

                Tuple<int, int> move = bot.GetMove(game.Copy());
                game.Mark(move.Item1, move.Item2);
            }

            this.SessionSet(this.SessionGameKey, game);

            return this.RedirectToAction("Show");
        }
    }
}
