using System;
using System.Collections.Generic;

namespace KaimGames.Web.Models
{
    public class AdminViewModel
    {
        public readonly List<ApplicationUser> Users;

        public AdminViewModel(List<ApplicationUser> users)
        {
            this.Users = users;
        }
    }
}
