using Cicero.Service.Helpers;
namespace Cicero.Service.Models.Core.Elements
{

    public class Heading : Element
    {

        public Heading()
        {

            this.Type = this.GetType().FullName;
            //base.Setting = "";
        }
        

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string HeaderType { get; set; } = "";
        public string HeaderText { get; set; } = "";
        public string FrontendId { get; set; } = "Heading" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string BackendId { get; set; } = "Heading" + Utils.GenerateId();
        public string BackendClass { get; set; } = "";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string Attribute { get; set; }
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
