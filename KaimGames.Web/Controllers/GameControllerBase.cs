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
    public class GameControllerBase : Controller
    {
        protected readonly string GamePrefix;
        protected readonly ILogger<GameControllerBase> Logger;
        protected readonly UserManager<ApplicationUser> UserManager;
        protected readonly ApplicationDbContext Db;

        protected string SessionGameKey => $"{this.GamePrefix}.Game";
        protected string SessionGameStartedKey => $"{this.GamePrefix}.Started";
        protected string SessionGameElapsedKey => $"{this.GamePrefix}.Elapsed";

        protected T SessionGet<T>(string key) => this.HttpContext.Session.Get<T>(key);
        protected void SessionSet<T>(string key, T value) => this.HttpContext.Session.Set(key, value);

        async public Task<ApplicationUser> GetLoggedInUser()
        {
            return await this.UserManager.GetUserAsync(this.HttpContext.User);
        }

        public GameControllerBase(string gamePrefix, ILogger<GameControllerBase> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this.Db = db;
            this.GamePrefix = gamePrefix;
            this.Logger = logger;
            this.UserManager = userManager;
        }
    }
}
