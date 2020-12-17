using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Cicero.Service.Helpers;

namespace Cicero.Service.Models.Core.Elements
{
    public class Table : Element
    {
        public Table()
        {

            this.Type = this.GetType().FullName;
        }

        public override string Id { set; get; }
        public override string Title { get; set; }
        public string FrontendId { get; set; } = "Table" + Utils.GenerateId();
        public string FrontendClass { get; set; } = "table";
        public string FrontendLabel { get; set; }
        public bool FrontendVisible { get; set; } = true;
        public string FrontendIcon { get; set; }
        public bool? FrontendIconVisibility { get; set; } = false;
        public string BackendId { get; set; } = "Table" + Utils.GenerateId();
        public string BackendClass { get; set; } = "table";
        public string BackendLabel { get; set; }
        public bool BackendVisible { get; set; } = true;
        public string BackendIcon { get; set; }
        public bool? BackendIconVisibility { get; set; } = false;
        public override string WrapperTemplate { get; set; } = "<div class=\"form-group {0}\">{1}</div>";
        public override string Template { get; set; } = "<table {0}>{1}</table>";
        public bool VisibleinGrid { get; set; } = false;
        public bool? VisibleinFooter { get; set; } = false;
        public List<TargetOption> TargetOptions { get; set; }
        public string TabId { get; set; }
        public bool? IsOnClickEvent { get; set; } = false;
        public bool? IsOnSaveFormEvent { get; set; } = false;
        public bool? IsOnSwitchTabEvent { get; set; } = false;
        public bool? IsOnResponseTarget { get; set; } = false;
        public List<TargetSettingOnResponse> TargetSettingsOnResponse { get; set; }
        public ElementEvent OnClickEvent { get; set; }
        public List<TableColumn> Column { get; set; }
        public List<TableHeader> Header { get; set; }
        public int TType { get; set; } = 0;
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

        public class TableColumn
        {
            public string Caption { get; set; }
            public string ElementClass { get; set; }
            public Element ColumnElement { get; set; }
            public bool? HasSubColumn { get; set; } = false;
            public ColumnSetting Setting { get; set; }
        }

        public class TableHeader
        {
            public string HeaderTitle { get; set; }
            public string HeaderId { get; set; }
            public string Colspan { get; set; }
            public string Attribute { get; set; }
            public string ParentId { get; set; } = "0";
           
        }
        public class ColumnSetting
        {
            public bool? IsVisible { get; set; } = true;
            public string FrontendClass { get; set; } 
            public string BackendClass { get; set; }
            public string Style { get; set; }
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
        public enum TableType
        {
            [Description("Input Table")]
            InputTable,
            [Description("Display Table")]
            DisplayTable,
            [Description("Display with Editable Table")]
            DisplayAndEdit
        }
    }
}
