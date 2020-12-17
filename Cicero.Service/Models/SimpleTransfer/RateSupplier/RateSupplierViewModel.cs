using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models.SimpleTransfer.RateSupplier
{
    public class RateSupplierViewModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }
        public string SystemId { get; set; }
        public int RatePriority { get; set; }

        [Display(Name = "Created At")]
        public string CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        public string UpdatedAt { get; set; } = DateTime.Now.ToString();

    }
}
