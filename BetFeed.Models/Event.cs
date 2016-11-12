using System.Collections.Generic;

namespace BetFeed.Models
{
    public class Event
    {
        public Event()
        {
            this.Matches = new HashSet<Match>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}