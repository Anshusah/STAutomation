using Cicero.Service.Helpers;
namespace Cicero.Service.Models.Core.Elements
{

    public class Paragraph : Element
    {

        public Paragraph()
        {

            this.Type = this.GetType().FullName;
            //base.Setting = "";
        }
        

        public override string Id { set; get; }
        public override string Title { get; set; } 
        public string ParagraphText { get; set; } = "";
        public string FrontendId { get; set; } = "Paragraph" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = false;
        public string BackendId { get; set; } = "Paragraph" + Utils.GenerateId();
        public string BackendClass { get; set; } = "";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = false;
        public override string Template { get; set; } = "<p class=\"{0}\" {2}>{1}</p>";
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
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
        public bool? automationEnable { get; set; }
    }


}
