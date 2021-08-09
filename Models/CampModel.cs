using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CoreCodeCamp.Data;

namespace CoreCodeCamp.Models
{
    public class CampModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        [Required]
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Range(1,100)]
        public int Length { get; set; } = 1;

        public LocationModel Location { get; set; }
        public ICollection<TalkModel> Talks { get; set; }
    }
}
