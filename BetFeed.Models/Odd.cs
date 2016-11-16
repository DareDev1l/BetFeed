using BetFeed.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models
{
    public class Odd : BaseModel
    {
        public decimal Value { get; set; }

        public decimal SpecialBetValue { get; set; }

        //[ForeignKey("Bet_Id")]
        //public int BetId { get; set; }
    }
}