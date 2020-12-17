using System.Collections.Generic;
using System.ComponentModel;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class Number : Element
    {


        public Number()
        {

            this.Type = this.GetType().FullName;
            //base.Setting = "";
        }
    
        public override string Id { set; get; } 
        public string Index { set; get; }
        public override string Title { get; set; }
        public string BackendId { get; set; } = "Number" + Utils.GenerateId();
        public string BackendClass { get; set; } = "form-control";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public string FrontendId { get; set; } = "Number" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "form-control";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public bool IsCurrency { get; set; } = false;
        public string CurrencyType { get; set; } = "£";
        public string DecimalDigit { get; set; } = "2";
        public string FrontendFormatter { get; set; }
        public string BackendFormatter { get; set; }
        public override string Template { get; set; } = "<input type=\"number\" {0}/>";
        public string CurrencyTemplate { get; set; } = "<div class=\"currencyType {1}\"> {0} </div>";
        public string RangeTemplate { get; set; } = "<input type=\"range\" class=\"custom-range {1}\" {0}>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; } = "number";
        public string FieldSize { get; set; }
        public string RangeMin { get; set; } = "0";
        public string RangeMax { get; set; } = "5";
        public string RangeStep { get; set; } = "0.5";
        public int NumberType { get; set; } = 0;

        public string DefaultValue { get; set; }
        public List<Validation> Validations { get; set; }
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public List<TargetOption> TargetOptions { get; set; }
        public bool? IsOnKeyUpEvent { get; set; } = false;
        public ElementEvent OnKeyUpEvent { get; set; }


        //public string TargetName
        //{
        //    get { return _targetName; }
        //    set { _targetName = (value == null ? "" : value).Trim().Replace(" ", "_"); }
        //}
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

        public class TargetOption
        {
            public string TargetId { get; set; }
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

        public enum NumType
        {
            [Description("Number")]
            Number,
            [Description("Range")]
            Range
        }
    }
}
