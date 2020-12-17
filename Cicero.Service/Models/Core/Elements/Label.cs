using System;
using System.Collections.Generic;
using System.Text;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class Label: Element
    {
        public Label()
        {

            this.Type = this.GetType().FullName;
        }

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Label" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string BackendId { get; set; } = "Label" + Utils.GenerateId();
        public string BackendClass { get; set; } = "";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public override string Template { get; set; } = "<div class=\"{0}\" {3}><div class='label-text' {2}>{1}</div></div>";
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public override string SetValueFrom { get; set; }
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public bool? FrontendLabelVisibility { get; set; } = true;
        public bool? FrontendIconVisibility { get; set; } = false;
        public bool? FrontendImageVisibility { get; set; } = false;
        public string FrontendLabelStyle { get; set; }
        public string FrontendIcon { get; set; }
        public string FrontendImage { get; set; }
        public string FrontendImageHeight { get; set; }
        public string FrontendImageWidth { get; set; }
        public string FrontendLabelFontSize { get; set; }
        public string FrontendIconFontSize { get; set; }
        //  public string FrontendLabelAlignment { get; set; }
        public bool? BackendLabelVisibility { get; set; } = true;
        public bool? BackendIconVisibility { get; set; } = false;
        public bool? BackendImageVisibility { get; set; } = false;
        public string BackendLabelStyle { get; set; }
        public string BackendIcon { get; set; }
        public string BackendImage { get; set; }
        public string BackendImageHeight { get; set; }
        public string BackendImageWidth { get; set; }
        public string BackendLabelFontSize { get; set; }
        public string BackendIconFontSize { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        //public string BackendLabelAlignment { get; set; }
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

    public enum LabelStyles
    {
        Bold = 1,
        Italic = 2,
        Underline = 3,
    }
}
