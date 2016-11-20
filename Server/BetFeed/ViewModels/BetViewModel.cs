using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetFeed.ViewModels
{
    public class BetViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }
    }
}