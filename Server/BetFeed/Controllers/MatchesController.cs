using AutoMapper;
using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BetFeed.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MatchesController : ApiController
    {
        private IRepository<Match> matchRepository;

        public MatchesController(IRepository<Match> matchRepository)
        {
            this.matchRepository = matchRepository;
        }

        [HttpGet]
        public IHttpActionResult Match(int id)
        {
            if (id == 0)
            {
                return BadRequest("You must pass id!");
            }

            var match = this.matchRepository.GetById(id);

            if (match == null)
            {
                return NotFound();
            }
            
            var matchViewModel = Mapper.Map<Match, MatchWithBetsViewModel>(match);

            // If name of a model is "1" or "2", change it to the competitor's name
            foreach (var bet in matchViewModel.Bets)
            {
                foreach (var odd in bet.Odds)
                {
                    var hasDashesInPlayerName = matchViewModel.Name.Split('-').Length > 2;

                    if (odd.Name == "1" && !hasDashesInPlayerName)
                    {
                        odd.Name = matchViewModel.First;
                    }

                    if(odd.Name == "2" && !hasDashesInPlayerName)
                    {
                        odd.Name = matchViewModel.Second;
                    }
                }
            }

            return Json(matchViewModel);
        }

        public IHttpActionResult ByName(string name)
        {
            if(String.IsNullOrEmpty(name))
            {
                return BadRequest("You must pass a name.");
            }

            var matchesByName = this.matchRepository.GetMany(match => match.Name.Contains(name));

            return Json(matchesByName);
        }
    }
}