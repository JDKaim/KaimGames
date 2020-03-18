using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KaimGames.Web.Models
{
    public class CompletedGame
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Created { get; set; }
        public DateTime Completed { get; set; }
        public string GameName { get; set; }
        public double Elapsed { get; set; }
        public string SubGame { get; set; }
        public int Moves { get; set; }
        public int Score { get; set; }
    }
}
