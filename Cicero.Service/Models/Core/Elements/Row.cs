
using System.Collections.Generic;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Row : Element
    {

        public Row()
        {
            Column = new List<Column>();
        }
        public List<Column> Column { get; set; }

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Row" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string BackendId { get; set; } = "Row" + Utils.GenerateId();
        public string BackendClass { get; set; } = "";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public bool VisibleinGrid { get; set; } = false;
        public override string Template { get; set; } = "<div class=\"form-row {0}\" id=\"{2}\" {3} >{1}</div>";
        public bool SetAsRepeatItem { get; set; } = false;
        public bool ShowInAccordion { get; set; } = false;

        public string RepeatItemTitle { get; set; } = "";
        public List<string> AppendElement { get; set; }
        public string GetName(string FieldName)
        {
            return " name=" + FieldName + " data-fb-name=" + FieldName + "";
        }
        public string GetChecked(bool? checkValue)
        {
            if (checkValue == true)
                return " checked=" + checkValue;
            return "";
        }
    }

}
