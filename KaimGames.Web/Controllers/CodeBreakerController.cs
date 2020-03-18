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

namespace KaimGames.Web.Controllers
{
    [Authorize]
    public class CodeBreakerController : Controller
    {
        const string SessionPrefix = "CodeBreaker";

        private string SessionGameKey => $"{SessionPrefix}.Game";

        private readonly ILogger<HomeController> _logger;

        public CodeBreakerController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateEasy()
        {
            return this.Create(3, 3);
        }

        public IActionResult CreateMedium()
        {
            return this.Create(5, 5);
        }

        public IActionResult CreateHard()
        {
            return this.Create(7, 7);
        }

        public IActionResult CreateExpert()
        {
            return this.Create(10, 10);
        }

        public IActionResult Create(int codeLength, int codeOptionsLength)
        {
            this.HttpContext.Session.Set(this.SessionGameKey, Game.Create(codeLength, codeOptionsLength));
            return this.RedirectToAction("Show");
        }

        public IActionResult Show()
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            if (game == null) { return this.RedirectToAction("Index"); }

            return this.View(new CodeBreakerGameViewModel(game));
        }

        public IActionResult Guess(char[] code)
        {
            Game game = this.HttpContext.Session.Get<Game>(this.SessionGameKey);
            game.Guess(code);
            this.HttpContext.Session.Set(this.SessionGameKey, game);

            return this.RedirectToAction("Show");
        }
    }
}
