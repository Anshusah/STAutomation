using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class SupplierBank
    {   
        [Key]
        public int Id { get; set; }
        public int SupplierId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string BankName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string BankCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CountryCode { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
    public class SupplierBankBranch
    {
        [Key]
        public int Id { get; set; }
        public int SupplierId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string BranchName { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string BranchCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string BankCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CountryCode { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string CityCode { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
    public class SupplierCity
    {
        [Key]
        public int Id { get; set; }
        public int SupplierId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string CityCode { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string CityName { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string CountryCode { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string StateId { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string StateName { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string CreatedBy { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
