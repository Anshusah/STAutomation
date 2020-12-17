using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cicero.Data.Entities
{
   public class SkillSet
    {
        [Key]
        public int Id { get; set; }
        
        [Column(TypeName ="nvarchar(100)")]
        public string Title { get; set; }

        [Column(TypeName ="datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName ="datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "varchar(250)")]
        public string CreatedBy { get; set; }

        public int CaseLimit { get; set; }

        public bool IsActive { get; set; }

        public int TenantId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenants { get; set; }

    }
}
