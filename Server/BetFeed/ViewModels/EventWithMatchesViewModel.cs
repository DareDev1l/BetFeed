using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetFeed.ViewModels
{
    public class EventWithMatchesViewModel
    {
        public EventWithMatchesViewModel()
        {
            this.RequestDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public ICollection<MatchViewModel> Matches { get; set; }

        public DateTime RequestDate { get; set; }
    }
}