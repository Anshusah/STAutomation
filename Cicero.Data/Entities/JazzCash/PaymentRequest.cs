using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.JazzCash
{
    public class PaymentRequest
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string PayeeName { get; set; }

        public string PayerName { get; set; }

        public string Currency { get; set; }

        public int Reason { get; set; }

        public string RequestId { get; set; }

        public decimal RequestAmount { get; set; }

        public string JazzCashAccountNumber { get; set; }

        public string PaymentReferenceNumber { get; set; }

        public int Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
