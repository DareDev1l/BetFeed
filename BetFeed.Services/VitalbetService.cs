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
        
        public async Task GetSportsFeed()
        {
            var sportsList = await this.ParseSportsFeedFromXml();

            foreach (var sport in sportsList)
            {
                this.sportRepository.Add(sport);
            }

            this.sportRepository.SaveChanges();
        }

        private async Task<List<Sport>> ParseSportsFeedFromXml()
        {
            var xmlString = await this.GetSportsXml();
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlString));

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
                    if (!String.IsNullOrEmpty(currentSport.Name))
                    {
                        sportsList.Add(currentSport);
                    }

                    currentSport = new Sport();
                    currentSport.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentSport.Name = xmlReader.GetAttribute("Name");
                }

                if (xmlReader.Name.Equals("Event") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    if(!String.IsNullOrEmpty(currentEvent.Name))
                    {
                        currentSport.Events.Add(currentEvent);
                    }

                    currentEvent = new Event();

                    currentEvent.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentEvent.Name = xmlReader.GetAttribute("Name");
                    currentEvent.CategoryId = int.Parse(xmlReader.GetAttribute("CategoryID"));
                    currentEvent.IsLive = bool.Parse(xmlReader.GetAttribute("IsLive"));
                }

                if (xmlReader.Name.Equals("Match") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    if (!String.IsNullOrEmpty(currentMatch.Name))
                    {
                        if(currentMatchIsWithin24Hours == true)
                        {
                            currentEvent.Matches.Add(currentMatch);
                        }
                    }

                    currentMatch = new Match();

                    currentMatch.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentMatch.Name = xmlReader.GetAttribute("Name");
                    currentMatch.StartDate = DateTime.Parse(xmlReader.GetAttribute("StartDate"));

                    var tomorrow = DateTime.Now.AddDays(1);
                    var now = DateTime.Now;

                    if(currentMatch.StartDate > tomorrow || currentMatch.StartDate < now)
                    {
                        currentMatchIsWithin24Hours = false;
                    }
                    else
                    {
                        currentMatchIsWithin24Hours = true;
                    }

                    currentMatch.MatchType = xmlReader.GetAttribute("MatchType");
                }

                if (xmlReader.Name.Equals("Bet") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    if (!String.IsNullOrEmpty(currentBet.Name))
                    {
                        currentMatch.Bets.Add(currentBet);
                    }

                    currentBet = new Bet();

                    currentBet.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentBet.Name = xmlReader.GetAttribute("Name");
                    currentBet.IsLive = bool.Parse(xmlReader.GetAttribute("IsLive"));
                }

                if (xmlReader.Name.Equals("Odd") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    if (!String.IsNullOrEmpty(currentOdd.Name))
                    {
                        currentBet.Odds.Add(currentOdd);
                    }

                    currentOdd = new Odd();

                    currentOdd.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentOdd.Name = xmlReader.GetAttribute("Name");
                    currentOdd.Value = decimal.Parse(xmlReader.GetAttribute("Value"));

                    if(xmlReader.GetAttribute("SpecialBetValue") != null)
                    {
                        currentOdd.SpecialBetValue = decimal.Parse(xmlReader.GetAttribute("SpecialBetValue"));
                    }
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