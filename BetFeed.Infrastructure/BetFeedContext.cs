using BetFeed.Models;
using System.Data.Entity;

namespace BetFeed.Infrastructure
{
    public class BetFeedContext : DbContext
    {
        public BetFeedContext()
            : base("BetFeedDb")
        {
        }

        public virtual IDbSet<Sport> Sports { get; set; }

        public virtual IDbSet<Bet> Bets { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Event> Events { get; set; }

        public virtual IDbSet<Match> Matches { get; set; }

        public virtual IDbSet<Odd> Odds { get; set; }

        public static BetFeedContext Create()
        {
            return new BetFeedContext();
        }
    }
}
