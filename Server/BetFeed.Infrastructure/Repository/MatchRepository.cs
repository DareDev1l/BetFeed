using BetFeed.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetFeed.Infrastructure.Repository
{
    public class MatchRepository : EfRepository<Match>
    {
        protected BetFeedContext dataContext;
        protected readonly IDbSet<Match> dbSet;

        public MatchRepository(BetFeedContext context) : base(context)
        {
            this.dataContext = context;
            this.dbSet = this.dataContext.Set<Match>();
        }

        public override Match GetById(int id)
        {
            return this.dbSet.Include
                (match => match.Bets.Select
                (bet => bet.Odds))
                .FirstOrDefault(x => x.Id == id);
        }
    }
}