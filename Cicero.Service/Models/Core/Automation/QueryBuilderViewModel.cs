using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.Core.Automation
{
    class QueryBuilderViewModel
    {

        public string id { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public string optgroup { get; set; }
        public string input { get; set; }
        //public int size { get; set; }
        public object validation { get; set; }
        //public string operators { get; set; }
        public object values { get; set; }
        public List<string> operators { get; set; }
    }
}
