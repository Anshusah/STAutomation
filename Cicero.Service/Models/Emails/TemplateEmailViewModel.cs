using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class TemplateEmailViewModel
    {

        //[Display(Name = "Template Name")]
        public string Title { get; set; }

        //[Display(Name = "Html Editor")]
        public string Content { get; set; }

    }
}
