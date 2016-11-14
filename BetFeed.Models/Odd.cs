using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models
{
    public class Odd
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal SpecialBetValue { get; set; }
    }
}