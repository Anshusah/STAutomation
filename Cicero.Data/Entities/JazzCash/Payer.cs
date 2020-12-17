using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.JazzCash
{
    public class Payer
    {
        [Key]
        public int Id { get; set; }

        public int PayerType { get; set; }

        public string UserId { get; set; }

        public int TenantId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string TypeOfBusinessEntity { get; set; }

        public string CompanyWebsite { get; set; }

        public string Email { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DOB { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CountryCode { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Address2 { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string City { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PostCode { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string MobileNumber { get; set; }

        public int IdType { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string IdNumber { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime IdExpiryDate { get; set; }

        public string IssuingCountry { get; set; }

        public string JazzCashAccount { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        public bool Status { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedDate { get; set; }
    }
}
