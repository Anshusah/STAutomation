using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace Cicero.Data.Entities
{
    public class CaseCoreDataTable
    {
        [Key]
        public int Id { get; set; }     

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string SurName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string MaritialStatus { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Address1 { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Address2 { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PostCode { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string City { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Country { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string TelephoneNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string ContactDay { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        public string VatRegistered { get; set; }

        public int VatRate { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string PolicyNumber { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime PolicyStartDate { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime PolicyEndDate { get; set; }

        public string HasChildren { get; set; }

        public int NumberOfChildren { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string InsuranceType { get; set; }

        public float Excess { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string BankAccountName { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string BankSortCode { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string BankAccountNumber { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string GeoLocation { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string OtherInformation { get; set; }

        public string Extras { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CaseGeneratedId { get; set; }

        public int OrganisationId { get; set; }

        public int Version { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        public bool Status { get; set; }

        public string UserId { get; set; }

        public int StateId { get; set; }

        public int TenantId { get; set; }

        public int CaseFormId { get; set; }

        public int SignatureId { get; set; }
        
        public int Order { get; set; }

        public string UserAccessId { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime SignatureDate { get; set; }

        public string SchemeId { get; set; }

        public string JsonExtras { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("StateId")]
        public virtual State State { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("CaseFormId")]
        public virtual CaseForm CaseForm { get;}

        [ForeignKey("SignatureId")]
        public virtual Media Media { get; }

        public virtual ICollection<Message> Messages { get; set; }

    }
}
