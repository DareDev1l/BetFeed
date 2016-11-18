using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services.Inferfaces;

namespace BetFeed.Services
{
    public class OddService : IOddService
    {
        private IRepository<Odd> oddRepository;

        public OddService(IRepository<Odd> oddRepository)
        {
            this.oddRepository = oddRepository;
        }
    }
}
