using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cicero.Service.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public partial class DTResponseModel//<T>// where T :class


    {
        public int? draw { get; set; }
        public int? recordsTotal { get; set; }
        public int? recordsFiltered { get; set; }

        public string status { get; set; }
        public object data { get; set; }
    }
}
