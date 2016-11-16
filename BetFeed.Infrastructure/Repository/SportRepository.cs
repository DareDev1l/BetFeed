using BetFeed.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetFeed.Infrastructure.Repository
{
    public class SportRepository : EfRepository<Sport>
    {
        protected BetFeedContext dataContext;
        protected readonly IDbSet<Sport> dbSet;
        private IRepository<Bet> betRepository;
        private IRepository<Odd> oddRepository;

        public SportRepository(BetFeedContext context, IRepository<Bet> betRepository, IRepository<Odd> oddRepository) : base(context)
        {
            this.dataContext = context;
            this.dbSet = this.dataContext.Set<Sport>();
            this.betRepository = betRepository;
            this.oddRepository = oddRepository;

            // Eager load bets
        }

        // TODO: load new bets and odds if matches are updated
        public override void Update(Sport entity)
        {
            var originalEntity = this.GetById(entity.Id);
            
            if(originalEntity == null)
            {
                this.Add(entity);
                return;
            }

            // Eager load the hierarchy in originalEntity
            foreach (var sportEvent in originalEntity.Events)
            {
                var matches = sportEvent.Matches;

                foreach (var match in matches)
                {
                    var bets = match.Bets;

                    foreach (var bet in bets)
                    {
                        var odds = bet.Odds;
                    }
                }
            }

            foreach (var sportEvent in entity.Events)
            {
                // If event exists in original entity check for matches to update
                if (originalEntity.Events.Any(ev => ev.Id == sportEvent.Id))
                {
                    var originalEntityEvent = originalEntity.Events.First(ev => ev.Id == sportEvent.Id);

                    foreach (var match in sportEvent.Matches)
                    {
                        if (!originalEntityEvent.Matches.Any(m => m.Id == match.Id))
                        {
                            originalEntityEvent.Matches.Add(match);
                        }
                    }
                }
                else
                {
                    originalEntity.Events.Add(sportEvent);
                }
            }

            var entityInContext = this.dataContext.Entry(originalEntity);
            entityInContext.State = EntityState.Modified;
        }
    }
}
