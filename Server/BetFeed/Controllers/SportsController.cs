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

            var sportViewModel = Mapper.Map<Sport, SportViewModel>(sport);

            return Json(sportViewModel);
        }
    }
}