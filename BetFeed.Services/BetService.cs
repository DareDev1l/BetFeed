using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services.Inferfaces;

namespace BetFeed.Services
{
    public class BetService : IBetService
    {
        private IRepository<Bet> betRepository;

        public BetService(IRepository<Bet> betRepository)
        {
            this.betRepository = betRepository;
        }
    }
}
