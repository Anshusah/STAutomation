using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.Core
{
    public class SelectListItemWithIcon: SelectListItem
    {
        public string Icon { get; set; }
    }
}
