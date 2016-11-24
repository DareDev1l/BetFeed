using System;
using System.Collections;
using System.Collections.Generic;

namespace BetFeed.ViewModels
{
    public class NewBetsViewModel
    {
        public NewBetsViewModel()
        {
            this.RequestDate = DateTime.Now;
            this.Bets = new HashSet<BetViewModel>();
        }

        public ICollection<BetViewModel> Bets { get; set; }

        public DateTime RequestDate { get; set; }
    }
}