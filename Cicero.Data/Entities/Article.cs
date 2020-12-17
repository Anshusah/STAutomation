using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Data.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Slug { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Template { get; set; }

        public short Status { get; set; }

        [Column(TypeName = "varchar(5000)")]
        public string Excerpt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedAt { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedAt { get; set; }

        public int Version { get; set; }

        public int ParentId { get; set; }

        public string UserId { get; set; }

        public string Type { get; set; }

        public int? TenantId { get; set; }

        public int? FormId { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Subject { get; set; }

        [Column(TypeName = "varchar(50)")]
        public int RecipientType { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string RecipientField { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string RecipientDatabaseTable { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string RoleId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string EmailGroupId { get; set; }

        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<ArticleMedia> ArticleMedias { get; set; }
    }
}
