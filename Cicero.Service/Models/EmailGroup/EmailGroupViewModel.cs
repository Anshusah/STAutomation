using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class EmailGroupViewModel
    {
        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Allowed characters are A-Z, a-z, 0-9 and _")]
        [Required(ErrorMessage = "Please Enter Skill Title.")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Skill Title")]
        public string Title { get; set; }
        
        public DateTime CreatedAt { get; set; }

       
        public DateTime UpdatedAt { get; set; }

        
        public string CreatedBy { get; set; }

        public List<EmailsViewModel> Emails { get; set; }

        public int TenantId { get; set; }
    }

    public class EmailsViewModel
    { 
        public int Id { get; set; }

        public string Emailstring { get; set; }

        public int EmailGroupId { get; set; }

    }
}
