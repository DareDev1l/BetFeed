using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BetFeed.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SportsController : ApiController
    {
        private IRepository<Sport> sportRepository;

        public SportsController(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            if(id == 0)
            {
                return BadRequest("You must pass id!");
            }

            var sport = this.sportRepository.GetById(id);

            if(sport == null)
            {
                return NotFound();
            }

            var sportViewModel = new SportViewModel();
            sportViewModel.Name = sport.Name;
            sportViewModel.Id = sport.Id;
            sportViewModel.Events = new HashSet<EventViewModel>();

            foreach (var sportEvent in sport.Events)
            {
                var eventViewModel = new EventViewModel();
                eventViewModel.Name = sportEvent.Name;
                eventViewModel.Matches = new HashSet<MatchViewModel>();
                eventViewModel.Id = sportEvent.Id;

                foreach (var match in sportEvent.Matches)
                {
                    eventViewModel.Matches.Add(new MatchViewModel()
                    {
                        Name = match.Name,
                        StartDate = match.StartDate
                    });
                }

                sportViewModel.Events.Add(eventViewModel);
            }

            return Json(sportViewModel);
        }
    }
}