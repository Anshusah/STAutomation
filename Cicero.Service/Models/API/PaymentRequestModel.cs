using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class PaymentRequestModel
    {
        public string Token { get; set; }

        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PayeeName { get; set; }

        [Required]
        public string PayerName { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public int Reason { get; set; }

        [Required]
        public string RequestId { get; set; }

        [Required]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The field {0} must be greater than Zero.")]
        public decimal RequestAmount { get; set; }

        [Required]
        public string JazzCashAccountNumber { get; set; }

        public string PaymentReferenceNumber { get; set; }

        public int Status { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
