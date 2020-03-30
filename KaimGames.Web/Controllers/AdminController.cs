using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KaimGames.Web.Models;
using KaimGames.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace KaimGames.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._db = db;
            this._logger = logger;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            AdminViewModel vm = new AdminViewModel(this._db.Users.ToList());

            return View(vm);
        }

    }
}
