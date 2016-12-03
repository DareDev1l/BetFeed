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
        }
        
        public override void Update(Sport entity)
        {
            var originalEntity = this.GetById(entity.Id);
            
            if(originalEntity == null)
            {
                this.Add(entity);
                return;
            }

            foreach (var sportEvent in entity.Events)
            {
                // If event exists in original entity check for matches to update
                if (!originalEntity.Events.Any(ev => ev.Id == sportEvent.Id))
                {
                    originalEntity.Events.Add(sportEvent);
                }
                else
                {
                    var originalEntityEvent = originalEntity.Events.First(ev => ev.Id == sportEvent.Id);

                    GetNewMatches(ref originalEntityEvent, sportEvent);
                }
            }

            var entityInContext = this.dataContext.Entry(originalEntity);
            entityInContext.State = EntityState.Modified;
        }

        public override Sport GetById(int id)
        {
            return this.dbSet.Include("Events.Matches.Bets.Odds").FirstOrDefault(x => x.Id == id);
        }

        private void GetNewOdds(ref Bet originalBet, Bet newBet)
        {
            foreach (var odd in newBet.Odds)
            {
                if (!originalBet.Odds.Any(o => o.Id == odd.Id))
                {
                    originalBet.Odds.Add(odd);
                }
            }
        }

        private void GetNewBets(ref Match originalMatch, Match newMatch)
        {
            foreach (var bet in newMatch.Bets)
            {
                if (!originalMatch.Bets.Any(b => b.Id == bet.Id))
                {
                    originalMatch.Bets.Add(bet);
                }
                else
                {
                    var originalEntityMatchBet = originalMatch.Bets.First(b => b.Id == bet.Id);

                    GetNewOdds(ref originalEntityMatchBet, bet);
                }
            }
        }

        private void GetNewMatches(ref Event originalEvent, Event newEvent)
        {
            foreach (var match in newEvent.Matches)
            {
                if (!originalEvent.Matches.Any(m => m.Id == match.Id))
                {
                    originalEvent.Matches.Add(match);
                }
                else
                {
                    var originalEntityMatch = originalEvent.Matches.First(m => m.Id == match.Id);

                    GetNewBets(ref originalEntityMatch, match);
                }
            }
        }
    }
}
