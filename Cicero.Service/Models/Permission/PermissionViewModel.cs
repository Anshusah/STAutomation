using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class PermissionViewModel
    {
        

        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }


    }
}
