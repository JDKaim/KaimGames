using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KaimGames.Web.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [PersonalData, Required, StringLength(maximumLength: 3, MinimumLength = 3)]
        public string DisplayName { get; set; }
        public virtual List<CompletedGame> CompletedGames { get; set; }
    }
}
