using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cicero.Data.Entities
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Url { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Title { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Description { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string CreatedBy { get; set; }

        public int? TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? Type { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public virtual Case Case { get; set; }

        public virtual ICollection<ArticleMedia> ArticleMedias { get; set; }
        public virtual ICollection<CaseMedia> CaseMedias { get; set; }
        public virtual ICollection<UserMedia> UserMedias { get; set; }
    }
}
