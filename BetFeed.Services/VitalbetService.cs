using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BetFeed.Services
{
    public class VitalbetService
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
                this.sportRepository.Add(sport);
            }
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
                    currentEvent.CategoryId = int.Parse(xmlReader.GetAttribute("CategoryId"));
                    currentEvent.IsLive = bool.Parse(xmlReader.GetAttribute("IsLive"));
                }

                if (xmlReader.Name.Equals("Match") && (xmlReader.NodeType == XmlNodeType.Element))
                {
                    if (!String.IsNullOrEmpty(currentMatch.Name))
                    {
                        currentEvent.Matches.Add(currentMatch);
                    }

                    currentMatch = new Match();

                    currentMatch.Id = int.Parse(xmlReader.GetAttribute("ID"));
                    currentMatch.Name = xmlReader.GetAttribute("Name");
                    currentMatch.StartDate = DateTime.Parse(xmlReader.GetAttribute("StartDate"));
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
                    currentOdd.SpecialBetValue = decimal.Parse(xmlReader.GetAttribute("SpecialBetValue"));
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
