using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cicero.Service.Models
{
    public class AddArticleViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Excerpt { get; set; }

        public short Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int ParentId { get; set; }

        [Display(Name = "Template")]
        public string Template { get; set; }

        [Display(Name = "Version")]
        public int Version { get; set; }

        [Display(Name = "Identifier")]
        public string Slug { get; set; }

        public int TenantId { get; set; }

    }
}