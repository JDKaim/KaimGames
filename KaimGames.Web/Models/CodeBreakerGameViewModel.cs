using KaimGames.CodeBreaker.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KaimGames.Web.Models
{
    public class CodeBreakerGameViewModel
    {
        public readonly Game Game;
        public readonly char[] PreviousCode;
        public readonly List<SelectList> CodeOptionSelectLists;

        public readonly Dictionary<char, string> CodeOptionMap =
            new Dictionary<char, string>()
            {
                //{ '1', "&#xf1fd;" },
                //{ '2', "&#xf1d8;" },
                //{ '3', "&#xf091;" },
                //{ '4', "&#xf1e2;" },
                //{ '5', "&#xf140;" },
                //{ '6', "&#xf188;" },
                //{ '7', "&#xf13d;" },
                //{ '8', "&#xf1fb;" },
                //{ '9', "&#xf084;" },
                //{ '0', "&#xf1ae;" },
                { '1', "1" },
                { '2', "2" },
                { '3', "3" },
                { '4', "4" },
                { '5', "5" },
                { '6', "6" },
                { '7', "7" },
                { '8', "8" },
                { '9', "9" },
                { '0', "0" },
            };

        public CodeBreakerGameViewModel(Game game)
        {
            this.Game = game;

            if (game.Guesses.Any())
            {
                this.PreviousCode = game.Guesses.Last().Code;
            }
            else
            {
                this.PreviousCode = new char[game.CodeLength];
                for (int lcv = 0; lcv < game.CodeLength; lcv++)
                {
                    this.PreviousCode[lcv] = game.CodeOptions.First();
                }
            }

            this.CodeOptionSelectLists = new List<SelectList>();

            var selectOptions = this.Game.CodeOptions.Select(item => new { Id = item, Name = this.CodeOptionMap[item] });

            for (int lcv = 0; lcv < this.Game.CodeLength; lcv++)
            {
                this.CodeOptionSelectLists.Add(new SelectList(selectOptions, "Id", "Name", this.PreviousCode[lcv]));
            }
        }
    }
}
