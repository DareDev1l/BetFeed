using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services.Inferfaces;

namespace BetFeed.Services
{
    public class EventService : IEventService
    {
        private IRepository<Event> eventRepository;

        public EventService(IRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public void AddOrUpdate(Event eventToAdd)
        {
            this.eventRepository.AddOrUpdate(eventToAdd);
        }
    }
}
