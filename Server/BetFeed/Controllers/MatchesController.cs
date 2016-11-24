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

        // Returns new bets for given match since a given date
        [HttpGet]
        public IHttpActionResult NewBets(int matchId, DateTime after)
        {
            if (matchId == 0)
            {
                return BadRequest("You must pass match id!");
            }

            var match = this.matchRepository.GetById(matchId);

            if (match == null)
            {
                return NotFound();
            }

            var newBets = match.Bets.Where(bet => bet.UpdatedOn > after);
            match.Bets = newBets.ToList();

            var newBetsViewModel = Mapper.Map<Match, NewBetsViewModel>(match);

            return Json(newBetsViewModel);
        }

        [HttpPost]
        public IHttpActionResult AddBetToMatch(int betId, int matchId)
        {
            var match = this.matchRepository.GetById(matchId);

            var bet = new Bet();
            bet.Id = betId;
            bet.Name = string.Format("Bet {0}", betId);
            bet.Odds = new HashSet<Odd>();
            bet.Odds.Add(new Odd()
            {
                Id = betId + 11,
                Name = "RandomOdd " + betId,
                SpecialBetValue = betId % 12,
                Value = betId % 4 + 1,
            });

            match.Bets.Add(bet);

            this.matchRepository.Update(match);
            this.matchRepository.SaveChanges();

            return Ok();
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