using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace KaimGames.Web.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [PersonalData, Required, StringLength(maximumLength: 3, MinimumLength = 3)]
        public string DisplayName { get; set; }
    }
}
