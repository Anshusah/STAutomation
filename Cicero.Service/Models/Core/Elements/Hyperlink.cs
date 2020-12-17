using Cicero.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.Core.Elements
{
    public class Hyperlink: Element
    {
        public Hyperlink()
        {

            this.Type = this.GetType().FullName;
        }

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Hyperlink" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string FrontendIcon { get; set; }
        public bool? FrontendIconVisibility { get; set; } = false;
        public string BackendId { get; set; } = "Hyperlink" + Utils.GenerateId();
        public string BackendClass { get; set; } = "";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public string BackendIcon { get; set; }
        public bool? BackendIconVisibility { get; set; } = false;
        public override string Template { get; set; } = "<a href='{0}' {1}>{2}</a>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public string FrontendHref { get; set; }
        public string FrontendTarget { get; set; }
        public string BackendHref { get; set; }
        public string BackendTarget { get; set; }
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
    }
}
