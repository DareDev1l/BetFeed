using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services.Inferfaces;

namespace BetFeed.Services
{
    public class MatchService : IMatchService
    {
        private IRepository<Match> matchRepository;

        public MatchService(IRepository<Match> matchRepository)
        {
            this.matchRepository = matchRepository;
        }
    }
}
