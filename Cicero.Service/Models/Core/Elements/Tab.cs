 
using System.Collections.Generic;
using Cicero.Service.Helpers;
using static Cicero.Service.Enums;

namespace Cicero.Service.Models.Core.Elements
{

    public class Tab : Element
    {

        public Tab()
        {
            Row = new List<Row>();
        }
        public List<Row> Row { get; set; }

        public override string Id { set; get; }
        public override string Title { get; set; } 
        public string FrontendId { get; set; } = "Tab" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "tab-pane fade ";
        public string FrontendLabel { get; set; }
        public bool FrontendLabelVisible { get; set; } = true;
        public bool FrontendVisible { get; set; } = true;
        public bool FrontendIconVisible { get; set; } = false;
        public string FrontendIcon { get; set; }
        public string BackendId { get; set; } = "Tab" + Utils.GenerateId();
        public string BackendClass { get; set; } = "tab-pane fade ";
        public string BackendLabel { get; set; }
        public bool BackendLabelVisible { get; set; } = true;
        public bool BackendVisible { get; set; } = true;
        public bool BackendIconVisible { get; set; } = false;
        public string BackendIcon { get; set; }
        public override string Template { get; set; } = "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>";
        public bool VisibleinGrid { get; set; } = false;
        public int TabType { get; set; } = 0;
        public string GetName(string FieldName)
        {
            return "name=" + FieldName + " data-fb-name=" + FieldName + "";
        }
        public string GetChecked(bool? checkValue)
        {
            if (checkValue == true)
                return " checked=" + checkValue;
            return "";
        }
    }


}
