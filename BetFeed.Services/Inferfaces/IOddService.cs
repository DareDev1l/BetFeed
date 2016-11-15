using BetFeed.Models;

namespace BetFeed.Services.Inferfaces
{
    public interface IOddService
    {
        void AddOrUpdate(Odd odd);
    }
}
