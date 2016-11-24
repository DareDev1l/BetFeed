using BetFeed.Models.Base;
using System.Collections.Generic;

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

        //[ForeignKey("Match_Id")]
        //public int MatchId { get; set; }
    }
}