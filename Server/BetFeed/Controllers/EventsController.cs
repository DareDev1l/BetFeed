using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using System.Web.Http.Cors;
using BetFeed.ViewModels;
using BetFeed.Helpers;

namespace BetFeed.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventsController : ApiController
    {
        private IRepository<Event> eventRepository;

        public EventsController(IRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        [HttpGet]
        public IHttpActionResult Event(int id)
        {
            if (id == 0)
            {
                return BadRequest("You must pass id!");
            }

            var sportEvent = this.eventRepository.GetById(id);

            if (sportEvent == null)
            {
                return NotFound();
            }

            // Get only matches from today
            var matchesToRemove = new HashSet<Match>();
            
            foreach (var match in sportEvent.Matches)
            {
                if (!DateHelper.IsWithin24Hours(match.StartDate) && match.Bets.Count > 0)
                {
                    matchesToRemove.Add(match);
                }
            }

            foreach (var matchToRemove in matchesToRemove)
            {
                sportEvent.Matches.Remove(matchToRemove);
            }
            
            var eventViewModel = Mapper.Map<Event, EventWithMatchesViewModel>(sportEvent);

            return Json(eventViewModel);
        }
    }
}
