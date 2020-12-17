using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class ArticleViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is Required")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Short Description")]
        public string Excerpt { get; set; }

        [Display(Name = "Status")]
        public short Status { get; set; }

        [Display(Name = "Created At")]
        public string CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public string UpdatedAt { get; set; }

        [Display(Name = "Template")]
        public string Template { get; set; }

        [Display(Name = "Version")]
        public int Version { get; set; }

        [Display(Name = "Identifier")]
        public string Slug { get; set; }

        public string Type { get; set; }

        public int ParentId { get; set; }

        public int TenantId { get; set; }

        public IEnumerable<int> Images { set; get; }
    }
}
