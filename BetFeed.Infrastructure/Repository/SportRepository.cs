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

        public override void Update(Sport entity)
        {
            var originalEntity = this.GetById(entity.Id);

            // Eager load the hierarchy in entity
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
            
            var allBets = betRepository.GetAll();
            var allOdds = oddRepository.GetAll();
            
            foreach (var sportEvent in entity.Events)
            {
                var eventToAdd = new Event();
                
                if(!originalEntity.Events.Any(ev => ev.Id == sportEvent.Id))
                {
                    originalEntity.Events.Add(sportEvent);

                    foreach (var match in sportEvent.Matches)
                    {
                        var betsToReplace = new HashSet<Bet>();

                        foreach (var bet in match.Bets)
                        {
                            var oddsToReplace = new HashSet<Odd>();

                            if(allBets.Any(b => b.Id == bet.Id))
                            {
                                betsToReplace.Add(bet);
                            }

                            foreach (var odd in bet.Odds)
                            {
                                if(allOdds.Any(o => o.Id == odd.Id))
                                {
                                    oddsToReplace.Add(odd);
                                }
                            }

                            foreach (var oddToReplace in oddsToReplace)
                            {
                                bet.Odds.Remove(oddToReplace);
                                // Add the odd back 
                            }
                        }

                        // Rmove identical bets

                        foreach (var betToReplace in betsToReplace)
                        {
                            match.Bets.Remove(betToReplace);
                            // match.Bets.Add(allBets.ToList().Find(b => b.Id == betToReplace.Id));
                        }
                    }
                }
            }
           

            var entityInContext = this.dataContext.Entry(originalEntity);
            entityInContext.State = EntityState.Modified;
        }
    }
}
