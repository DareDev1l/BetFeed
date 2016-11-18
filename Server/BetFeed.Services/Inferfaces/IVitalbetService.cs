using System.Threading.Tasks;

namespace BetFeed.Services.Inferfaces
{
    public interface IVitalbetService
    {
        Task UpdateSportsFeed();

        Task SeedSportsFeed();
    }
}
