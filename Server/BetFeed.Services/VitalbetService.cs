using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Services.Inferfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;

namespace BetFeed.Services
{
    public class VitalbetService : IVitalbetService
    {
        private string sportsFeedXmlUri = "http://vitalbet.net/sportxml";
        private IRepository<Sport> sportRepository;
        
        public VitalbetService(IRepository<Sport> sportRepository)
        {
            this.sportRepository = sportRepository;
        }

        public async Task UpdateSportsFeed()
        {            
            var sportsList = await this.ParseSportsFeedFromXml();

            foreach (var sport in sportsList)
            {
                sportRepository.Update(sport);
                sportRepository.SaveChanges();
            }
        }

        public async Task SeedSportsFeed()
        {
            var sportsList = await this.ParseSportsFeedFromXml();

            foreach (var sport in sportsList)
            {
                sportRepository.Add(sport);
                sportRepository.SaveChanges();
            }
        }

        private async Task<List<Sport>> ParseSportsFeedFromXml()
        {
            var xmlString = await this.GetSportsXml();

            var xmlReaderSettings = new XmlReaderSettings();

            xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString), xmlReaderSettings);

            var sportsList = new List<Sport>();
            var currentSport = new Sport();
            var currentEvent = new Event();
            var currentMatch = new Match();
            var currentBet = new Bet();
            var currentOdd = new Odd();

            bool currentMatchIsWithin24Hours = true;

            while (xmlReader.Read())
            {
                if (xmlReader.Name.Equals("Sport") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    currentSport = new Sport();
                    currentSport.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentSport.Name = xmlReader.GetAttribute("Name");

                    continue;
                }
                else if (xmlReader.Name.Equals("Sport") && (xmlReader.NodeType == XmlNodeType.EndElement))
                {
                    sportsList.Add(currentSport);
                    continue;
                }

                if (xmlReader.Name.Equals("Event") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    currentEvent = new Event();

                    var nameParts = xmlReader.GetAttribute("Name").Split(',');

                    currentEvent.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentEvent.CategoryName = nameParts[0];
                    currentEvent.Name = nameParts[1];
                    currentEvent.CategoryId = int.Parse(xmlReader.GetAttribute("CategoryID"));
                    currentEvent.IsLive = bool.Parse(xmlReader.GetAttribute("IsLive"));

                    continue;
                }
                else if (xmlReader.Name.Equals("Event") && (xmlReader.NodeType == XmlNodeType.EndElement))
                {
                    currentSport.Events.Add(currentEvent);

                    continue;
                }

                if (xmlReader.Name.Equals("Match") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    currentMatch = new Match();

                    currentMatch.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentMatch.Name = xmlReader.GetAttribute("Name");
                    currentMatch.StartDate = DateTime.Parse(xmlReader.GetAttribute("StartDate"));
                    currentMatch.MatchType = xmlReader.GetAttribute("MatchType");

                    var tomorrow = DateTime.Now.AddDays(1);
                    var now = DateTime.Now;

                    if (currentMatch.StartDate > tomorrow || currentMatch.StartDate < now)
                    {
                        currentMatchIsWithin24Hours = false;
                    }
                    else
                    {
                        currentMatchIsWithin24Hours = true;
                    }

                    continue;
                }
                else if (xmlReader.Name.Equals("Match") && (xmlReader.NodeType == XmlNodeType.EndElement))
                {
                    if (currentMatchIsWithin24Hours == true)
                    {
                        currentEvent.Matches.Add(currentMatch);
                    }

                    continue;
                }

                if (xmlReader.Name.Equals("Bet") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    currentBet = new Bet();

                    currentBet.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentBet.Name = xmlReader.GetAttribute("Name");
                    currentBet.IsLive = bool.Parse(xmlReader.GetAttribute("IsLive"));

                    continue;
                }
                else if (xmlReader.Name.Equals("Bet") && (xmlReader.NodeType == XmlNodeType.EndElement))
                {
                    currentMatch.Bets.Add(currentBet);

                    continue;
                }

                if (xmlReader.Name.Equals("Odd") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    currentOdd = new Odd();

                    currentOdd.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentOdd.Name = xmlReader.GetAttribute("Name");
                    currentOdd.Value = decimal.Parse(xmlReader.GetAttribute("Value"));

                    if (xmlReader.GetAttribute("SpecialBetValue") != null)
                    {
                        currentOdd.SpecialBetValue = xmlReader.GetAttribute("SpecialBetValue");
                    }

                    currentBet.Odds.Add(currentOdd);

                    continue;
                }
            }

            return sportsList;
        }

        private async Task<string> GetSportsXml()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(this.sportsFeedXmlUri);
            }
        }
    }
}