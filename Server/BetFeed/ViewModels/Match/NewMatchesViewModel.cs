using System;
using System.Collections;
using System.Collections.Generic;

namespace BetFeed.ViewModels
{
    public class NewMatchesViewModel
    {
        public NewMatchesViewModel()
        {
            this.RequestDate = DateTime.Now;
        }

        public DateTime RequestDate { get; set; }

        public ICollection<MatchViewModel> Matches { get; set; }
    }
}