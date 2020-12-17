using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models
{

    public class DynamicFormViewModel
    {
        public string formName { get; set; }

        public List<Tab> Tabs { get; set; }
    }

    public class Tab
    {
        public string tabName { get; set; }

        public List<Element> element { get; set; }
    }

    public class Element
    {
        public string type { get; set; }

        public bool required { get; set; }

        public string label { get; set; }

        public string description { get; set; }

        public string placeholder { get; set; }

        public string className { get; set; }

        public string name { get; set; }

        public string elementName { get; set; }

        public bool access { get; set; }

        public string value { get; set; }

        public string subtype { get; set; }

        public string maxlength { get; set; }

        public string wrapper_class { get; set; }

        public string role { get; set; }

        public bool? toggle { get; set; }

        public bool? inline { get; set; }

        public bool? other { get; set; }

        public bool? multiple { get; set; }

        public bool? show_in_back { get; set; }

        public bool? show_in_front { get; set; }

        public string data { get; set; }

        public string min { get; set; }

        public string max { get; set; }

        public string step { get; set; }

        public string rows { get; set; }

        public bool showHide { get; set; }

        public List<string> optionSelected { get; set; }

        public List<Value> values { get; set; }

        public List<Targetformdata> targetformdata { get; set; }

        public List<TableData> tableData { get; set; }

        public List<string> optionData { get; set; }

    }

    public class TableData
    {
        public string colTitle { get; set; }

        public string colType { get; set; }

        public List<string> values { get; set; }
    }

    public class Value
    {
        public string label { get; set; }

        public string value { get; set; }

        public bool selected { get; set; }

        public List<string> mapElement { get; set; }

        public bool showHide { get; set; }
    }

    public class Targetformdata
    {
        public string name { get; set; }

        public bool selected { get; set; }

        public List<Children> childrens { get; set; }
    }

    public class Children
    {
        public string list { get; set; }

        public bool selected { get; set; }
    }

}
