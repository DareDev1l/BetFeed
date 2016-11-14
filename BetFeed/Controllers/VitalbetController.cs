using BetFeed.Services.Inferfaces;
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
            await this.vitalBetService.GetSportsFeed();
        }
    }
}
