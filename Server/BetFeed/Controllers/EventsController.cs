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

        // Returns new matches for given event since a given date
        [HttpGet]
        public IHttpActionResult NewMatches(int eventId, DateTime after)
        {
            if (eventId == 0)
            {
                return BadRequest("You must pass match id!");
            }

            var sportEvent = this.eventRepository.GetById(eventId);

            if (sportEvent == null)
            {
                return NotFound();
            }

            var newMatches = sportEvent.Matches.Where(match => match.UpdatedOn > after);
            sportEvent.Matches = newMatches.ToList();

            var newMatchesViewModel = Mapper.Map<Event, NewMatchesViewModel>(sportEvent);

            return Json(newMatchesViewModel);
        }

        // Returns new matches for given event since a given date
        [HttpPost]
        public IHttpActionResult AddMatchToEvent(int matchId, int eventId)
        {
            var sportEvent = this.eventRepository.GetById(eventId);

            var match = new Match();
            match.Id = matchId;
            match.MatchType = "PreMatch";
            match.StartDate = DateTime.Now.AddHours(2);
            match.Name = string.Format("Team1{0} - Team2{0}", matchId);

            sportEvent.Matches.Add(match);

            this.eventRepository.Update(sportEvent);
            this.eventRepository.SaveChanges();

            return Ok();
        }
    }
}
