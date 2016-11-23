using System;

namespace BetFeed.Models.Base
{
    public interface IBaseModel
    {
        int Id { get; set; }

        string Name { get; set; }

        DateTime UpdatedOn { get; set; }
    }
}
