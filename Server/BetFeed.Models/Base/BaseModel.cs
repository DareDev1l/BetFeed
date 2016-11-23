using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFeed.Models.Base
{
    public class BaseModel : IBaseModel
    {
        public BaseModel()
        {
            UpdatedOn = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
