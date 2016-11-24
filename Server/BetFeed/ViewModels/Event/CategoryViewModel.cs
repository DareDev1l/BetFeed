using System.Collections.Generic;

namespace BetFeed.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            this.Events = new HashSet<EventViewModel>();
        }

        public string Name { get; set; }

        public ICollection<EventViewModel> Events { get; set; }
    }
}