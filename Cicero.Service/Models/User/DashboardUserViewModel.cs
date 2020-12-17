using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class DashboardUserViewModel
    {

        public string Id { get; set; }


        public string DisplayName { get; set; }


        public string CreatedAt { get; set; }

    }
}
