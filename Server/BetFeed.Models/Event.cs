using BetFeed.Models.Base;
using System.Collections.Generic;

namespace BetFeed.Models
{
    public class Event : BaseModel
    {
        public Event()
        {
            this.Matches = new HashSet<Match>();
        }

        public string CategoryName { get; set; }

        public bool IsLive { get; set; }

        public int CategoryId { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}