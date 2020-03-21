using System;

namespace KaimGames.Web.Models
{
    public class UserViewModel
    {
        public readonly ApplicationUser User;

        public UserViewModel(ApplicationUser user)
        {
            this.User = user;
        }
    }
}
