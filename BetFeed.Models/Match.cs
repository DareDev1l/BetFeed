using BetFeed.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models
{
    public class Match : BaseModel
    {
        public Match()
        {
            this.Bets = new HashSet<Bet>();
        }

        public DateTime StartDate { get; set; }

        // TODO: Check if you can find all possible match types and make it enum
        public string MatchType { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }

        //[ForeignKey("Event_Id")]
        //public int EventId { get; set; }
    }
}