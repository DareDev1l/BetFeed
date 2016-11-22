using BetFeed.Services.Inferfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BetFeed.Controllers
{
    public class VitalbetController : ApiController
    {
        private IVitalbetService vitalBetService;

        public VitalbetController(IVitalbetService vitalBetService)
        {
            this.vitalBetService = vitalBetService;
        }

        [HttpPost]
        public async Task UpdateFeed()
        {
            await this.vitalBetService.UpdateSportsFeed();
        }

        [HttpPost]
        public async Task SeedSportsFeed()
        {
            await this.vitalBetService.SeedSportsFeed();
        }
    }
}
