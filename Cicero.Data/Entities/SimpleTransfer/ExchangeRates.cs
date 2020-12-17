using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class ExchangeRates
    {
        [Key]
        public int Id { get; set; }

        public DateTime? DateTime { get; set; }

        public string FromCountryCode { get; set; }

        public string ToCountryCode { get; set; }

        public string FromCurrencyCode { get; set; }

        public string ToCurrencyCode { get; set; }

        public string Bank { get; set; }
        public string BankCode { get; set; }

        public int ModeOfPayment { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? ExchangeRate { get; set; }

        public string Source { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
