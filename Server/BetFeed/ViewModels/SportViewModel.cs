using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetFeed.ViewModels
{
    public class SportViewModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public ICollection<EventViewModel> Events { get; set; }
    }
}