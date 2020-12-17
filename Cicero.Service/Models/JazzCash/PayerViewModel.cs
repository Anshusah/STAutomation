using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Cicero.Service.Enums;

namespace Cicero.Service.Models.JazzCash
{
    public class PayerViewModel
    {
        public int Id { get; set; }

        [Required]
        public PayeeType PayerType { get; set; }

        public string UserId { get; set; }

        public int TenantId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string TypeOfBusinessEntity { get; set; }

        public string CompanyWebsite { get; set; }

        [Required]
        public string Email { get; set; }

        public DateTime DOB { get; set; }

        [Required]
        public string CountryCode { get; set; }

        [Required]
        public string Address { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string MobileNumber { get; set; }

        public int IdType { get; set; }

        public string IdNumber { get; set; }

        public DateTime IdExpiryDate { get; set; }

        public string IssuingCountry { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public bool Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        public string CreatedDate { get; set; } 

        public string UpdatedDate { get; set; }

        public string JazzCashAccount { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

    }
}
