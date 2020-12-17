using System.Collections.Generic;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class Media : Element
    {


        public Media()
        {

            this.Type = this.GetType().FullName;
            //base.Setting = "";
        }

        public override string Id { set; get; }
        public string Index { set; get; }
        public override string Title { get; set; }
        public string BackendId { get; set; } = "Media" + Utils.GenerateId();
        public string BackendClass { get; set; } = "media-list";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public string FrontendId { get; set; } = "Media" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "media-list";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public override string Template { get; set; } = "<ul class=\"{0}\" id=\"{2}\" name=\"{3}\">{1}</ul>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; } = "media";
        public string FieldSize { get; set; }
        public List<Validation> Validations { get; set; }
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public List<Image> Images { get; set; }
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
        public class Validation
        {
            public string FieldOperator { get; set; }
            public string ErrorMessage { get; set; }
            public List<string> ValidationValues { get; set; }
        }
        public class Image
        {
            public string Id { get; set; }
            public string Url { get; set; }
        }

    }
}
