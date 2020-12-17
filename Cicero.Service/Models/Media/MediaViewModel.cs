using Cicero.Service.Models.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cicero.Service.Models
{
    public partial class MediaViewModel
    {
        public int Id { get; set; }

        public string Url { get; set; }

        [Required(ErrorMessage = "Please Provide a description")]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public int TenantId { get; set; }
        public int Type { get; set; }

        [Range(1, int.MaxValue)]
        public int ParentId { get; set; }
        [Display(Name = "Upload Image")]
        public IFormFile UploadImage { get; set; }

        public virtual ICollection<UserMediaViewModel> UserMedias { get; set; }
    }

    public partial class MediaByParentId
    {
        public string Parent { get; set; }
        public int ParentId { get; set; }
        public List<MediaViewModel> Media { get; set; }
    }
}
