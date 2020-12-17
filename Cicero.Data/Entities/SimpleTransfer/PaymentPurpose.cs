using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class PaymentPurpose
    {   
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string PurposeName { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }
        public int TransfastId { get; set; }


    }
    public class BeneficiaryRelationship
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string RelationshipName { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }

    }
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public string Code { get; set; }

        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }
    }
    public class MaritalStatus
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string MaritalStatusName { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }

    }
    public class IdentificationType
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string IdentificationTypeName { get; set; }
        public string Code { get; set; }

        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }

    }
    public class SourceOfFund
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string SourceOfFundName { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }

        public int TransfastId { get; set; }

    }
    public class TransfastSourceOfFund
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int TenantId { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }

    }
    public class TransfastRemittancePurpose
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int TenantId { get; set; }
        public string CountryCode { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }

    }
}
