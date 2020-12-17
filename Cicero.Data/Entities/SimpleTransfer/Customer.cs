using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class Customer
    {   
        [Key]
        [StringLength(450)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public int TenantId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string MiddleName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CountryCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string MobileNumber { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Street { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string City { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string PostCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Gender { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime  DOB { get; set; }
        public int IdType { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string IdNumber { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime IdExpiryDate { get; set; }
        public bool Status { get; set; }

        public bool IsOnfidoVerify { get; set; }

        public string OnfidoChecksResult { get; set; }

        public DateTime? KycVerifiedDate { get; set; }

        public int KycFailedCount { get; set; }

        public bool KycManualPass { get; set; }

        public DateTime? KycManualPassDate { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; }    

        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
