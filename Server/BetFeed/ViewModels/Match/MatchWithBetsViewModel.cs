﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetFeed.ViewModels
{
    public class MatchWithBetsViewModel
    {
        public MatchWithBetsViewModel()
        {
            this.RequestDate = DateTime.Now;
            this.Bets = new HashSet<BetViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<BetViewModel> Bets { get; set; }

        public string First { get; set; }

        public string Second { get; set; }

        public DateTime RequestDate { get; set; }
    }
}