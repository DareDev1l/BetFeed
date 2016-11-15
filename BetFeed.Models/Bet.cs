using BetFeed.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models
{
    public class Bet : BaseModel
    {
        public Bet()
        {
            this.Odds = new HashSet<Odd>();
        }

        public bool IsLive { get; set; }

        public ICollection<Odd> Odds { get; set; }
    }
}