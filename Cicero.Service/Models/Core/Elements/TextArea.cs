 
using System.Collections.Generic;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class TextArea : Element
    {


        public TextArea()
        {

            this.Type = this.GetType().FullName;
            //base.Setting = "";
        }

        public override string Id { set; get; }
        public string Index { set; get; }
        public override string Title { get; set; }
        public string BackendId { get; set; } = "TextArea" + Utils.GenerateId();
        public string BackendClass { get; set; } = "form-control";
        public string BackendLabel { get; set; }
        public bool? BackendVisible { get; set; } = true;
        public string FrontendId { get; set; } = "TextArea" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "form-control";
        public string FrontendLabel { get; set; }
        public bool? FrontendVisible { get; set; } = true;
        public override string Template { get; set; } = "<textarea class=\"{0}\" {2}/>{1}</textarea>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public List<Validation> Validations { get; set; }
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public bool? IsOnKeyUpEvent { get; set; } = false;
        public ElementEvent OnKeyUpEvent { get; set; }
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
        public bool? automationEnable { get; set; }
        public class Validation
        {
            public string FieldOperator { get; set; }
            public string ErrorMessage { get; set; }
            public List<string> ValidationValues { get; set; }
        }
        public class ElementEvent
        {
            public string StartState { get; set; }
            public string EndState { get; set; }
            public List<string> TakeValueFromName { get; set; }
            public List<string> TakeValueFromElement { get; set; }
            public List<string> TakeValueFromElementValidation { get; set; }
            public List<string> SetValueToName { get; set; }
            public List<string> SetValueToElement { get; set; }
            public List<string> SetValueToEventTrigger { get; set; }
        }

    }
}
