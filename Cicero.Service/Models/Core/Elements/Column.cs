 
using System.Collections.Generic;
using Cicero.Service.Helpers; 

namespace Cicero.Service.Models.Core.Elements
{
    public class Column : Element
    {

        public Column()
        {
            Element = new List<Element>();
        }
        public List<Element> Element { get; set; }


        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Column" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "col-lg-12";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string BackendId { get; set; } = "Column" + Utils.GenerateId();
        public string BackendClass { get; set; } = "col-lg-12";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public bool VisibleinGrid { get; set; } = false;
        public override string Template { get; set; } = "<div class=\"{0}\" id=\"{2}\" {3} >{1}</div>";
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
