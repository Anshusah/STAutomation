using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class RateSupplierFeeConfig
    {
        [Key]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public int TenantId { get; set; }

        public int SupplierId { get; set; }
        public int PayoutModeId { get; set; }
        public decimal UpperLimitAmount { get; set; }
        public decimal LowerLimitAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal FeePercentage { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public bool Status { get; set; }
    }
}
