using BetFeed.Models;

namespace BetFeed.Services.Inferfaces
{
    public interface IEventService
    {
        void AddOrUpdate(Event eventToAdd);
    }
}
