using System;
using System.Collections.Generic;
using System.Text;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class Button : Element
    {
        public Button()
        {

            this.Type = this.GetType().FullName;
        }

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Button" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "btn btn-primary";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string FrontendIcon { get; set; }
        public bool? FrontendIconVisibility { get; set; } = false;
        public string BackendId { get; set; } = "Button" + Utils.GenerateId();
        public string BackendClass { get; set; } = "btn btn-primary";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public string BackendIcon { get; set; }
        public bool? BackendIconVisibility { get; set; } = false;
        public override string Template { get; set; } = "<button type='button' {0}>{1}</button>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public List<TargetOption> TargetOptions { get; set; }
        public string TabId { get; set; }
        public bool? IsOnClickEvent { get; set; } = false;
        public bool? IsOnSaveFormEvent { get; set; } = false;
        public bool? IsOnSwitchTabEvent { get; set; } = false;
        public bool? IsValidateOnTabSwitch { get; set; } = false;
        public bool? IsOnResponseTarget { get; set; } = false;
        public List<TargetSettingOnResponse> TargetSettingsOnResponse { get; set; } 
        public ElementEvent OnClickEvent { get; set; }
        public bool? EnablePopup { get; set; } = false;
        public bool? DisablePopupClose { get; set; } = false;
        public string PopUpTitle { get; set; }
        public string PopUpClass { get; set; }
        public List<string> PopUpTarget { get; set; }
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
        public class TargetSettingOnResponse
        {
            public string SelectValue { get; set; }
            public string TargetId { get; set; }
            public bool HasSubTarget { get; set; } = false;
            public string Option { get; set; }
            public List<SubTargetSetting> SubTarget { get; set; }
        }
        public class SubTargetSetting
        {
            public string Value { get; set; }
            public string Option { get; set; }
        }
    }
}
