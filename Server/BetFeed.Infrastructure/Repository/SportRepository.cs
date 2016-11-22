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

        public SportRepository(BetFeedContext context) : base(context)
        {
            this.dataContext = context;
            this.dbSet = this.dataContext.Set<Sport>();

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

            /*
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

                        foreach (var odd in odds)
                        {
                            var oddId = odd.Id;
                        }
                    }
                }
            }
            */

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
                        else
                        {
                            var originalEntityMatch = originalEntityEvent.Matches.First(m => m.Id == match.Id);

                            foreach (var bet in match.Bets)
                            {
                                if(!originalEntityMatch.Bets.Any(b => b.Id == bet.Id))
                                {
                                    originalEntityMatch.Bets.Add(bet);
                                }
                                else
                                {
                                    var originalEntityMatchBet = originalEntityMatch.Bets.First(b => b.Id == bet.Id);

                                    foreach (var odd in bet.Odds)
                                    {
                                        if (!originalEntityMatchBet.Odds.Any(o => o.Id == odd.Id))
                                        {
                                            originalEntityMatchBet.Odds.Add(odd);
                                        }
                                    }
                                }
                            }
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

        public override Sport GetById(int id)
        {
            return this.dbSet.Include("Events.Matches.Bets.Odds").FirstOrDefault(x => x.Id == id);
        }
    }
}
