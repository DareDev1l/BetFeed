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
using AutoMapper;

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

        // GET api/sports/sport/id
        [HttpGet]
        public IHttpActionResult Sport(int id)
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

            var sportViewModel = Mapper.Map<Sport, SportViewModel>(sport);

            foreach (var sportEvent in sportViewModel.Events)
            {
                if(!sportViewModel.Categories.Any(x => x.Name == sportEvent.CategoryName))
                {
                    var categoryViewModel = new CategoryViewModel();

                    categoryViewModel.Name = sportEvent.CategoryName;
                    categoryViewModel.Events.Add(sportEvent);

                    sportViewModel.Categories.Add(categoryViewModel);
                }
                else
                {
                    sportViewModel.Categories.First(x => x.Name == sportEvent.CategoryName).Events.Add(sportEvent);
                }
            }

            sportViewModel.Events = null;

            return Json(sportViewModel);
        }

        // GET api/Sports/NamesAndIds
        [HttpGet]
        public IHttpActionResult NamesAndIds()
        {
            var sports = this.sportRepository.GetAll();

            if (sports == null)
            {
                return NotFound();
            }

            var sportNamesAndIds = Mapper.Map<IEnumerable<Sport>, IEnumerable<SportWithNameAndId>>(sports);

            return Json(sportNamesAndIds);
        }
    }
}