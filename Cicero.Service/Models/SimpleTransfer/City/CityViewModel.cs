using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.City
{
    public class CityViewModel
    {
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        [Display(Name = "City Code")]
        [MinLength(2), MaxLength(5)]
        public string CityCode { get; set; }

        [Required]
        [Display(Name = "City Name")]
        [MinLength(2), MaxLength(100)]
        public string CityName { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Display(Name = "State Id")]
        [MinLength(2), MaxLength(5)]

        public string StateId { get; set; }

        [Display(Name = "State Name")]
        [MinLength(2), MaxLength(100)]

        public string StateName { get; set; }

        public bool Status { get; set; }

        public string UpdatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedAt { get; set; }
    }
}
