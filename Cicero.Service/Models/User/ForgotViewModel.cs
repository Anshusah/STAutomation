using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class ForgotViewModel
    {

        [Display(Name = "Email Address")]
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Remote("CheckIfEmailExists", "User", ErrorMessage = "Email Address does not Exist.", HttpMethod = "Post", AdditionalFields = "Id")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
