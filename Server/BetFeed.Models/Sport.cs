using BetFeed.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models
{
    public class Sport : BaseModel
    {
        public Sport()
        {
            this.Events = new HashSet<Event>();
        }

        public virtual ICollection<Event> Events { get; set; }
    }
}
