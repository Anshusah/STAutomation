﻿ 
using System.Collections.Generic;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class RadioGroup : Element
    {

        public RadioGroup(){
            this.Type = this.GetType().FullName;
            //base.Setting = "";
        }

        public override string Id { set; get; }
        public string Index { set; get; }
        public override string Title { get; set; }
        public string BackendId { get; set; } = "RadioGroup" + Utils.GenerateId();
        public string BackendClass { get; set; } = "custom-control-input radio-target";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public string FrontendId { get; set; } = "RadioGroup" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "custom-control-input radio-target";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public List<Validation> Validations { get; set; }
        public List<RadioOption> RadioOptions { get; set; }
        public List<TargetOption> TargetOptions { get; set; }
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public bool? IsOnChangeEvent { get; set; } = false;
        public bool? IsOnLoadEvent { get; set; } = false;
        public ElementEvent OnChangeEvent { get; set; }
        public ElementEvent OnLoadEvent { get; set; }
        public bool? EnablePopup { get; set; } = false;
        public bool? DisablePopupClose { get; set; } = false;
        public string PopUpTitle { get; set; }
        public string PopUpClass { get; set; }
        public List<PopUpTargetOption> PopUpTarget { get; set; }

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
        public class PopUpTargetOption
        {
            public string SelectId { get; set; }
            public string TargetId { get; set; }
            public string PopUpTitle { get; set; }
            public bool? EnablePopup { get; set; } = false;
        }
        public class Validation
        {
            public string FieldOperator { get; set; }
            public string ErrorMessage { get; set; }
            public List<string> ValidationValues { get; set; }
        }
        public class RadioOption
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public string Icon { get; set; }
            public bool Selected { get; set; } = false; 
        }
        public class TargetOption
        {
            public string SelectId { get; set; }
            public string TargetId { get; set; }
            public string ShowHide { get; set; } = "0";
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
