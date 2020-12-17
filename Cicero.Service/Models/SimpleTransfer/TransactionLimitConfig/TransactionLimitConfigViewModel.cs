using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.TransactionLimitConfig
{
    public class TransactionLimitConfigViewModel: IValidatableObject
    {
        public int Id { get; set; }
        public int TenantId { get; set; }

        [Required(ErrorMessage = "Please select any country.")]
        public string CountryCode { get; set; }

        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Value must be 1 or greater than 1.")]
        public decimal LimitAmountPerTxn { get; set; }

        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Value must be 1 or greater than 1.")]
        public decimal LimitAmountPerDay { get; set; }

        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Value must be 1 or greater than 1.")]
        public decimal LimitAmountPerMonth { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int LimitNoPerDay { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than 0.")]
        public int LimitNoPerMonth { get; set; }

        public bool Status { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public string UpdatedDate { get; set; }

        public string CreatedDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LimitAmountPerTxn >= LimitAmountPerDay)
            {
                yield return new ValidationResult(
                    $"Limit Amount Per Day must be greater than Limit Amount Per Transaction",
                    new[] { "LimitAmountPerDay" });
            }

            if (LimitAmountPerDay >= LimitAmountPerMonth)
            {
                yield return new ValidationResult(
                    $"Limit Amount Per Month must be greater than Limit Amount Per Day",
                    new[] { "LimitAmountPerMonth" });
            }

            if (LimitNoPerDay >= LimitNoPerMonth)
            {
                yield return new ValidationResult(
                    $"Limit No Per Month must be greater than Limit No Per Day",
                    new[] { "LimitNoPerMonth" });
            }
        }
    }
}
