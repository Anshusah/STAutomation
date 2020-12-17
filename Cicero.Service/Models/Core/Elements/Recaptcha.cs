using Cicero.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.Core.Elements
{
    public class Recaptcha: Element
    {
        public Recaptcha()
        {

            this.Type = this.GetType().FullName;
        }

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Recaptcha" + Utils.GenerateId();
        public string FrontendClass { get; set; }
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string BackendId { get; set; } = "Recaptcha" + Utils.GenerateId();
        public string BackendClass { get; set; }
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public override string Template { get; set; } = "<div class=recaptcha-div><input type=hidden {2}/><div class=\"g-recaptcha  {0}\" {1} {3}></div></div>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public string SiteKey { get; set; }
        public string SecrertKey { get; set; }
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
