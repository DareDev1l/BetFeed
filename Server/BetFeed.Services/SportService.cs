using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services.Inferfaces;

namespace BetFeed.Services
{
    public class SportService : ISportService
    {
        private IRepository<Sport> sportRepository;

        public SportService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }
    }
}
