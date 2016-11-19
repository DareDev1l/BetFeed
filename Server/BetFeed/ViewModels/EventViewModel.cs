using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetFeed.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<MatchViewModel> Matches { get; set; }
    }
}