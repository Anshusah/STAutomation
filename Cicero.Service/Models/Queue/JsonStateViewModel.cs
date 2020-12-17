using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class JsonStateViewModel
    {

        public First First { get; set; }

        public bool IsActive { get; set; } = true;

        public Last Last { get; set; }

        public Style Style { get; set; }

        public int CaseFormId { get; set; }
        public string ActionId { get; set; }

    }

    public class First
    {
        public int State { get; set; }

        public string StateXPos { get; set; }

        public string StateYPos { get; set; }

        public string LineXPos { get; set; }

        public string LineYPos { get; set; }

        public bool IsLock { get; set; } = true;

        public bool Aero { get; set; }

        public string Type { get; set; }
    }

    public class Last
    {
        public bool IsLock { get; set; } = true;

        public bool Aero { get; set; }

        public string StateXPos { get; set; }

        public string StateYPos { get; set; }

        public string LineXPos { get; set; }

        public string LineYPos { get; set; }

        public int State { get; set; }

        public string Type { get; set; }
    }

    public class Style
    {
        public string Color { get; set; }

        public string HoverColor { get; set; }
    }

}
