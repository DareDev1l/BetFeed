using BetFeed.Models.Base;

namespace BetFeed.Models
{
    public class Odd : BaseModel
    {
        public decimal Value { get; set; }

        public string SpecialBetValue { get; set; }
    }
}