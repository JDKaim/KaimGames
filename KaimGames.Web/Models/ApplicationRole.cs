using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace KaimGames.Web.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() { }

        public ApplicationRole(string name)
        {
            this.Name = name;
        }
    }
}
