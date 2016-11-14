using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models
{
    public class Match
    {
        public Match()
        {
            this.Bets = new HashSet<Bet>();
        }
        
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        // TODO: Check if you can find all possible match types and make it enum
        public string MatchType { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}