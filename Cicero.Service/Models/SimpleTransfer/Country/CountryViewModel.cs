using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Country
{
    public class CountryViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencyName { get; set; }

        public string FlagCode { get; set; }

        public string FlagImageUrl { get; set; }

        public int DisplayOrder { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public bool Status { get; set; }
    }
}
