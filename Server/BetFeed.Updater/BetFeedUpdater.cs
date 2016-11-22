using BetFeed.Infrastructure;
using BetFeed.Infrastructure.Repository;
using BetFeed.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BetFeed.Updater
{
    public class BetFeedUpdater
    {
        public static void Main(string[] args)
        {
            var timer = new Timer(async e => await UpdateOdds(), null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            Console.ReadLine();
        }

        // Call UpdateFeed method from vitalbet controller via httpclient post
        private static async Task UpdateOdds()
        {
            var betFeedContext = new BetFeedContext();
            var sportRepository = new SportRepository(betFeedContext);
            var vitalBetService = new VitalbetService(sportRepository);

            await vitalBetService.UpdateSportsFeed();
            Console.WriteLine("Updated on {0}", DateTime.Now);
        }
    }
}
