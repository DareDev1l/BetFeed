using System.Collections.Generic;

namespace BetFeed.Models
{
    public class Bet
    {
        public Bet()
        {
            this.Odds = new HashSet<Odd>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public ICollection<Odd> Odds { get; set; }
    }
}