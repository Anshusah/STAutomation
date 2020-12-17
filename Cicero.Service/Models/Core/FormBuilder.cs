using System;
using System.Collections.Generic;
using System.Linq;

using Cicero.Service.Helpers;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using Cicero.Service.Services;
using Cicero.Service.Models.Core.Elements;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using Cicero.Data;
using Microsoft.EntityFrameworkCore;
using static Cicero.Service.Enums;


namespace Cicero.Service.Models.Core
{


    public class FormBuilder
    {
        [JsonIgnore]
        public ICommonService commonService;
        public ICaseService caseService;

        [JsonIgnore]
        public IMediaService mediaService;

        [JsonIgnore]
        public ApplicationDbContext _db;
        // Required Service Instance
        [JsonIgnore]
        protected AppSetting appSetting;

        [JsonIgnore]
        protected IUserService userService;

        [JsonIgnore]
        public HttpContext HttpContext { get; set; }

        // Required base field
        [JsonIgnore]
        public virtual FormBuilderViewModel FormData { get; set; }
        public virtual string Side { get; set; } = "frontend";
        public virtual string Title { get; set; }
        public virtual string Id { get; set; }
        public virtual string ElementId { get; set; }
        public virtual string Type { get; set; }
        public virtual string Collection { get; set; }

        public string GenerateValidationData(dynamic ElementBody)
        {
            string temp = string.Empty;
            string op = string.Empty;
            string opVal = string.Empty;
            string opMsg = string.Empty;
            if (ElementBody.GetType().GetProperty("Validations") != null && ElementBody.Validations != null && ElementBody.Validations.Count > 0)
            {

                foreach (var valid in ElementBody.Validations)
                {
                    var last = string.Empty;
                    var first = string.Empty;
                    var error = valid?.ErrorMessage;

                    if (valid?.ValidationValues?.Count > 0)
                    {
                        first = valid?.ValidationValues[0];
                    }
                    if (valid?.ValidationValues?.Count > 1)
                    {
                        last = valid?.ValidationValues[1];
                    }
                    string elementType = ElementBody.GetType().Name.ToLower();
                    var url = "/admin/validate.html?type=" + elementType;
                    switch (valid.FieldOperator)
                    {
                        case "required":

                            op = op + "required,,";
                            opVal = opVal + first + ",,";
                            opMsg = opMsg + error + ",,";
                            break;
                        case ">":
                            if ((first != null && first != string.Empty && first != ""))
                            {
                                op = op + ">,,";
                                opVal = opVal + first + ",,";
                                opMsg = opMsg + error + ",,";

                            }
                            break;
                        case "<":
                            if ((first != null && first != string.Empty && first != ""))
                            {
                                op = op + "<,,";
                                opVal = opVal + first + ",,";
                                opMsg = opMsg + error + ",,";

                            }
                            break;
                        case "=":
                            if ((first != null && first != string.Empty && first != ""))
                            {


                                op = op + "=,,";
                                opVal = opVal + first + ",,";
                                opMsg = opMsg + error + ",,";
                            }
                            break;
                        case "between":
                            if ((first != null && first != string.Empty && first != "") && (last != null && last != string.Empty && last != ""))
                            {


                                op = op + "between,,";
                                opVal = opVal + first + "-" + last + ",,";
                                opMsg = opMsg + error + ",,";

                            }

                            break;
                        case "email":
                            if (elementType != "number")
                            {
                                op = op + "email,,";
                                opVal = opVal + "true,,";
                                opMsg = opMsg + error + ",,";
                            }

                            break;
                        case "regex":
                            if ((first != null && first != string.Empty && first != ""))
                            {
                                op = op + "regex,,";
                                opVal = opVal + first + ",,";
                                opMsg = opMsg + error + ",,";

                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            temp += " data-op='" + op + "' data-op-val='" + opVal + "' data-op-msg='" + opMsg + "' ";
            return temp;
        }
        public string Render(int? caseId = null, int? stateId = null, int? formid = null, string sides = "backend")
        {
            try
            {
                if (this.commonService == null) this.commonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                if (this.caseService == null) this.caseService = this.HttpContext.RequestServices.GetService(typeof(ICaseService)) as ICaseService;
                string tab = string.Empty;

                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);

                string caseContent = "";
                List<CaseStateHistoryViewModel> caseStateHistoryItems = new List<CaseStateHistoryViewModel>();
                foreach (Elements.Tab TabBody in this.FormData.Tab)
                {
                    bool setAll = false;
                    bool temp = (this.Side == "frontend") ? TabBody.FrontendVisible : TabBody.BackendVisible;
                    bool tempIcon = (this.Side == "frontend") ? TabBody.FrontendIconVisible : TabBody.BackendIconVisible;

                    string tabpriviledge = "data";
                    string tabpriviledgeAlt = "data";
                    // if (!EleVisible) { priviledge = "hidden"; priviledgeAlt = "hidden"; }

                    var checkPermission = new Element.Permission();
                    if (TabBody.Permissions != null)
                    {
                        checkPermission = TabBody.Permissions.Where(x => x.RoleId == roleid).FirstOrDefault();
                    }
                    if (checkPermission != null && checkPermission.Read == true && temp == true && roleid != " ")
                    {
                        //    if (roleid != " ")
                        //{
                        if (TabBody.Permissions != null && TabBody.Permissions.Count > 0)
                        {

                            foreach (var permission in TabBody.Permissions)
                            {
                                if (permission.RoleId == roleid)
                                {
                                    if (permission.Read == true && permission.Write == false)
                                    {
                                        tabpriviledge = "disabled";
                                        tabpriviledgeAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                        setAll = true;
                                    }
                                    else if (permission.Read == false && permission.Write == false)
                                    {
                                        tabpriviledge = "hidden";
                                        tabpriviledgeAlt = "hidden";
                                        temp = false;
                                    }
                                    else
                                    {
                                        tabpriviledge = "";
                                        tabpriviledgeAlt = "";
                                    }
                                }

                            }
                            if (tabpriviledge == "data")
                            {
                                tabpriviledge = "hidden";
                                tabpriviledgeAlt = "hidden";
                                temp = false;
                            }
                        }
                    }
                    else
                    {
                        tabpriviledge = "";
                        tabpriviledgeAlt = "";
                    }

                    if (temp == true)
                    {

                        string tabid = (this.Side == "frontend") ? TabBody.FrontendId : TabBody.BackendId;

                        var tabLabelVisible = (this.Side == "frontend") ? TabBody.FrontendLabelVisible : TabBody.BackendLabelVisible;
                        string tablabel = (this.Side == "frontend") ? TabBody.FrontendLabel : TabBody.BackendLabel;
                        if (!tabLabelVisible)
                        {
                            tablabel = string.Empty;
                        }

                        var tabIconVisible = (this.Side == "frontend") ? TabBody.FrontendIconVisible : TabBody.BackendIconVisible;
                        string tabicon = (this.Side == "frontend") ? TabBody.FrontendIcon : TabBody.BackendIcon;
                        if (!tabIconVisible)
                        {
                            tabicon = string.Empty;
                        }

                        string tabiconhtml = string.Empty;
                        if (tempIcon && tabicon != null && tabicon != "")
                        {
                            tabiconhtml = "<span class='m-2'><i class='" + tabicon + "'></i></span>";
                        }
                        //static li and a class\
                        if (this.Side == "frontend")
                        {


                            if (this.FormData.Tab.IndexOf(TabBody) == 0)
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link active position-relative' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' ><span class='nav-count'><i>1</i></span>" + tabiconhtml + tablabel + "</a></li>";

                            }
                            else
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > <span class='nav-count'><i>" + (Convert.ToInt32(this.FormData.Tab.IndexOf(TabBody)) + 1).ToString() + "</i></span>" + tabiconhtml + tablabel + "</a></li>";

                            }
                        }
                        else
                        {
                            if (this.FormData.Tab.IndexOf(TabBody) == 0)
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link active' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > " + tabiconhtml + tablabel + "</a></li>";

                            }
                            else
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > " + tabiconhtml + tablabel + "</a></li>";

                            }
                        }

                        string row = string.Empty;
                        //string strtab = (TabBody.Template == null ? "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>" : TabBody.Template);
                        string strtab = "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>";
                        if (TabBody.Row != null && TabBody.Row.Count() > 0)
                        {
                            foreach (Row RowBody in TabBody.Row)
                            {
                                bool rowVisible = (this.Side == "frontend") ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                if (rowVisible)
                                {
                                    string strrow = RowBody.Template;
                                    string column = string.Empty;
                                    //this.Collection= this.Collection+TabBody.
                                    if (RowBody.Column != null && RowBody.Column.Count() > 0)
                                    {
                                        foreach (Elements.Column ColumnBody in RowBody.Column)
                                        {
                                            bool ColVisible = (this.Side == "frontend") ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                            if (ColVisible)
                                            {
                                                string element = "";
                                                string strcolumn = ColumnBody.Template;
                                                //this.Collection= this.Collection+TabBody.
                                                if (ColumnBody.Element != null && ColumnBody.Element.Count() > 0)
                                                {
                                                    foreach (dynamic ElementBody in ColumnBody.Element)
                                                    {

                                                        if (ElementBody != null)
                                                        {
                                                            bool EleVisible = (this.Side == "frontend") ? ElementBody.FrontendVisible : ElementBody.BackendVisible;
                                                            if (EleVisible)
                                                            {
                                                                string priviledge = "data";
                                                                string priviledgeAlt = "data";

                                                                //replace with another logic for view
                                                                if (setAll == true)
                                                                {
                                                                    priviledge = "disabled";
                                                                    priviledgeAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                }
                                                                else
                                                                {
                                                                    if (roleid != " ")
                                                                    {
                                                                        if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                                        {

                                                                            foreach (var permission in ElementBody.Permissions)
                                                                            {
                                                                                if (permission.RoleId == roleid)
                                                                                {
                                                                                    if (permission.Read == true && permission.Write == false)
                                                                                    {
                                                                                        priviledge = "disabled";
                                                                                        priviledgeAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                                    }
                                                                                    else if (permission.Read == false && permission.Write == false)
                                                                                    {
                                                                                        priviledge = "hidden";
                                                                                        priviledgeAlt = "hidden";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        priviledge = "";
                                                                                        priviledgeAlt = "";
                                                                                    }
                                                                                }

                                                                            }
                                                                            if (priviledge == "data")
                                                                            {
                                                                                priviledge = "hidden";
                                                                                priviledgeAlt = "hidden";
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        priviledge = "";
                                                                        priviledgeAlt = "";
                                                                    }
                                                                }
                                                                // if (!EleVisible) { priviledge = "hidden"; priviledgeAlt = "hidden"; }

                                                                //till here

                                                                string elementstr = string.Empty;

                                                                var label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "' " + priviledge + ">" + label + "</label>";
                                                                if (priviledge != "hidden")
                                                                {
                                                                    elementstr = elementstr + ElementBody.Template + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                }
                                                                else
                                                                {
                                                                    elementstr = elementstr + ElementBody.Template;
                                                                }
                                                                string attr = string.Empty;



                                                                if (this.Side == "frontend")
                                                                {
                                                                    attr = this.GenerateValidationData(ElementBody) + priviledge + " name=\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.FrontendClass + "\"";
                                                                    attr = attr + " data-elm-type='" + ElementBody.GetType().Name.ToLower() + "'";
                                                                }
                                                                else
                                                                {
                                                                    attr = this.GenerateValidationData(ElementBody) + priviledge + " name =\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.BackendClass + "\"";
                                                                    attr = attr + " data-elm-type='" + ElementBody.GetType().Name.ToLower() + "'";
                                                                }

                                                                string elms = "";
                                                                attr = attr + " data-name=\"" + ElementBody.Name + "\"";

                                                                var setValueFrom = ElementBody.SetValueFrom;
                                                                var sfvAttr = string.Empty;
                                                                if (setValueFrom != null)
                                                                {
                                                                    sfvAttr = " data-setvaluefrom='" + setValueFrom + "'";
                                                                }
                                                                switch (ElementBody.GetType().Name.ToLower())
                                                                {
                                                                    #region label
                                                                    case "label":
                                                                        elementstr = ElementBody.Template;
                                                                        string labelStyle = ((this.Side == "frontend") ? ElementBody.FrontendLabelStyle : ElementBody.BackendLabelStyle);

                                                                        string labelFontSize = ((this.Side == "frontend") ? ElementBody.FrontendLabelFontSize : ElementBody.BackendLabelFontSize);
                                                                        string iconFontSize = ((this.Side == "frontend") ? ElementBody.FrontendIconFontSize : ElementBody.BackendIconFontSize);

                                                                        string labelFontHtml = string.Empty;
                                                                        string iconFontHtml = string.Empty;

                                                                        attr = attr + sfvAttr;

                                                                        if (labelFontSize != null)
                                                                        {
                                                                            labelFontHtml = "style=font-size:" + labelFontSize + "px;";
                                                                        }

                                                                        if (iconFontSize != null)
                                                                        {
                                                                            iconFontHtml = "style=font-size:" + iconFontSize + "px;";
                                                                        }

                                                                        if (labelStyle != null)
                                                                        {
                                                                            switch (labelStyle)
                                                                            {
                                                                                case "1":
                                                                                    ElementBody.FrontendLabel = "<b>" + ElementBody.FrontendLabel + "</b>";
                                                                                    ElementBody.BackendLabel = "<b>" + ElementBody.BackendLabel + "</b>";
                                                                                    break;

                                                                                case "2":
                                                                                    ElementBody.FrontendLabel = "<i>" + ElementBody.FrontendLabel + "</i>";
                                                                                    ElementBody.BackendLabel = "<i>" + ElementBody.BackendLabel + "</i>";
                                                                                    break;

                                                                                case "3":
                                                                                    ElementBody.FrontendLabel = "<u>" + ElementBody.FrontendLabel + "</u>";
                                                                                    ElementBody.BackendLabel = "<u>" + ElementBody.BackendLabel + "</u>";
                                                                                    break;

                                                                                case "1,2":
                                                                                    ElementBody.FrontendLabel = "<b><i>" + ElementBody.FrontendLabel + "</i></b>";
                                                                                    ElementBody.BackendLabel = "<b><i>" + ElementBody.BackendLabel + "</i></b>";
                                                                                    break;

                                                                                case "1,3":
                                                                                    ElementBody.FrontendLabel = "<b><u>" + ElementBody.FrontendLabel + "</u></b>";
                                                                                    ElementBody.BackendLabel = "<b><u>" + ElementBody.BackendLabel + "</u></b>";
                                                                                    break;

                                                                                case "1,2,3":
                                                                                    ElementBody.FrontendLabel = "<b><i><u>" + ElementBody.FrontendLabel + "</b></i></u>";
                                                                                    ElementBody.BackendLabel = "<b><i><u>" + ElementBody.BackendLabel + "</b></i></u>";
                                                                                    break;

                                                                                case "2,3":
                                                                                    ElementBody.FrontendLabel = "<i><u>" + ElementBody.FrontendLabel + "</u></i>";
                                                                                    ElementBody.BackendLabel = "<i><u>" + ElementBody.BackendLabel + "</u></i>";
                                                                                    break;
                                                                            }
                                                                        }

                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel), labelFontHtml, attr);

                                                                        bool imageVisibility = ((this.Side == "frontend") ? ElementBody.FrontendImageVisibility : ElementBody.BackendImageVisibility);
                                                                        bool iconVisibility = ((this.Side == "frontend") ? ElementBody.FrontendIconVisibility : ElementBody.BackendIconVisibility);
                                                                        bool labelVisibility = ((this.Side == "frontend") ? ElementBody.FrontendLabelVisibility : ElementBody.BackendLabelVisibility);
                                                                        if (labelVisibility)
                                                                        {
                                                                            if (iconVisibility)
                                                                            {
                                                                                elementstr = "<div class=\"{0}\" {5}><div class='label-icon' {4}><i class=\"{2}\"></i></div><div class='label-text' {3}>{1}</div></div>";
                                                                                elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass + " has-icon" : ElementBody.BackendClass + " has-icon"), ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel), ((this.Side == "frontend") ? ElementBody.FrontendIcon : ElementBody.BackendIcon), labelFontHtml, iconFontHtml, attr);
                                                                            }

                                                                            if (imageVisibility)
                                                                            {
                                                                                string imageHeight = ((this.Side == "frontend") ? ElementBody.FrontendImageHeight : ElementBody.BackendImageHeight);
                                                                                string imageWidth = ((this.Side == "frontend") ? ElementBody.FrontendImageWidth : ElementBody.BackendImageWidth);

                                                                                var imgAttr = "height= '" + imageHeight + "' width='" + imageWidth + "'";
                                                                                elementstr = "<div class=\"{0}\" {5}><div class='label-image' {4}><img src=\"/images/{2}\" " + imgAttr + "/></div><div class='label-text' {3}>{1}</div></div>";
                                                                                elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass + " has-image" : ElementBody.BackendClass + " has-image"), ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel), ((this.Side == "frontend") ? ElementBody.FrontendImage : ElementBody.BackendImage), labelFontHtml, iconFontHtml, attr);
                                                                            }
                                                                        }
                                                                        else if (iconVisibility)
                                                                        {
                                                                            elementstr = "<div class=\"{0}\" {3}><div class='label-icon' {2}><i class=\"{1}\"></i></div></div>";
                                                                            elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass + " has-icon" : ElementBody.BackendClass + " has-icon"), ((this.Side == "frontend") ? ElementBody.FrontendIcon : ElementBody.BackendIcon), iconFontHtml, attr);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (imageVisibility)
                                                                            {
                                                                                string imageHeight = ((this.Side == "frontend") ? ElementBody.FrontendImageHeight : ElementBody.BackendImageHeight);
                                                                                string imageWidth = ((this.Side == "frontend") ? ElementBody.FrontendImageWidth : ElementBody.BackendImageWidth);

                                                                                var imgAttr = "height= '" + imageHeight + "' width='" + imageWidth + "'";
                                                                                elementstr = "<div class=\"{0}\" {3}><div class='label-image' {2}><img src=\"/images/{1}\" " + imgAttr + "/></div></div>";
                                                                                elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass + " has-image" : ElementBody.BackendClass + " has-image"), ((this.Side == "frontend") ? ElementBody.FrontendImage : ElementBody.BackendImage), iconFontHtml, attr);
                                                                            }
                                                                        }

                                                                        break;

                                                                    #endregion
                                                                    #region Button
                                                                    case "button":
                                                                        bool buttonIconVisibility = ((this.Side == "frontend") ? ElementBody.FrontendIconVisibility : ElementBody.BackendIconVisibility);
                                                                        string dataAttrClickEvent = string.Empty;

                                                                        if (ElementBody.IsOnClickEvent)
                                                                        {
                                                                            dataAttrClickEvent = "data-work-onclick='true' data-action-onclick='onClick'";
                                                                            if (ElementBody.OnClickEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnClickEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnClickEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnClickEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnClickEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrClickEvent = dataAttrClickEvent + " data-val-onclick-parname='" + t + "' data-val-onclick-parelm='" + t1 + "' data-val-onclick-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnClickEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnClickEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnClickEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnClickEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnClickEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnClickEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrClickEvent = dataAttrClickEvent + " data-val-onclick-resname='" + r + "' data-val-onclick-reselm='" + r1 + "' data-val-onclick-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrClickEvent = dataAttrClickEvent + " data-work-onclick-start='" + ElementBody.OnClickEvent.StartState + "' data-work-onclick-end='" + ElementBody.OnClickEvent.EndState + "' data-work-onclick-formid='" + formid + "'";
                                                                        }

                                                                        var saveFormEvent = ElementBody.IsOnSaveFormEvent ? " data-saveform=true " : "data-saveform=false ";
                                                                        bool isOnSwitchTabEvent = ElementBody.IsOnSwitchTabEvent;
                                                                        bool isValidateOnTabSwitch = (ElementBody.IsValidateOnTabSwitch != null ? ElementBody.IsValidateOnTabSwitch : false);
                                                                        var switchTab = string.Empty;
                                                                        if (isOnSwitchTabEvent)
                                                                        {
                                                                            switchTab = " data-switchtab='" + ElementBody.TabId + "' ";

                                                                        }
                                                                        if (isValidateOnTabSwitch)
                                                                        {
                                                                            switchTab += " data-switchtab-validate='true'";
                                                                        }
                                                                        else
                                                                        {
                                                                            switchTab += " data-switchtab-validate='false'";
                                                                        }
                                                                        dataAttrClickEvent = dataAttrClickEvent + saveFormEvent + switchTab;

                                                                        var buttonIconHtml = string.Empty;
                                                                        if (buttonIconVisibility)
                                                                        {
                                                                            buttonIconHtml = "<i class='" + ((this.Side == "frontend") ? ElementBody.FrontendIcon : ElementBody.BackendIcon) + "'></i>";
                                                                        }

                                                                        string buttontarget = "";

                                                                        List<Button.TargetOption> buttontargets = (List<Button.TargetOption>)ElementBody.TargetOptions;
                                                                        if (buttontargets != null)
                                                                        {
                                                                            var result = buttontargets;

                                                                            string toption = "";
                                                                            string taction = "";

                                                                            foreach (var targetoption in result)
                                                                            {
                                                                                if (result[0] == targetoption)
                                                                                {
                                                                                    toption = "#" + targetoption.TargetId;

                                                                                    taction = targetoption.ShowHide;
                                                                                }
                                                                                else
                                                                                {
                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                }

                                                                            }

                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                            {
                                                                                buttontarget = "data-target='" + toption + "' data-target-action='" + taction + "' data-action-click=onClick";
                                                                            }
                                                                        }
                                                                        var responseTarget = "";
                                                                        var responseTargetValue = string.Empty;
                                                                        if (ElementBody.IsOnResponseTarget)
                                                                        {
                                                                            responseTarget = "data-isresponse-target='true'";
                                                                            responseTargetValue = JsonConvert.SerializeObject(ElementBody.TargetSettingsOnResponse);
                                                                        }
                                                                        else
                                                                        {
                                                                            responseTarget = "data-isresponse-target='false'";
                                                                        }
                                                                        string popup = string.Empty;
                                                                        if (ElementBody.EnablePopup)
                                                                        {
                                                                            string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                            popup = popup + " data-popup-class='" + popupClass + "'";

                                                                            if (ElementBody.DisablePopupClose)
                                                                            {
                                                                                popup = popup + " data-popup-close-disable='true'";
                                                                            }
                                                                            popup = popup + " data-popup-enable='true'";
                                                                            if (ElementBody.PopUpTarget != null && ElementBody.PopUpTarget.Count > 0)
                                                                            {
                                                                                string t = string.Empty;

                                                                                for (int i = 0; i < ElementBody.PopUpTarget.Count; i++)
                                                                                {
                                                                                    if (ElementBody.PopUpTarget[i] != "" && ElementBody.PopUpTarget[i] != null)
                                                                                    {
                                                                                        t = t + "," + ElementBody.PopUpTarget[i];
                                                                                    }

                                                                                }
                                                                                if (t != string.Empty)
                                                                                {
                                                                                    t = t.Substring(1);
                                                                                    popup = popup + " data-popup-target='" + t + "'";
                                                                                }

                                                                            }
                                                                            popup = popup + " data-popup-title='" + ElementBody.PopUpTitle + "'";
                                                                        }
                                                                        else
                                                                        {
                                                                            popup = " data-popup-enable='false'";
                                                                        }

                                                                        elementstr = ElementBody.Template;
                                                                        var elementLabelValue = ((this.Side == "frontend") ? (buttonIconHtml + " " + ElementBody.FrontendLabel) : (buttonIconHtml + " " + ElementBody.BackendLabel));

                                                                        var attrs = " " + buttontarget + " " + popup + " " + dataAttrClickEvent + responseTarget + " class='" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "' " + attr;
                                                                        //elementstr = "<button type='button' " + buttontarget + " " + popup + " " + dataAttrClickEvent + responseTarget + " class='" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "' " + attr + " >" + ((this.Side == "frontend") ? (buttonIconHtml + " " + ElementBody.FrontendLabel) : (buttonIconHtml + " " + ElementBody.BackendLabel)) + "</button>"
                                                                        //    + "<input type='hidden' data-target-setting-for='elm" + ElementBody.ElementId + "' value='" + responseTargetValue + "'/>";

                                                                        elms = string.Format(elementstr, attrs, elementLabelValue);
                                                                        elms = elms + "<input type='hidden' data-target-setting-for='elm" + ElementBody.ElementId + "' value='" + responseTargetValue + "'/>";
                                                                        //elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), "", attr);
                                                                        break;

                                                                    #endregion
                                                                    #region recaptcha
                                                                    case "recaptcha":
                                                                        if (this.Side == "frontend")
                                                                        {
                                                                            attr = this.GenerateValidationData(ElementBody) + priviledge + " name=\"recaptcha" + ElementBody.ElementId + "\" id=\"recaptcha" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.FrontendClass + "\"";
                                                                        }
                                                                        else
                                                                        {
                                                                            attr = this.GenerateValidationData(ElementBody) + priviledge + " name =\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.BackendClass + "\"";

                                                                        }
                                                                        elementstr = ElementBody.Template;
                                                                        var className = ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass);
                                                                        if (className == null)
                                                                        {
                                                                            className = string.Empty;
                                                                        }
                                                                        var dataSite = "data-site=" + ElementBody.SiteKey + " " + "data-callback=verifyCallback";
                                                                        var rnd = new Random();
                                                                        var id = "id=" + ((this.Side == "frontend") ? ElementBody.FrontendId + rnd.Next().ToString() : ElementBody.BackendId + rnd.Next().ToString());
                                                                        elms = string.Format(elementstr, className, dataSite, attr, id);
                                                                        break;

                                                                    #endregion
                                                                    #region hyperlink
                                                                    case "hyperlink":
                                                                        bool hyperlinkIconVisibility = ((this.Side == "frontend") ? ElementBody.FrontendIconVisibility : ElementBody.BackendIconVisibility);
                                                                        popup = string.Empty;
                                                                        var hyperlinkIconHtml = string.Empty;
                                                                        if (hyperlinkIconVisibility)
                                                                        {
                                                                            hyperlinkIconHtml = "<i class='" + ((this.Side == "frontend") ? ElementBody.FrontendIcon : ElementBody.BackendIcon) + "'></i>";
                                                                        }

                                                                        string href = ((this.Side == "frontend") ? ElementBody.FrontendHref : ElementBody.BackendHref);
                                                                        if (href == null || href == "")
                                                                        {
                                                                            href = "javascript::";
                                                                        }
                                                                        string _target = ((this.Side == "frontend") ? ElementBody.FrontendTarget : ElementBody.BackendTarget);
                                                                        if (ElementBody.EnablePopup)
                                                                        {
                                                                            string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                            popup = popup + " data-popup-class='" + popupClass + "'";
                                                                            if (ElementBody.DisablePopupClose)
                                                                            {
                                                                                popup = popup + " data-popup-close-disable='true'";
                                                                            }
                                                                            popup = popup + " data-popup-enable='true'";
                                                                            if (ElementBody.PopUpTarget != null && ElementBody.PopUpTarget.Count > 0)
                                                                            {
                                                                                string t = string.Empty;

                                                                                for (int i = 0; i < ElementBody.PopUpTarget.Count; i++)
                                                                                {
                                                                                    if (ElementBody.PopUpTarget[i] != "" && ElementBody.PopUpTarget[i] != null)
                                                                                    {
                                                                                        t = t + "," + ElementBody.PopUpTarget[i];
                                                                                    }

                                                                                }
                                                                                if (t != string.Empty)
                                                                                {
                                                                                    t = t.Substring(1);
                                                                                    popup = popup + " data-popup-target='" + t + "'";
                                                                                }

                                                                            }
                                                                            popup = popup + " data-popup-title='" + ElementBody.PopUpTitle + "'";
                                                                        }
                                                                        else
                                                                        {
                                                                            popup = " data-popup-enable='false'";
                                                                        }
                                                                        elementstr = ElementBody.Template;
                                                                        var attributes = " " + "' target='" + _target + "'" + popup + " class='" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "' " + attr + " name =elm" + ElementBody.ElementId + " id=elm" + ElementBody.ElementId;

                                                                        elms = string.Format(elementstr, href, attributes, ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel));
                                                                        //elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), "", attr);
                                                                        break;

                                                                    #endregion
                                                                    #region paragraph
                                                                    case "paragraph":
                                                                        elementstr = ElementBody.Template;
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), ElementBody.ParagraphText, attr);
                                                                        break;
                                                                    #endregion
                                                                    #region TextArea
                                                                    case "textarea":
                                                                        string dataAttrKeyUpEvent = string.Empty;

                                                                        if (ElementBody.IsOnKeyUpEvent)
                                                                        {
                                                                            dataAttrKeyUpEvent = "data-work-onkeyup='true' data-action-onkeyup='onKeyUp'";
                                                                            if (ElementBody.OnKeyUpEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnKeyUpEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnKeyUpEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnKeyUpEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnKeyUpEvent.TakeValueFromElementValidation[i];

                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-parname='" + t + "' data-val-onkeyup-parelm='" + t1 + "' data-val-onkeyup-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnKeyUpEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnKeyUpEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnKeyUpEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnKeyUpEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnKeyUpEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnKeyUpEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-resname='" + r + "' data-val-onkeyup-reselm='" + r1 + "' data-val-onkeyup-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-work-onkeyup-start='" + ElementBody.OnKeyUpEvent.StartState + "' data-work-onkeyup-end='" + ElementBody.OnKeyUpEvent.EndState + "' data-work-onkeyup-formid='" + formid + "'";
                                                                        }

                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), "", attr + " " + dataAttrKeyUpEvent);
                                                                        break;
                                                                    #endregion
                                                                    #region Heading
                                                                    case "heading":
                                                                        elms = "<" + ElementBody.HeaderType + " class = '" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "' " + attr + " " + ElementBody.Attribute + "> " + ElementBody.HeaderText + " </" + ElementBody.HeaderType + ">";
                                                                        break;
                                                                    #endregion
                                                                    #region TextBox
                                                                    case "textbox":
                                                                        string txtfrm = "";
                                                                        string tel = "";
                                                                        var txtformatter = (this.Side == "frontend") ? ElementBody.FrontendFormatter : ElementBody.BackendFormatter;
                                                                        if (txtformatter != "" && txtformatter != null)
                                                                        {
                                                                            txtfrm = " data-inputmask=\"" + txtformatter + "\"";
                                                                        }
                                                                        if (ElementBody.IsTelephoneNumber == true)
                                                                        {
                                                                            tel = " data-inputmask='\"mask\":\"" + ElementBody.TelephoneNumberFormat + "\"'";

                                                                        }

                                                                        dataAttrKeyUpEvent = string.Empty;
                                                                        if (ElementBody.IsOnKeyUpEvent)
                                                                        {
                                                                            dataAttrKeyUpEvent = "data-work-onkeyup='true' data-action-onkeyup='onKeyUp'";
                                                                            if (ElementBody.OnKeyUpEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnKeyUpEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnKeyUpEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnKeyUpEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnKeyUpEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-parname='" + t + "' data-val-onkeyup-parelm='" + t1 + "' data-val-onkeyup-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnKeyUpEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnKeyUpEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnKeyUpEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnKeyUpEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnKeyUpEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnKeyUpEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-resname='" + r + "' data-val-onkeyup-reselm='" + r1 + "' data-val-onkeyup-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-work-onkeyup-start='" + ElementBody.OnKeyUpEvent.StartState + "' data-work-onkeyup-end='" + ElementBody.OnKeyUpEvent.EndState + "' data-work-onkeyup-formid='" + formid + "'";
                                                                        }


                                                                        string radioGroups = ((this.Side == "frontend") ? ElementBody.RadioGroup : ElementBody.RadioGroup);
                                                                        var rgAttr = "";
                                                                        if (radioGroups != null)
                                                                        {
                                                                            rgAttr = " data-element-radio-ids=" + radioGroups;
                                                                        }
                                                                        elms = string.Format(elementstr, attr + rgAttr + sfvAttr + txtfrm + tel + " " + dataAttrKeyUpEvent);
                                                                        break;
                                                                    #endregion
                                                                    #region Number
                                                                    case "number":

                                                                        string nmfrm = "";
                                                                        var nmformatter = (this.Side == "frontend") ? ElementBody.FrontendFormatter : ElementBody.BackendFormatter;
                                                                        string ntarget = "";
                                                                        string defaultValue = "";
                                                                        if (ElementBody.DefaultValue != null)
                                                                        {
                                                                            defaultValue = " value='" + ElementBody.DefaultValue + "'";
                                                                        }
                                                                        if (nmformatter != "")
                                                                        {
                                                                            nmfrm = " data-types=\"" + nmformatter + "\"";

                                                                        }

                                                                        dataAttrKeyUpEvent = string.Empty;
                                                                        if (ElementBody.IsOnKeyUpEvent)
                                                                        {
                                                                            dataAttrKeyUpEvent = "data-work-onkeyup='true' data-action-onkeyup='onKeyUp'";
                                                                            if (ElementBody.OnKeyUpEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnKeyUpEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnKeyUpEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnKeyUpEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnKeyUpEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-parname='" + t + "' data-val-onkeyup-parelm='" + t1 + "' data-val-onkeyup-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnKeyUpEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnKeyUpEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnKeyUpEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnKeyUpEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnKeyUpEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnKeyUpEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-resname='" + r + "' data-val-onkeyup-reselm='" + r1 + "' data-val-onkeyup-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-work-onkeyup-start='" + ElementBody.OnKeyUpEvent.StartState + "' data-work-onkeyup-end='" + ElementBody.OnKeyUpEvent.EndState + "' data-work-onkeyup-formid='" + formid + "'";
                                                                        }
                                                                        string tempaa = "";
                                                                        if (ElementBody.IsCurrency == true)
                                                                        {
                                                                            string decimalDigit = string.Empty;
                                                                            if (ElementBody.DecimalDigit != null)
                                                                            {
                                                                                decimalDigit = ElementBody.DecimalDigit;
                                                                            }
                                                                            else
                                                                            {
                                                                                decimalDigit = "2";
                                                                            }
                                                                            label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;




                                                                            if (ElementBody.NumberType != null)
                                                                            {
                                                                                switch (ElementBody.NumberType)
                                                                                {
                                                                                    case (int)Number.NumType.Range:

                                                                                        string elmClass = (this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass;



                                                                                        elementstr = "<label for='elm" + ElementBody.ElementId + "'" + priviledge + ">" + label + "</label>" + ElementBody.CurrencyTemplate;
                                                                                        string elementstr1 = ElementBody.RangeTemplate;
                                                                                        string tempaa1 = "min='" + ElementBody.RangeMin + "' max='" + ElementBody.RangeMax + "' step='" + ElementBody.RangeStep + "'data-value-for='currency_" + ElementBody.ElementId + "'";
                                                                                        string abc = string.Format(elementstr1, attr + defaultValue + " " + dataAttrKeyUpEvent + " " + tempaa1, elmClass);
                                                                                        tempaa += "<input data-elm-type='currency' name='currency_" + ElementBody.ElementId + "' " + defaultValue + " id='currency_" + ElementBody.ElementId + "' type=\"text\" " + priviledge + " " + attr + " " + dataAttrKeyUpEvent + " class='form-control' data-inputmask=\"'alias': 'numeric', 'groupSeparator': ',', 'digits':" + decimalDigit + ", 'digitsOptional': false, 'prefix': '" + ElementBody.CurrencyType + " ', 'placeholder': '0'\" inputmode=\"numeric\" style=\"text-align: right;\"/>";
                                                                                        tempaa += abc;
                                                                                        tempaa += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                                        elms = string.Format(elementstr, tempaa, "");

                                                                                        break;
                                                                                    case (int)Number.NumType.Number:

                                                                                        elementstr = "<label for='elm" + ElementBody.ElementId + "'" + priviledge + ">" + label + "</label>" + ElementBody.CurrencyTemplate;
                                                                                        tempaa += "<input name = 'elm" + ElementBody.ElementId + "' " + defaultValue + " " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'number' hidden  data-value-for='currency_" + ElementBody.ElementId + "' " + dataAttrKeyUpEvent + " class='custom-control-input' id='elm" + ElementBody.ElementId + "' />";

                                                                                        tempaa += "<input data-elm-type='currency' name='currency_" + ElementBody.ElementId + "' " + defaultValue + " id='currency_" + ElementBody.ElementId + "' type=\"text\" " + priviledge + " " + attr + " " + dataAttrKeyUpEvent + " class='form-control' data-inputmask=\"'alias': 'numeric', 'groupSeparator': ',', 'digits':" + decimalDigit + ", 'digitsOptional': false, 'prefix': '" + ElementBody.CurrencyType + " ', 'placeholder': '0'\" inputmode=\"numeric\" style=\"text-align: right;\"/>";
                                                                                        tempaa += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                                        elms = string.Format(elementstr, tempaa, "");
                                                                                        break;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "'" + priviledge + ">" + label + "</label>" + ElementBody.CurrencyTemplate;
                                                                                tempaa += "<input name = 'elm" + ElementBody.ElementId + "' " + defaultValue + " " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'number' hidden  data-value-for='currency_" + ElementBody.ElementId + "' " + dataAttrKeyUpEvent + " class='custom-control-input' id='elm" + ElementBody.ElementId + "' />";

                                                                                tempaa += "<input data-elm-type='currency' name='currency_" + ElementBody.ElementId + "' " + defaultValue + " id='currency_" + ElementBody.ElementId + "' type=\"text\" " + priviledge + " " + attr + " " + dataAttrKeyUpEvent + " class='form-control' data-inputmask=\"'alias': 'numeric', 'groupSeparator': ',', 'digits':" + decimalDigit + ", 'digitsOptional': false, 'prefix': '" + ElementBody.CurrencyType + " ', 'placeholder': '0'\" inputmode=\"numeric\" style=\"text-align: right;\"/>";
                                                                                tempaa += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                                elms = string.Format(elementstr, tempaa, "");
                                                                            }


                                                                        }
                                                                        else
                                                                        {
                                                                            List<Number.TargetOption> ntargets = (List<Number.TargetOption>)ElementBody.TargetOptions;
                                                                            if (ntargets != null)
                                                                            {
                                                                                foreach (var targetoption in ntargets)
                                                                                {
                                                                                    if (targetoption.TargetId != "0")
                                                                                    {
                                                                                        if (ntargets[0] == targetoption)
                                                                                        {
                                                                                            ntarget = "#" + targetoption.TargetId;

                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            ntarget = ntarget + ",#" + targetoption.TargetId;

                                                                                        }
                                                                                    }

                                                                                }
                                                                            }

                                                                            if (!string.IsNullOrWhiteSpace(ntarget))
                                                                            {
                                                                                ntarget = " data-target='" + ntarget + "' number-target";
                                                                            }
                                                                            if (ElementBody.NumberType != null)
                                                                            {
                                                                                switch (ElementBody.NumberType)
                                                                                {
                                                                                    case (int)Number.NumType.Range:
                                                                                        label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                                        string elmClass = (this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass;
                                                                                        elementstr = "<label for='elm" + ElementBody.ElementId + "'" + priviledge + ">" + label + "</label>" + ElementBody.RangeTemplate;
                                                                                        tempaa = "min='" + ElementBody.RangeMin + "' max='" + ElementBody.RangeMax + "' step='" + ElementBody.RangeStep + "'";
                                                                                        elms = string.Format(elementstr, attr + ntarget + defaultValue + " " + dataAttrKeyUpEvent + " " + tempaa, elmClass);
                                                                                        break;
                                                                                    case (int)Number.NumType.Number:
                                                                                        elms = string.Format(elementstr, attr + ntarget + defaultValue + nmfrm + " " + dataAttrKeyUpEvent);
                                                                                        break;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                elms = string.Format(elementstr, attr + ntarget + defaultValue + nmfrm + " " + dataAttrKeyUpEvent);
                                                                            }

                                                                        }

                                                                        break;
                                                                    #endregion
                                                                    #region SelectBox
                                                                    case "selectbox":
                                                                        string seloptions = "";
                                                                        string selected = string.Empty;
                                                                        string dataAttrChangeEvent = string.Empty;
                                                                        string dataAttrLoadEvent = string.Empty;
                                                                        if (ElementBody.IsOnChangeEvent)
                                                                        {
                                                                            dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                            if (ElementBody.OnChangeEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnChangeEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnChangeEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnChangeEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnChangeEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnChangeEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + ElementBody.OnChangeEvent.StartState + "' data-work-onchange-end='" + ElementBody.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                        }


                                                                        if (ElementBody.IsOnLoadEvent)
                                                                        {
                                                                            dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                            if (ElementBody.OnLoadEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnLoadEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnLoadEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnLoadEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnLoadEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnLoadEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + ElementBody.OnLoadEvent.StartState + "' data-work-onload-end='" + ElementBody.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                        }

                                                                        if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                        {

                                                                            if ((this.Side == "frontend" && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("IMAGE")) || (this.Side == "backend" && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("IMAGE")))
                                                                            {
                                                                                label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "' " + priviledgeAlt + ">" + label + "</label>" + ElementBody.AltTemplate;
                                                                                var altTemplate = (ElementBody.AltTemplate.Contains("exchange-type") ? true : false);
                                                                                var count = 0;
                                                                                //elementstr = ElementBody.AltTemplate;
                                                                                string value = string.Empty;
                                                                                selected = string.Empty;
                                                                                string toggle = string.Empty;

                                                                                foreach (var soption in ElementBody.SelectOptions)
                                                                                {
                                                                                    if (soption.Selected == true)
                                                                                    {
                                                                                        selected = " show ";
                                                                                        value = soption.Value;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }

                                                                                    if (soption.ToggleOptions == true)
                                                                                    {
                                                                                        toggle = "toggle-options='true'";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        toggle = "toggle-options='false'";
                                                                                    }
                                                                                    string target = "";

                                                                                    List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }

                                                                                    count++;
                                                                                    var prClass = string.Empty;
                                                                                    prClass = "mb-3";

                                                                                    if (altTemplate)
                                                                                    {
                                                                                        if (count == 1)
                                                                                        {
                                                                                            prClass = "pr-md-0";
                                                                                        }
                                                                                        else if (count == ElementBody.SelectOptions.Count)
                                                                                        {
                                                                                            prClass = "pl-md-0";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            prClass = "p-md-0";
                                                                                        }
                                                                                    }
                                                                                    List<SelectBox.PopUpTargetOption> popUpTargetOptions = (List<SelectBox.PopUpTargetOption>)ElementBody.PopUpTarget;
                                                                                    popup = string.Empty;
                                                                                    if (ElementBody.EnablePopup)
                                                                                    {
                                                                                        string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                                        popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                        if (ElementBody.DisablePopupClose)
                                                                                        {
                                                                                            popup = popup + " data-popup-close-disable='true'";
                                                                                        }
                                                                                        if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                        {
                                                                                            var result = popUpTargetOptions.Where(x => x.SelectId == soption.Value).ToList();
                                                                                            foreach (SelectBox.PopUpTargetOption popTarget in result)
                                                                                            {
                                                                                                if ((bool)popTarget.EnablePopup)
                                                                                                {
                                                                                                    popup = popup + " data-popup-enable='true'";
                                                                                                    string t = string.Empty;


                                                                                                    if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                    {
                                                                                                        t = t + "," + popTarget.TargetId;
                                                                                                    }

                                                                                                    if (t != string.Empty)
                                                                                                    {
                                                                                                        t = t.Substring(1);
                                                                                                        popup = popup + " data-popup-target='" + t + "'";

                                                                                                        popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                    }

                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    popup = " data-popup-enable='false'";
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    seloptions = seloptions + "<div " + target + " class=\"  col-md-3 " + prClass + "\"><a onclick='setValueOn(\"elm" + ElementBody.ElementId + "\",\"" + soption.Value + "\")' class='Image-target module-tab" + selected + "' value ='" + soption.Value + "' " + selected + " " + target + " " + popup + " " + toggle + " name=''><i class=\"module-icon module-icon--check\"></i><div class='module-type module-type--img d-flex justify-content-center align-items-center'><img src = '/images/" + soption.IconUrl + ".png' alt = '" + soption.IconUrl + "' ></div><p class='m-0'>" + soption.Text + "</p></a></div>";

                                                                                }
                                                                                seloptions += "<input name = 'elm" + ElementBody.ElementId + "' type= 'text-box'" + priviledgeAlt + this.GenerateValidationData(ElementBody) + " value = '" + value + "' data-elm-select = 'elmSelect' class='custom-control-input' id='elm" + ElementBody.ElementId + "' " + dataAttrChangeEvent + " " + dataAttrLoadEvent + " " + attr + sfvAttr + "/>";
                                                                                seloptions += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";
                                                                                elms = string.Format(elementstr, seloptions, " ", priviledgeAlt + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent);
                                                                            }



                                                                            else
                                                                            {
                                                                                if (ElementBody.UseFromDatabase == false)
                                                                                {

                                                                                    foreach (var soption in ElementBody.SelectOptions)
                                                                                    {
                                                                                        if (soption.Selected == true)
                                                                                        {
                                                                                            selected = "selected";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            selected = "";
                                                                                        }

                                                                                        string target = "";

                                                                                        List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                            }
                                                                                        }
                                                                                        List<SelectBox.PopUpTargetOption> popUpTargetOptions = (List<SelectBox.PopUpTargetOption>)ElementBody.PopUpTarget;
                                                                                        popup = string.Empty;
                                                                                        if (ElementBody.EnablePopup)
                                                                                        {
                                                                                            string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                                            popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                            if (ElementBody.DisablePopupClose)
                                                                                            {
                                                                                                popup = popup + " data-popup-close-disable='true'";
                                                                                            }
                                                                                            if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                            {
                                                                                                var result = popUpTargetOptions.Where(x => x.SelectId == soption.Value).ToList();
                                                                                                foreach (SelectBox.PopUpTargetOption popTarget in result)
                                                                                                {
                                                                                                    if ((bool)popTarget.EnablePopup)
                                                                                                    {
                                                                                                        popup = popup + " data-popup-enable='true'";
                                                                                                        string t = string.Empty;


                                                                                                        if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                        {
                                                                                                            t = t + "," + popTarget.TargetId;
                                                                                                        }

                                                                                                        if (t != string.Empty)
                                                                                                        {
                                                                                                            t = t.Substring(1);
                                                                                                            popup = popup + " data-popup-target='" + t + "'";

                                                                                                            popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                        }


                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        popup = " data-popup-enable='false'";
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }

                                                                                        seloptions = seloptions + "<option name='" + ElementBody.ElementId + "-" + soption.Value + "' value='" + soption.Value + "' " + selected + " " + target + " " + popup + ">" + soption.Text + "</option>";

                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    string optionText = ElementBody.SelectFromDbFieldText;
                                                                                    string optionValue = ElementBody.SelectFromDbFieldValue;
                                                                                    string optionIcon = ElementBody.SelectFromDbIconUrl;
                                                                                    string tableName = ElementBody.SelectFromDbTable;
                                                                                    int selectedRow = ElementBody.SelectFromDbFieldSelectedOnRow;
                                                                                    DataTable dataTable = new DataTable();
                                                                                    var CommandText = "Select [" + optionText + "], [" + optionValue + "],[" + optionIcon + "] from " + tableName;
                                                                                    if (this._db == null) this._db = this.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
                                                                                    using (var command = _db.Database.GetDbConnection().CreateCommand())
                                                                                    {
                                                                                        command.CommandText = CommandText;
                                                                                        _db.Database.OpenConnection();
                                                                                        try
                                                                                        {
                                                                                            SqlConnection connection = new SqlConnection(command.Connection.ConnectionString);
                                                                                            SqlCommand sql = new SqlCommand(command.CommandText, connection);
                                                                                            using (SqlDataAdapter result = new SqlDataAdapter(sql))
                                                                                            {
                                                                                                result.Fill(dataTable);
                                                                                            }
                                                                                        }
                                                                                        catch (Exception ex)
                                                                                        {

                                                                                        }

                                                                                    }


                                                                                    if (dataTable.Rows.Count > 0)
                                                                                    {
                                                                                        int p = 0;
                                                                                        foreach (DataRow dRow in dataTable.Rows)
                                                                                        {
                                                                                            p = p + 1;
                                                                                            if (Convert.ToInt16(ElementBody.SelectFromDbFieldSelectedOnRow) == p)
                                                                                            {
                                                                                                selected = "selected";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                selected = "";
                                                                                            }

                                                                                            string target = "";

                                                                                            //List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                            //if (targets != null)
                                                                                            //{
                                                                                            //    var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                            //    string toption = "";
                                                                                            //    string taction = "";

                                                                                            //    foreach (var targetoption in result)
                                                                                            //    {
                                                                                            //        if (result[0] == targetoption)
                                                                                            //        {
                                                                                            //            toption = "#" + targetoption.TargetId;

                                                                                            //            taction = "" + targetoption.ShowHide + "";
                                                                                            //        }
                                                                                            //        else
                                                                                            //        {
                                                                                            //            toption = toption + ",#" + targetoption.TargetId;

                                                                                            //            taction = taction + "," + targetoption.ShowHide + "";
                                                                                            //        }

                                                                                            //    }

                                                                                            //    if (!string.IsNullOrWhiteSpace(toption))
                                                                                            //    {
                                                                                            //        target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                            //    }
                                                                                            //}


                                                                                            seloptions = seloptions + "<option value='" + dRow[optionValue] + "' " + selected + " " + target + ">" + dRow[optionText] + "</option>";

                                                                                        }
                                                                                    }
                                                                                }
                                                                                elms = string.Format(elementstr, attr + sfvAttr + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent + " select-target", seloptions);
                                                                            }

                                                                        }

                                                                        break;
                                                                    #endregion
                                                                    #region MultiSelectBox
                                                                    case "multiselectbox":
                                                                        string mulseloptions = "";
                                                                        dataAttrChangeEvent = string.Empty;
                                                                        dataAttrLoadEvent = string.Empty;
                                                                        if (ElementBody.IsOnChangeEvent)
                                                                        {
                                                                            dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                            if (ElementBody.OnChangeEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnChangeEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnChangeEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnChangeEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnChangeEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnChangeEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + ElementBody.OnChangeEvent.StartState + "' data-work-onchange-end='" + ElementBody.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                        }


                                                                        if (ElementBody.IsOnLoadEvent)
                                                                        {
                                                                            dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                            if (ElementBody.OnLoadEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnLoadEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnLoadEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnLoadEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnLoadEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnLoadEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + ElementBody.OnLoadEvent.StartState + "' data-work-onload-end='" + ElementBody.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                        }
                                                                        if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                        {

                                                                            selected = string.Empty;
                                                                            foreach (var msoption in ElementBody.SelectOptions)
                                                                            {
                                                                                if (msoption.Selected == true)
                                                                                {
                                                                                    selected = "selected";
                                                                                }
                                                                                else
                                                                                {
                                                                                    selected = "";
                                                                                }

                                                                                mulseloptions = mulseloptions + "<option value='" + msoption.Value + "' " + selected + ">" + msoption.Text + "</option>";
                                                                            }
                                                                        }

                                                                        elms = string.Format(elementstr, attr + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent, mulseloptions);
                                                                        break;
                                                                    #endregion
                                                                    #region RadioGroup
                                                                    case "radiogroup":
                                                                        string radoptions = "";
                                                                        dataAttrChangeEvent = string.Empty;
                                                                        dataAttrLoadEvent = string.Empty;
                                                                        if (ElementBody.IsOnChangeEvent)
                                                                        {
                                                                            dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                            if (ElementBody.OnChangeEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnChangeEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnChangeEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnChangeEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnChangeEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnChangeEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + ElementBody.OnChangeEvent.StartState + "' data-work-onchange-end='" + ElementBody.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                        }


                                                                        if (ElementBody.IsOnLoadEvent)
                                                                        {
                                                                            dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                            if (ElementBody.OnLoadEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnLoadEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnLoadEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnLoadEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnLoadEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnLoadEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + ElementBody.OnLoadEvent.StartState + "' data-work-onload-end='" + ElementBody.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                        }

                                                                        if (ElementBody.RadioOptions != null && ElementBody.RadioOptions.Count > 0)
                                                                        {
                                                                            selected = string.Empty;
                                                                            int i = 0;
                                                                            if (((this.Side == "frontend" && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("ONOFF")) || (this.Side == "backend" && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("ONOFF"))) && ElementBody.RadioOptions.Count == 2)
                                                                            {
                                                                                string targetY = "";
                                                                                string targetN = "";
                                                                                string vals = "false";
                                                                                popup = string.Empty;
                                                                                foreach (var roption in ElementBody.RadioOptions)
                                                                                {
                                                                                    if (roption.Value == "Y" || roption.Value == "Yes")
                                                                                    {
                                                                                        vals = "true";
                                                                                        List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                targetY = "data-targety='" + toption + "' data-actiony='" + taction + "'";
                                                                                            }
                                                                                        }
                                                                                        List<RadioGroup.PopUpTargetOption> popUpTargetOptions = (List<RadioGroup.PopUpTargetOption>)ElementBody.PopUpTarget;

                                                                                        if (ElementBody.EnablePopup)
                                                                                        {
                                                                                            string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                                            popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                            if (ElementBody.DisablePopupClose)
                                                                                            {
                                                                                                popup = popup + " data-popup-close-disable='true'";
                                                                                            }
                                                                                            if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                            {
                                                                                                var result = popUpTargetOptions.Where(x => x.SelectId == roption.Value).ToList();
                                                                                                foreach (RadioGroup.PopUpTargetOption popTarget in result)
                                                                                                {
                                                                                                    if ((bool)popTarget.EnablePopup)
                                                                                                    {
                                                                                                        popup = popup + " data-popup-enable='true'";
                                                                                                        string t = string.Empty;


                                                                                                        if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                        {
                                                                                                            t = t + "," + popTarget.TargetId;
                                                                                                        }
                                                                                                        if (t != string.Empty)
                                                                                                        {
                                                                                                            t = t.Substring(1);
                                                                                                            popup = popup + " data-popup-target='" + t + "'";

                                                                                                            popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                        }


                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        popup = " data-popup-enable='false'";
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (roption.Selected)
                                                                                        {
                                                                                            selected = "checked";
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }
                                                                                            List<RadioGroup.PopUpTargetOption> popUpTargetOptions = (List<RadioGroup.PopUpTargetOption>)ElementBody.PopUpTarget;

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                targetN = "data-targetn='" + toption + "' data-actionn='" + taction + "'";
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    i++;
                                                                                }
                                                                                string v = "False";
                                                                                if (selected == "checked")
                                                                                {
                                                                                    v = "True";
                                                                                }

                                                                                radoptions += "<input type='text' value=" + v + " name='elm" + ElementBody.ElementId + "' hidden data-for-switch='check" + ElementBody.ElementId + "'/> <div  class='custom-control custom-switch " + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + " '> <input  " + targetY + " " + targetN + " data-toggle='slider' name='check" + ElementBody.ElementId + "' type='checkbox'" + priviledgeAlt + " " + dataAttrChangeEvent + " " + popup + " " + dataAttrLoadEvent + this.GenerateValidationData(ElementBody) + " value='" + vals + "' class='custom-control-input radio-slider-target' id='" + ElementBody.ElementId + "switch'  " + selected + " data-for-check='elm" + ElementBody.ElementId + "' />  <label class='custom-control-label' for='" + ElementBody.ElementId + "switch'" + priviledgeAlt + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent + ">  </label> </div>";

                                                                            }
                                                                            else
                                                                            {
                                                                                foreach (var roption in ElementBody.RadioOptions)
                                                                                {
                                                                                    if (roption.Selected == true)
                                                                                    {
                                                                                        selected = "checked";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }
                                                                                    string target = "";

                                                                                    List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }
                                                                                    List<RadioGroup.PopUpTargetOption> popUpTargetOptions = (List<RadioGroup.PopUpTargetOption>)ElementBody.PopUpTarget;
                                                                                    popup = string.Empty;
                                                                                    if (ElementBody.EnablePopup)
                                                                                    {
                                                                                        string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                                        popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                        if (ElementBody.DisablePopupClose)
                                                                                        {
                                                                                            popup = popup + " data-popup-close-disable='true'";
                                                                                        }
                                                                                        if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                        {
                                                                                            var result = popUpTargetOptions.Where(x => x.SelectId == roption.Value).ToList();
                                                                                            foreach (RadioGroup.PopUpTargetOption popTarget in result)
                                                                                            {
                                                                                                if ((bool)popTarget.EnablePopup)
                                                                                                {
                                                                                                    popup = popup + " data-popup-enable='true'";
                                                                                                    string t = string.Empty;


                                                                                                    if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                    {
                                                                                                        t = t + "," + popTarget.TargetId;
                                                                                                    }

                                                                                                    if (t != string.Empty)
                                                                                                    {
                                                                                                        t = t.Substring(1);
                                                                                                        popup = popup + " data-popup-target='" + t + "'";

                                                                                                        popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                    }


                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    popup = " data-popup-enable='false'";
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    var hasIcon = string.Empty;
                                                                                    hasIcon = ((roption.Icon == null || roption.Icon == "") ? "" : "has-icon");
                                                                                    //radoptions += "<div class='custom-control custom-radio custom-control-inline'><input class='custom-control-input' type= 'radio' " + selected + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + (Utils.ConvertToString(roption.Value) == "" ? (ElementBody.ElementId + i) : roption.Value) + "'><label class='custom-control-label' for='elm" + roption.Value + "'>" + roption.Text + "</label></div>";
                                                                                    radoptions += "<div " + target + " class='custom-control custom-radio custom-control-inline " + hasIcon + "'><input " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'radio' " + selected + " " + target + " " + popup + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + ElementBody.ElementId + roption.Value + "' " + attr + "><i class='" + roption.Icon + "' for='elm" + ElementBody.ElementId + roption.Value + "' " + "></i><label class='custom-control-label' for='elm" + ElementBody.ElementId + roption.Value + "'" + priviledgeAlt + "> " + roption.Text + "</label></div>";
                                                                                    i++;
                                                                                }
                                                                            }
                                                                        }

                                                                        elms = "<div class='mb-2 '" + priviledgeAlt + ">" + ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel) + "</div><div>" + radoptions + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div></div>";
                                                                        break;
                                                                    #endregion
                                                                    #region CheckBoxGroup
                                                                    case "checkboxgroup":
                                                                        string isInline = (this.Side == "frontend") ? ElementBody.FrontendCheckboxStyle : ElementBody.BackendCheckboxStyle;
                                                                        var inlineClass = string.Empty;
                                                                        if (isInline == "1")
                                                                        {
                                                                            inlineClass = "custom-control-inline";
                                                                        }
                                                                        string chkoptions = "";
                                                                        dataAttrChangeEvent = string.Empty;
                                                                        dataAttrLoadEvent = string.Empty;
                                                                        if (ElementBody.IsOnChangeEvent)
                                                                        {
                                                                            dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                            if (ElementBody.OnChangeEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnChangeEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnChangeEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnChangeEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnChangeEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnChangeEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnChangeEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + ElementBody.OnChangeEvent.StartState + "' data-work-onchange-end='" + ElementBody.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                        }


                                                                        if (ElementBody.IsOnLoadEvent)
                                                                        {
                                                                            dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                            if (ElementBody.OnLoadEvent.TakeValueFromName != null)
                                                                            {
                                                                                string t = string.Empty;
                                                                                string t1 = string.Empty;
                                                                                string t2 = string.Empty;
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                {
                                                                                    t = t + "," + ElementBody.OnLoadEvent.TakeValueFromName[i];
                                                                                    t1 = t1 + "," + ElementBody.OnLoadEvent.TakeValueFromElement[i];
                                                                                    t2 = t2 + "," + ElementBody.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                }
                                                                                t = t.Substring(1);
                                                                                t1 = t1.Substring(1);
                                                                                t2 = t2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                            }
                                                                            if (ElementBody.OnLoadEvent.SetValueToName != null)
                                                                            {
                                                                                string r = string.Empty;
                                                                                string r1 = string.Empty;
                                                                                string r2 = string.Empty;
                                                                                bool EventTriggerNull = false;
                                                                                if (ElementBody.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                {
                                                                                    EventTriggerNull = true;
                                                                                }
                                                                                for (int i = 0; i < ElementBody.OnLoadEvent.SetValueToName.Count; i++)
                                                                                {
                                                                                    r = r + "," + ElementBody.OnLoadEvent.SetValueToName[i];
                                                                                    r1 = r1 + "," + ElementBody.OnLoadEvent.SetValueToElement[i];
                                                                                    r2 = r2 + "," + (!EventTriggerNull ? ElementBody.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                }
                                                                                r = r.Substring(1);
                                                                                r1 = r1.Substring(1);
                                                                                r2 = r2.Substring(1);
                                                                                dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                            }
                                                                            dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + ElementBody.OnLoadEvent.StartState + "' data-work-onload-end='" + ElementBody.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                        }

                                                                        if (ElementBody.CheckOptions != null && ElementBody.CheckOptions.Count > 0)
                                                                        {

                                                                            selected = string.Empty;
                                                                            foreach (var coption in ElementBody.CheckOptions)
                                                                            {
                                                                                var hasIcon = string.Empty;
                                                                                hasIcon = ((coption.Icon == null || coption.Icon == "") ? "" : "has-icon");
                                                                                if (coption.Selected == true)
                                                                                {
                                                                                    selected = "checked";
                                                                                }
                                                                                else
                                                                                {
                                                                                    selected = "";
                                                                                }

                                                                                string target = "";

                                                                                List<CheckBoxGroup.TargetOption> targets = (List<CheckBoxGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                if (targets != null)
                                                                                {
                                                                                    var result = targets.Where(x => x.SelectId == coption.Value).ToList();

                                                                                    string toption = "";
                                                                                    string taction = "";

                                                                                    foreach (var targetoption in result)
                                                                                    {
                                                                                        if (result[0] == targetoption)
                                                                                        {
                                                                                            toption = "#" + targetoption.TargetId;

                                                                                            taction = targetoption.ShowHide;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            toption = toption + ",#" + targetoption.TargetId;

                                                                                            taction = taction + "," + targetoption.ShowHide;
                                                                                        }

                                                                                    }

                                                                                    if (!string.IsNullOrWhiteSpace(toption))
                                                                                    {
                                                                                        target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                    }
                                                                                }
                                                                                List<CheckBoxGroup.PopUpTargetOption> popUpTargetOptions = (List<CheckBoxGroup.PopUpTargetOption>)ElementBody.PopUpTarget;
                                                                                popup = string.Empty;
                                                                                if (ElementBody.EnablePopup)
                                                                                {
                                                                                    string popupClass = (ElementBody.PopUpClass == null) ? "modal-lg" : ElementBody.PopUpClass;
                                                                                    popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                    if (ElementBody.DisablePopupClose)
                                                                                    {
                                                                                        popup = popup + " data-popup-close-disable='true'";
                                                                                    }
                                                                                    if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                    {
                                                                                        var result = popUpTargetOptions.Where(x => x.SelectId == coption.Value).ToList();
                                                                                        foreach (CheckBoxGroup.PopUpTargetOption popTarget in result)
                                                                                        {
                                                                                            if ((bool)popTarget.EnablePopup)
                                                                                            {
                                                                                                popup = popup + " data-popup-enable='true'";
                                                                                                string t = string.Empty;


                                                                                                if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                {
                                                                                                    t = t + "," + popTarget.TargetId;
                                                                                                }

                                                                                                if (t != string.Empty)
                                                                                                {
                                                                                                    t = t.Substring(1);
                                                                                                    popup = popup + " data-popup-target='" + t + "'";

                                                                                                    popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                }

                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                popup = " data-popup-enable='false'";
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                var iHtml = string.Empty;
                                                                                if (coption.Icon != null)
                                                                                {
                                                                                    iHtml = "<i class='" + coption.Icon + "' for='elm" + coption.Value + "'></i>";
                                                                                }
                                                                                chkoptions += "<div class='custom-control custom-checkbox " + inlineClass + " " + hasIcon + "'><input class='custom-control-input' " + priviledgeAlt + " " + target + " " + popup + " data-checkbox=true" + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent + " " + this.GenerateValidationData(ElementBody) + " type= 'checkbox' " + selected + " value = '" + coption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + ElementBody.ElementId + coption.Value + "' " + attr + ">" + iHtml + "<label class='custom-control-label' for='elm" + ElementBody.ElementId + coption.Value + "'>" + coption.Text + "</label></div>";
                                                                            }
                                                                        }

                                                                        elms = "<div class='mb-2'>" + ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel) + "</div><div>" + chkoptions + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div></div>";
                                                                        break;
                                                                    #endregion
                                                                    #region Media
                                                                    case "media":
                                                                        //elementstr = elementstr;
                                                                        string elementidside = ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]";
                                                                        string elmId = "Media" + ElementBody.ElementId + "[]";
                                                                        string elmName = "Media" + ElementBody.ElementId + "[]";
                                                                        string media = @"<div class='fileuploader__list'><ul class='fileuploader__items'>";

                                                                        if (ElementBody.Images != null && ElementBody.Images.Count > 0)
                                                                        {
                                                                            foreach (var image in ElementBody.Images)
                                                                            {
                                                                                switch (System.IO.Path.GetExtension(image.Url))
                                                                                {
                                                                                    case ".pdf":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/pdf-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    case ".doc":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    case ".docx":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    case ".txt":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    default:
                                                                                        media += "<li class=\"fileuploader__item\"><a class='pop' href=\"javascript: void(0)\"><div class='fileuploader__item-image'><img src=\"/uploads/" + image.Url + "\"></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' data-toggle='tooltip' data-placement='top' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                }
                                                                            }
                                                                        }
                                                                        media += @" </ul></div><div class='fileuploader__input'><div class='fileuploader__input-caption'><span>Choose file</span></div><button type='button' class='btn fileuploader__input-button'" + priviledgeAlt + " data-media='{OnInit:function(){this.size=\"modal-lg\";this.button.cancel=\"Cancel\";this.button.insert=\"Insert Media\";this.open();},OnInsert:function(e){Cicero.Form.InsertImages(e , \"" + elementidside + "\")},OnCancelled:function(){this.close();}}'>" +
                                                                        "<span>Browse</span></button> </div><div class='d-flex'><span class='text danger field-validation-valid' data-valmsg-for='" + elementidside + "' data-valmsg-replace='true'></span></div>";
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), media, elmId, elmName);
                                                                        break;
                                                                    #endregion
                                                                    #region Table
                                                                    case "table":

                                                                        Table tableElement = ElementBody as Table;
                                                                        string tableStr = "<thead>";
                                                                        if (tableElement.Header != null && tableElement.Header.Count > 0)
                                                                        {
                                                                            var parents = tableElement.Header.Where(x => x.ParentId == "0").ToList();
                                                                            if (parents.Count > 0)
                                                                            {
                                                                                tableStr = tableStr + "<tr>";
                                                                                foreach (var parent in parents)
                                                                                {
                                                                                    tableStr = tableStr + "<th " + parent.Attribute + " colspan='" + parent.Colspan + "'>" + parent.HeaderTitle + "</th>";
                                                                                }
                                                                                tableStr = tableStr + "</tr>";
                                                                            }
                                                                            bool hasSubHeader = false;
                                                                            string subHeaderStr = "<tr>";
                                                                            if (parents.Count > 0)
                                                                            {
                                                                                foreach (var parent in parents)
                                                                                {
                                                                                    var children = tableElement.Header.Where(p => p.ParentId == parent.HeaderId).ToList();
                                                                                    foreach (var child in children)
                                                                                    {
                                                                                        hasSubHeader = true;
                                                                                        subHeaderStr = subHeaderStr + "<th " + child.Attribute + " colspan='" + child.Colspan + "'>" + child.HeaderTitle + "</th>";
                                                                                    }
                                                                                }
                                                                            }
                                                                            subHeaderStr = subHeaderStr + "</tr>";
                                                                            if (hasSubHeader)
                                                                            {
                                                                                tableStr = tableStr + subHeaderStr;
                                                                            }
                                                                            //if (tableElement.Column != null && tableElement.Column.Count > 0)
                                                                            //{
                                                                            //    tableStr = tableStr + "<tr>";
                                                                            //    foreach (var co in tableElement.Column)
                                                                            //    {
                                                                            //        string setting = string.Empty;

                                                                            //        if (co.Setting != null)
                                                                            //        {
                                                                            //            if(co.Setting.IsVisible !=null)
                                                                            //            {
                                                                            //                if ((bool)co.Setting.IsVisible)
                                                                            //                {
                                                                            //                    setting = "class='" + ((this.Side == "frontend") ? co.Setting.FrontendClass : co.Setting.BackendClass) + "' style='" + co.Setting.Style + "'";
                                                                            //                    tableStr = tableStr + "<th " + setting + ">" + co.Caption + "</th>";
                                                                            //                }
                                                                            //            }
                                                                            //        }
                                                                            //    }
                                                                            //    tableStr = tableStr + "</tr>";
                                                                            //}
                                                                        }
                                                                        tableStr = tableStr + "</thead>";
                                                                        var showHideLabel = string.Empty;
                                                                        var showHideElement = string.Empty;
                                                                        var rowOption = string.Empty;
                                                                        switch (tableElement.TType)
                                                                        {
                                                                            case (int)Table.TableType.InputTable:
                                                                                showHideLabel = " data-label-hide='true'";
                                                                                rowOption = "<td class=\"row-option\"><div class=\"field-controller\">"
                                                                                + "<a class=\"fc-icon icon-copy\" data-action=\"clone-rule\" title=\"Copy\">"
                                                                                + "<i class=\"ri-file-copy-line\"></i>"
                                                                                + "<span class=\"sr-only\">Copy</span></a>"
                                                                                + "<a class=\"fc-icon icon-delete\" data-action=\"remove-rule\" title=\"Delete\">"
                                                                                + "<i class=\"ri-delete-bin-line\"></i>"
                                                                                + "<span class=\"sr-only\">Delete</span></a>"
                                                                                + "</div></td>";
                                                                                break;
                                                                            case (int)Table.TableType.DisplayTable:
                                                                                showHideElement = " data-elm-hide='true'";
                                                                                break;
                                                                            case (int)Table.TableType.DisplayAndEdit:
                                                                                showHideElement = " data-elm-hide='true'";
                                                                                rowOption = "<td class=\"row-option\"><div class=\"field-controller\">"
                                                                                + "<a class=\"fc-icon icon-edit\" data-action=\"edit-rule\" title=\"Edit\">"
                                                                                + "<i class=\"ri-pencil-line\"></i>"
                                                                                + "<span class=\"sr-only\">Edit</span></a>"
                                                                                + "<a class=\"fc-icon icon-ok hide-icon\" data-action=\"ok-rule\" title=\"Ok\">"
                                                                                + "<i class=\"ri-check-line\"></i>"
                                                                                + "<span class=\"sr-only hide-icon\">Delete</span></a>"
                                                                                + "<a class=\"fc-icon icon-cancel hide-icon\" data-action=\"cancel-rule\" title=\"Cancel\">"
                                                                                + "<i class=\"ri-close-line\"></i>"
                                                                                + "<span class=\"sr-only\">Cancel</span></a>"
                                                                                + "<a class=\"fc-icon icon-delete hide-icon\" data-action=\"remove-rule\" title=\"Delete\">"
                                                                                + "<i class=\"ri-delete-bin-line\"></i>"
                                                                                + "<span class=\"sr-only\">Delete</span></a>"
                                                                                + "</div></td>";
                                                                                break;
                                                                        }
                                                                        if (tableElement.Column != null && tableElement.Column.Count > 0)
                                                                        {
                                                                            string tableColStr = "<tbody><tr>";

                                                                            foreach (var col in tableElement.Column)
                                                                            {
                                                                                dynamic colElement = col.ColumnElement;
                                                                                if (colElement != null)
                                                                                {
                                                                                    bool isColElementVisible = (this.Side == "frontend") ? colElement.FrontendVisible : colElement.BackendVisible;
                                                                                    if (isColElementVisible)
                                                                                    {
                                                                                        tableColStr = tableColStr + "<td>";
                                                                                        string elementCol = "";
                                                                                        string colElmAttr = "data";
                                                                                        string colElmAttrAlt = "data";

                                                                                        //replace with another logic for view
                                                                                        if (setAll == true)
                                                                                        {
                                                                                            colElmAttr = "disabled";
                                                                                            colElmAttrAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (roleid != " ")
                                                                                            {
                                                                                                if (colElement.Permissions != null && colElement.Permissions.Count > 0)
                                                                                                {

                                                                                                    foreach (var permission in colElement.Permissions)
                                                                                                    {
                                                                                                        if (permission.RoleId == roleid)
                                                                                                        {
                                                                                                            if (permission.Read == true && permission.Write == false)
                                                                                                            {
                                                                                                                colElmAttr = "disabled";
                                                                                                                colElmAttrAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                                                            }
                                                                                                            else if (permission.Read == false && permission.Write == false)
                                                                                                            {
                                                                                                                colElmAttr = "hidden";
                                                                                                                colElmAttrAlt = "hidden";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                colElmAttr = "";
                                                                                                                colElmAttrAlt = "";
                                                                                                            }
                                                                                                        }

                                                                                                    }
                                                                                                    if (colElmAttr == "data")
                                                                                                    {
                                                                                                        colElmAttr = "hidden";
                                                                                                        colElmAttrAlt = "hidden";
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                colElmAttr = "";
                                                                                                colElmAttrAlt = "";
                                                                                            }
                                                                                        }
                                                                                        // if (!EleVisible) { priviledge = "hidden"; priviledgeAlt = "hidden"; }

                                                                                        //till here

                                                                                        string colElementStr = string.Empty;

                                                                                        var colLabel = (this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel;
                                                                                        colElementStr = "<label for='elm" + colElement.ElementId + "' " + colElmAttr + ">" + colLabel + "</label>";
                                                                                        if (colElmAttr != "hidden")
                                                                                        {
                                                                                            colElementStr = colElementStr + colElement.Template + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + colElement.ElementId + "[0]\" data-valmsg-replace=\"true\"></span></div>";

                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            colElementStr = colElementStr + colElement.Template;
                                                                                        }
                                                                                        string colAttr = string.Empty;



                                                                                        if (this.Side == "frontend")
                                                                                        {
                                                                                            colAttr = this.GenerateValidationData(colElement) + colElmAttr + " name=\"elm" + colElement.ElementId + "[0]\" id=\"elm" + colElement.ElementId + "[0]\" class=\"valid " + colElement.FrontendClass + "\"";
                                                                                            colAttr = colAttr + " data-elm-type='" + colElement.GetType().Name.ToLower() + "'";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            colAttr = this.GenerateValidationData(colElement) + colElmAttr + " name =\"elm" + colElement.ElementId + "[0]\" id=\"elm" + colElement.ElementId + "[0]\" class=\"valid " + colElement.BackendClass + "\"";
                                                                                            colAttr = colAttr + " data-elm-type='" + colElement.GetType().Name.ToLower() + "'";
                                                                                        }

                                                                                        string colElms = "";
                                                                                        colAttr = colAttr + " data-name=\"" + colElement.Name + "\"";
                                                                                        colAttr = colAttr + " data-table-elm =\"true\" data-repeat-name =\"elm" + colElement.ElementId + "[{0}]\"";
                                                                                        colAttr = colAttr + showHideElement;
                                                                                        switch (colElement.GetType().Name.ToLower())
                                                                                        {
                                                                                            #region label
                                                                                            case "label":
                                                                                                colElementStr = colElement.Template;
                                                                                                labelStyle = ((this.Side == "frontend") ? colElement.FrontendLabelStyle : colElement.BackendLabelStyle);

                                                                                                labelFontSize = ((this.Side == "frontend") ? colElement.FrontendLabelFontSize : colElement.BackendLabelFontSize);
                                                                                                iconFontSize = ((this.Side == "frontend") ? colElement.FrontendIconFontSize : colElement.BackendIconFontSize);

                                                                                                labelFontHtml = string.Empty;
                                                                                                iconFontHtml = string.Empty;

                                                                                                if (labelFontSize != null)
                                                                                                {
                                                                                                    labelFontHtml = "style=font-size:" + labelFontSize + "px;";
                                                                                                }

                                                                                                if (iconFontSize != null)
                                                                                                {
                                                                                                    iconFontHtml = "style=font-size:" + iconFontSize + "px;";
                                                                                                }

                                                                                                if (labelStyle != null)
                                                                                                {
                                                                                                    switch (labelStyle)
                                                                                                    {
                                                                                                        case "1":
                                                                                                            colElement.FrontendLabel = "<b>" + colElement.FrontendLabel + "</b>";
                                                                                                            colElement.BackendLabel = "<b>" + colElement.BackendLabel + "</b>";
                                                                                                            break;

                                                                                                        case "2":
                                                                                                            colElement.FrontendLabel = "<i>" + colElement.FrontendLabel + "</i>";
                                                                                                            colElement.BackendLabel = "<i>" + colElement.BackendLabel + "</i>";
                                                                                                            break;

                                                                                                        case "3":
                                                                                                            colElement.FrontendLabel = "<u>" + colElement.FrontendLabel + "</u>";
                                                                                                            colElement.BackendLabel = "<u>" + colElement.BackendLabel + "</u>";
                                                                                                            break;

                                                                                                        case "1,2":
                                                                                                            colElement.FrontendLabel = "<b><i>" + colElement.FrontendLabel + "</i></b>";
                                                                                                            colElement.BackendLabel = "<b><i>" + colElement.BackendLabel + "</i></b>";
                                                                                                            break;

                                                                                                        case "1,3":
                                                                                                            colElement.FrontendLabel = "<b><u>" + colElement.FrontendLabel + "</u></b>";
                                                                                                            colElement.BackendLabel = "<b><u>" + colElement.BackendLabel + "</u></b>";
                                                                                                            break;

                                                                                                        case "1,2,3":
                                                                                                            colElement.FrontendLabel = "<b><i><u>" + colElement.FrontendLabel + "</b></i></u>";
                                                                                                            colElement.BackendLabel = "<b><i><u>" + colElement.BackendLabel + "</b></i></u>";
                                                                                                            break;

                                                                                                        case "2,3":
                                                                                                            colElement.FrontendLabel = "<i><u>" + colElement.FrontendLabel + "</u></i>";
                                                                                                            colElement.BackendLabel = "<i><u>" + colElement.BackendLabel + "</u></i>";
                                                                                                            break;
                                                                                                    }
                                                                                                }

                                                                                                colElms = string.Format(colElementStr, ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass), ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel), labelFontHtml, colAttr);
                                                                                                imageVisibility = ((this.Side == "frontend") ? colElement.FrontendImageVisibility : colElement.BackendImageVisibility);
                                                                                                iconVisibility = ((this.Side == "frontend") ? colElement.FrontendIconVisibility : colElement.BackendIconVisibility);
                                                                                                labelVisibility = ((this.Side == "frontend") ? colElement.FrontendLabelVisibility : colElement.BackendLabelVisibility);
                                                                                                if (labelVisibility)
                                                                                                {
                                                                                                    if (iconVisibility)
                                                                                                    {
                                                                                                        colElementStr = "<div class=\"{0}\" {5}><div class='label-icon' {4}><i class=\"{2}\"></i></div><div class='label-text' {3}>{1}</div></div>";
                                                                                                        colElms = string.Format(colElementStr, ((this.Side == "frontend") ? colElement.FrontendClass + " has-icon" : colElement.BackendClass + " has-icon"), ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel), ((this.Side == "frontend") ? colElement.FrontendIcon : colElement.BackendIcon), labelFontHtml, iconFontHtml, attr);
                                                                                                    }

                                                                                                    if (imageVisibility)
                                                                                                    {
                                                                                                        string imageHeight = ((this.Side == "frontend") ? colElement.FrontendImageHeight : colElement.BackendImageHeight);
                                                                                                        string imageWidth = ((this.Side == "frontend") ? colElement.FrontendImageWidth : colElement.BackendImageWidth);

                                                                                                        var imgAttr = "height= '" + imageHeight + "' width='" + imageWidth + "'";
                                                                                                        elementstr = "<div class=\"{0}\" {5}><div class='label-image' {4}><img src=\"/images/{2}\" " + imgAttr + "/></div><div class='label-text' {3}>{1}</div></div>";
                                                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? colElement.FrontendClass + " has-image" : colElement.BackendClass + " has-image"), ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel), ((this.Side == "frontend") ? colElement.FrontendImage : colElement.BackendImage), labelFontHtml, iconFontHtml, attr);
                                                                                                    }
                                                                                                }
                                                                                                else if (iconVisibility)
                                                                                                {
                                                                                                    colElementStr = "<div class=\"{0}\" {3}><div class='label-icon' {2}><i class=\"{1}\"></i><div></div>";
                                                                                                    colElms = string.Format(colElementStr, ((this.Side == "frontend") ? colElement.FrontendClass + " has-icon" : colElement.BackendClass + " has-icon"), ((this.Side == "frontend") ? colElement.FrontendIcon : colElement.BackendIcon), iconFontHtml, colAttr);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (imageVisibility)
                                                                                                    {
                                                                                                        string imageHeight = ((this.Side == "frontend") ? colElement.FrontendImageHeight : colElement.BackendImageHeight);
                                                                                                        string imageWidth = ((this.Side == "frontend") ? colElement.FrontendImageWidth : colElement.BackendImageWidth);

                                                                                                        var imgAttr = "height= '" + imageHeight + "' width='" + imageWidth + "'";
                                                                                                        elementstr = "<div class=\"{0}\" {3}><div class='label-image' {2}><img src=\"/images/{1}\" " + imgAttr + "/></div></div>";
                                                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? colElement.FrontendClass + " has-image" : colElement.BackendClass + " has-image"), ((this.Side == "frontend") ? colElement.FrontendImage : colElement.BackendImage), iconFontHtml, attr);
                                                                                                    }
                                                                                                }

                                                                                                break;

                                                                                            #endregion
                                                                                            #region Button
                                                                                            case "button":
                                                                                                buttonIconVisibility = ((this.Side == "frontend") ? colElement.FrontendIconVisibility : colElement.BackendIconVisibility);
                                                                                                dataAttrClickEvent = string.Empty;

                                                                                                if (colElement.IsOnClickEvent)
                                                                                                {
                                                                                                    dataAttrClickEvent = "data-work-onclick='true' data-action-onclick='onClick'";
                                                                                                    if (colElement.OnClickEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnClickEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnClickEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnClickEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnClickEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrClickEvent = dataAttrClickEvent + " data-val-onclick-parname='" + t + "' data-val-onclick-parelm='" + t1 + "' data-val-onclick-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (ElementBody.OnClickEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnClickEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnClickEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnClickEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnClickEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnClickEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        dataAttrClickEvent = dataAttrClickEvent + " data-val-onclick-resname='" + r + "' data-val-onclick-reselm='" + r1 + "' data-val-onclick-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrClickEvent = dataAttrClickEvent + " data-work-onclick-start='" + colElement.OnClickEvent.StartState + "' data-work-onclick-end='" + colElement.OnClickEvent.EndState + "' data-work-onclick-formid='" + formid + "'";
                                                                                                }

                                                                                                saveFormEvent = ElementBody.IsOnSaveFormEvent ? " data-saveform=true " : "data-saveform=false ";
                                                                                                isOnSwitchTabEvent = ElementBody.IsOnSwitchTabEvent;
                                                                                                switchTab = string.Empty;
                                                                                                if (isOnSwitchTabEvent)
                                                                                                {
                                                                                                    switchTab = " data-switchtab='" + colElement.TabId + "' ";
                                                                                                }

                                                                                                dataAttrClickEvent = dataAttrClickEvent + saveFormEvent + switchTab;

                                                                                                buttonIconHtml = string.Empty;
                                                                                                if (buttonIconVisibility)
                                                                                                {
                                                                                                    buttonIconHtml = "<i class='" + ((this.Side == "frontend") ? colElement.FrontendIcon : colElement.BackendIcon) + "'></i>";
                                                                                                }

                                                                                                buttontarget = "";

                                                                                                buttontargets = (List<Button.TargetOption>)colElement.TargetOptions;
                                                                                                if (buttontargets != null)
                                                                                                {
                                                                                                    var result = buttontargets;

                                                                                                    string toption = "";
                                                                                                    string taction = "";

                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        if (result[0] == targetoption)
                                                                                                        {
                                                                                                            toption = "#" + targetoption.TargetId;

                                                                                                            taction = targetoption.ShowHide;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            toption = toption + ",#" + targetoption.TargetId;

                                                                                                            taction = taction + "," + targetoption.ShowHide;
                                                                                                        }

                                                                                                    }

                                                                                                    if (!string.IsNullOrWhiteSpace(toption))
                                                                                                    {
                                                                                                        buttontarget = "data-target='" + toption + "' data-target-action='" + taction + "' data-action-click=onClick";
                                                                                                    }
                                                                                                }
                                                                                                responseTarget = "";
                                                                                                responseTargetValue = string.Empty;
                                                                                                if (colElement.IsOnResponseTarget)
                                                                                                {
                                                                                                    responseTarget = "data-isresponse-target='true'";
                                                                                                    responseTargetValue = JsonConvert.SerializeObject(colElement.TargetSettingsOnResponse);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    responseTarget = "data-isresponse-target='false'";
                                                                                                }
                                                                                                popup = string.Empty;
                                                                                                if (colElement.EnablePopup)
                                                                                                {
                                                                                                    string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                    popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                    if (colElement.DisablePopupClose)
                                                                                                    {
                                                                                                        popup = popup + " data-popup-close-disable='true'";
                                                                                                    }
                                                                                                    popup = popup + " data-popup-enable='true'";
                                                                                                    if (colElement.PopUpTarget != null && colElement.PopUpTarget.Count > 0)
                                                                                                    {
                                                                                                        string t = string.Empty;

                                                                                                        for (int i = 0; i < colElement.PopUpTarget.Count; i++)
                                                                                                        {
                                                                                                            if (colElement.PopUpTarget[i] != "" && colElement.PopUpTarget[i] != null)
                                                                                                            {
                                                                                                                t = t + "," + colElement.PopUpTarget[i];
                                                                                                            }

                                                                                                        }
                                                                                                        if (t != string.Empty)
                                                                                                        {
                                                                                                            t = t.Substring(1);
                                                                                                            popup = popup + " data-popup-target='" + t + "'";
                                                                                                        }

                                                                                                    }
                                                                                                    popup = popup + " data-popup-title='" + colElement.PopUpTitle + "'";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    popup = " data-popup-enable='false'";
                                                                                                }

                                                                                                colElementStr = colElement.Template;

                                                                                                attrs = " " + buttontarget + " " + popup + " " + dataAttrClickEvent + responseTarget + " class='" + ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass) + "' " + colAttr;
                                                                                                //elementstr = "<button type='button' " + buttontarget + " " + popup + " " + dataAttrClickEvent + responseTarget + " class='" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "' " + attr + " >" + ((this.Side == "frontend") ? (buttonIconHtml + " " + ElementBody.FrontendLabel) : (buttonIconHtml + " " + ElementBody.BackendLabel)) + "</button>"
                                                                                                //    + "<input type='hidden' data-target-setting-for='elm" + ElementBody.ElementId + "' value='" + responseTargetValue + "'/>";

                                                                                                colElms = string.Format(colElementStr, attrs, ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel));
                                                                                                colElms = colElms + "<input type='hidden' data-target-setting-for='elm" + colElement.ElementId + "' value='" + responseTargetValue + "'/>";
                                                                                                break;

                                                                                            #endregion
                                                                                            #region recaptcha
                                                                                            case "recaptcha":
                                                                                                if (this.Side == "frontend")
                                                                                                {
                                                                                                    colAttr = this.GenerateValidationData(colElement) + colElmAttr + " name=\"recaptcha" + colElement.ElementId + "\" id=\"recaptcha" + colElement.ElementId + "\" class=\"valid " + colElement.FrontendClass + "\"";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    colAttr = this.GenerateValidationData(colElement) + colElmAttr + " name =\"elm" + colElement.ElementId + "\" id=\"elm" + colElement.ElementId + "\" class=\"valid " + colElement.BackendClass + "\"";

                                                                                                }
                                                                                                colElementStr = colElement.Template;
                                                                                                className = ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass);
                                                                                                if (className == null)
                                                                                                {
                                                                                                    className = string.Empty;
                                                                                                }
                                                                                                dataSite = "data-site=" + colElement.SiteKey + " " + "data-callback=verifyCallback";
                                                                                                rnd = new Random();
                                                                                                id = "id=" + ((this.Side == "frontend") ? colElement.FrontendId + rnd.Next().ToString() : colElement.BackendId + rnd.Next().ToString());
                                                                                                colElms = string.Format(colElementStr, className, dataSite, attr, id);
                                                                                                break;

                                                                                            #endregion
                                                                                            #region hyperlink
                                                                                            case "hyperlink":
                                                                                                hyperlinkIconVisibility = ((this.Side == "frontend") ? colElement.FrontendIconVisibility : colElement.BackendIconVisibility);
                                                                                                popup = string.Empty;
                                                                                                hyperlinkIconHtml = string.Empty;
                                                                                                if (hyperlinkIconVisibility)
                                                                                                {
                                                                                                    hyperlinkIconHtml = "<i class='" + ((this.Side == "frontend") ? colElement.FrontendIcon : colElement.BackendIcon) + "'></i>";
                                                                                                }

                                                                                                href = ((this.Side == "frontend") ? colElement.FrontendHref : colElement.BackendHref);
                                                                                                if (href == null || href == "")
                                                                                                {
                                                                                                    href = "javascript::";
                                                                                                }
                                                                                                _target = ((this.Side == "frontend") ? colElement.FrontendTarget : colElement.BackendTarget);
                                                                                                if (colElement.EnablePopup)
                                                                                                {
                                                                                                    string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                    popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                    if (colElement.DisablePopupClose)
                                                                                                    {
                                                                                                        popup = popup + " data-popup-close-disable='true'";
                                                                                                    }
                                                                                                    popup = popup + " data-popup-enable='true'";
                                                                                                    if (colElement.PopUpTarget != null && colElement.PopUpTarget.Count > 0)
                                                                                                    {
                                                                                                        string t = string.Empty;

                                                                                                        for (int i = 0; i < colElement.PopUpTarget.Count; i++)
                                                                                                        {
                                                                                                            if (colElement.PopUpTarget[i] != "" && colElement.PopUpTarget[i] != null)
                                                                                                            {
                                                                                                                t = t + "," + colElement.PopUpTarget[i];
                                                                                                            }

                                                                                                        }
                                                                                                        if (t != string.Empty)
                                                                                                        {
                                                                                                            t = t.Substring(1);
                                                                                                            popup = popup + " data-popup-target='" + t + "'";
                                                                                                        }

                                                                                                    }
                                                                                                    popup = popup + " data-popup-title='" + colElement.PopUpTitle + "'";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    popup = " data-popup-enable='false'";
                                                                                                }
                                                                                                colElementStr = colElement.Template;
                                                                                                attributes = " " + "' target='" + _target + "'" + popup + " class='" + ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass) + "' " + colAttr + " name =elm" + colElement.ElementId + "[] id=elm" + colElement.ElementId + "[]";

                                                                                                colElms = string.Format(colElementStr, href, attributes, ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel));
                                                                                                //elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), "", attr);
                                                                                                break;

                                                                                            #endregion
                                                                                            #region paragraph
                                                                                            case "paragraph":
                                                                                                colElementStr = colElement.Template;
                                                                                                colElms = string.Format(colElementStr, ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass), colElement.ParagraphText, colAttr);
                                                                                                break;
                                                                                            #endregion
                                                                                            #region TextArea
                                                                                            case "textarea":
                                                                                                dataAttrKeyUpEvent = string.Empty;

                                                                                                if (colElement.IsOnKeyUpEvent)
                                                                                                {
                                                                                                    dataAttrKeyUpEvent = "data-work-onkeyup='true' data-action-onkeyup='onKeyUp'";
                                                                                                    if (colElement.OnKeyUpEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnKeyUpEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnKeyUpEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnKeyUpEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnKeyUpEvent.TakeValueFromElementValidation[i];

                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-parname='" + t + "' data-val-onkeyup-parelm='" + t1 + "' data-val-onkeyup-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnKeyUpEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnKeyUpEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnKeyUpEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnKeyUpEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnKeyUpEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnKeyUpEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-resname='" + r + "' data-val-onkeyup-reselm='" + r1 + "' data-val-onkeyup-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-work-onkeyup-start='" + colElement.OnKeyUpEvent.StartState + "' data-work-onkeyup-end='" + colElement.OnKeyUpEvent.EndState + "' data-work-onkeyup-formid='" + formid + "'";
                                                                                                }

                                                                                                colElms = string.Format(colElementStr, ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass), "", colAttr + " " + dataAttrKeyUpEvent);
                                                                                                break;
                                                                                            #endregion
                                                                                            #region Heading
                                                                                            case "heading":
                                                                                                colElms = "<" + colElement.HeaderType + " class = '" + ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass) + "' " + colAttr + " " + colElement.Attribute + "> " + colElement.HeaderText + " </" + colElement.HeaderType + ">";
                                                                                                break;
                                                                                            #endregion
                                                                                            #region TextBox
                                                                                            case "textbox":
                                                                                                txtfrm = "";
                                                                                                tel = "";
                                                                                                txtformatter = (this.Side == "frontend") ? colElement.FrontendFormatter : colElement.BackendFormatter;
                                                                                                if (txtformatter != "" && txtformatter != null)
                                                                                                {
                                                                                                    txtfrm = " data-inputmask=\"" + txtformatter + "\"";
                                                                                                }
                                                                                                if (colElement.IsTelephoneNumber == true)
                                                                                                {
                                                                                                    tel = " data-inputmask='\"mask\":\"" + colElement.TelephoneNumberFormat + "\"'";

                                                                                                }

                                                                                                dataAttrKeyUpEvent = string.Empty;
                                                                                                if (colElement.IsOnKeyUpEvent)
                                                                                                {
                                                                                                    dataAttrKeyUpEvent = "data-work-onkeyup='true' data-action-onkeyup='onKeyUp'";
                                                                                                    if (colElement.OnKeyUpEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnKeyUpEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnKeyUpEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnKeyUpEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnKeyUpEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-parname='" + t + "' data-val-onkeyup-parelm='" + t1 + "' data-val-onkeyup-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnKeyUpEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnKeyUpEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnKeyUpEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnKeyUpEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnKeyUpEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnKeyUpEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-resname='" + r + "' data-val-onkeyup-reselm='" + r1 + "' data-val-onkeyup-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-work-onkeyup-start='" + colElement.OnKeyUpEvent.StartState + "' data-work-onkeyup-end='" + colElement.OnKeyUpEvent.EndState + "' data-work-onkeyup-formid='" + formid + "'";
                                                                                                }

                                                                                                radioGroups = ((this.Side == "frontend") ? colElement.RadioGroup : colElement.RadioGroup);
                                                                                                rgAttr = "";
                                                                                                if (radioGroups != null)
                                                                                                {
                                                                                                    rgAttr = " data-element-radio-ids=" + radioGroups;
                                                                                                }
                                                                                                colElms = string.Format(colElementStr, colAttr + rgAttr + txtfrm + tel + " " + dataAttrKeyUpEvent);
                                                                                                break;
                                                                                            #endregion
                                                                                            #region Number
                                                                                            case "number":

                                                                                                nmfrm = "";
                                                                                                nmformatter = (this.Side == "frontend") ? colElement.FrontendFormatter : colElement.BackendFormatter;
                                                                                                ntarget = "";
                                                                                                defaultValue = "";
                                                                                                if (colElement.DefaultValue != null)
                                                                                                {
                                                                                                    defaultValue = " value='" + colElement.DefaultValue + "'";
                                                                                                }
                                                                                                if (nmformatter != "")
                                                                                                {
                                                                                                    nmfrm = " data-types=\"" + nmformatter + "\"";

                                                                                                }

                                                                                                dataAttrKeyUpEvent = string.Empty;
                                                                                                if (colElement.IsOnKeyUpEvent)
                                                                                                {
                                                                                                    dataAttrKeyUpEvent = "data-work-onkeyup='true' data-action-onkeyup='onKeyUp'";
                                                                                                    if (colElement.OnKeyUpEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnKeyUpEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnKeyUpEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnKeyUpEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnKeyUpEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-parname='" + t + "' data-val-onkeyup-parelm='" + t1 + "' data-val-onkeyup-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnKeyUpEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnKeyUpEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnKeyUpEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnKeyUpEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnKeyUpEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnKeyUpEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-val-onkeyup-resname='" + r + "' data-val-onkeyup-reselm='" + r1 + "' data-val-onkeyup-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrKeyUpEvent = dataAttrKeyUpEvent + " data-work-onkeyup-start='" + colElement.OnKeyUpEvent.StartState + "' data-work-onkeyup-end='" + colElement.OnKeyUpEvent.EndState + "' data-work-onkeyup-formid='" + formid + "'";
                                                                                                }
                                                                                                tempaa = "";
                                                                                                if (colElement.IsCurrency == true)
                                                                                                {
                                                                                                    string decimalDigit = string.Empty;
                                                                                                    if (colElement.DecimalDigit != null)
                                                                                                    {
                                                                                                        decimalDigit = colElement.DecimalDigit;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        decimalDigit = "2";
                                                                                                    }
                                                                                                    label = (this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel;
                                                                                                    colElementStr = "<label for='elm" + colElement.ElementId + "'" + colElmAttr + ">" + label + "</label>" + colElement.CurrencyTemplate;
                                                                                                    tempaa += "<input name = 'elm" + colElement.ElementId + "[0]' " + defaultValue + " " + colElmAttrAlt + colAttr + this.GenerateValidationData(colElement) + " type= 'number' hidden  data-value-for='currency_" + colElement.ElementId + "[0]' " + dataAttrKeyUpEvent + " class='custom-control-input' id='elm" + colElement.ElementId + "[0]' data-repeat-id='elm" + colElement.ElementId + "[{0}]' data-repeat-name='elm" + colElement.ElementId + "[{0}]'  data-repeat-value-for='currency_" + colElement.ElementId + "[{0}]'/>";
                                                                                                    colAttr = colAttr + "data-repeat-name =\"currency_" + colElement.ElementId + "[{0}]\"";
                                                                                                    tempaa += "<input data-elm-type='currency' data-repeat-name='currency_" + colElement.ElementId + "[{0}]' name='currency_" + colElement.ElementId + "[0]' " + defaultValue + " id='currency_" + colElement.ElementId + "[0]' type=\"text\" " + colElmAttr + " " + colAttr + " " + dataAttrKeyUpEvent + " data-repeat-id='currency_" + colElement.ElementId + "[{0}]' class='form-control' data-inputmask=\"'alias': 'numeric', 'groupSeparator': ',', 'digits':" + decimalDigit + ", 'digitsOptional': false, 'prefix': '" + colElement.CurrencyType + " ', 'placeholder': '0'\" inputmode=\"numeric\" style=\"text-align: right;\"/>";
                                                                                                    tempaa += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + colElement.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                                                    colElms = string.Format(colElementStr, tempaa, "");
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    List<Number.TargetOption> ntargets = (List<Number.TargetOption>)colElement.TargetOptions;
                                                                                                    if (ntargets != null)
                                                                                                    {
                                                                                                        foreach (var targetoption in ntargets)
                                                                                                        {
                                                                                                            if (targetoption.TargetId != "0")
                                                                                                            {
                                                                                                                if (ntargets[0] == targetoption)
                                                                                                                {
                                                                                                                    ntarget = "#" + targetoption.TargetId;

                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    ntarget = ntarget + ",#" + targetoption.TargetId;

                                                                                                                }
                                                                                                            }

                                                                                                        }
                                                                                                    }

                                                                                                    if (!string.IsNullOrWhiteSpace(ntarget))
                                                                                                    {
                                                                                                        ntarget = " data-target='" + ntarget + "' number-target";
                                                                                                    }

                                                                                                    colElms = string.Format(colElementStr, colAttr + ntarget + defaultValue + nmfrm + " " + dataAttrKeyUpEvent);
                                                                                                }

                                                                                                break;
                                                                                            #endregion
                                                                                            #region SelectBox
                                                                                            case "selectbox":
                                                                                                seloptions = "";
                                                                                                selected = string.Empty;
                                                                                                dataAttrChangeEvent = string.Empty;
                                                                                                dataAttrLoadEvent = string.Empty;
                                                                                                if (colElement.IsOnChangeEvent)
                                                                                                {
                                                                                                    dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                                                    if (colElement.OnChangeEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnChangeEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnChangeEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnChangeEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnChangeEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnChangeEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + colElement.OnChangeEvent.StartState + "' data-work-onchange-end='" + colElement.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                                                }


                                                                                                if (colElement.IsOnLoadEvent)
                                                                                                {
                                                                                                    dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                                                    if (colElement.OnLoadEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnLoadEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnLoadEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnLoadEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnLoadEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnLoadEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + colElement.OnLoadEvent.StartState + "' data-work-onload-end='" + colElement.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                                                }

                                                                                                if (colElement.SelectOptions != null && colElement.SelectOptions.Count > 0)
                                                                                                {

                                                                                                    if ((this.Side == "frontend" && colElement.FrontendClass != null && colElement.FrontendClass.ToUpper().Contains("IMAGE")) || (this.Side == "backend" && colElement.BackendClass != null && colElement.BackendClass.ToUpper().Contains("IMAGE")))
                                                                                                    {
                                                                                                        label = (this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel;
                                                                                                        colElementStr = "<label for='elm" + colElement.ElementId + "' " + priviledgeAlt + ">" + label + "</label>" + colElement.AltTemplate;
                                                                                                        var altTemplate = (colElement.AltTemplate.Contains("exchange-type") ? true : false);
                                                                                                        var count = 0;
                                                                                                        //elementstr = ElementBody.AltTemplate;
                                                                                                        string value = string.Empty;
                                                                                                        selected = string.Empty;
                                                                                                        string toggle = string.Empty;

                                                                                                        foreach (var soption in colElement.SelectOptions)
                                                                                                        {
                                                                                                            if (soption.Selected == true)
                                                                                                            {
                                                                                                                selected = " show ";
                                                                                                                value = soption.Value;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                selected = "";
                                                                                                            }

                                                                                                            if (soption.ToggleOptions == true)
                                                                                                            {
                                                                                                                toggle = "toggle-options='true'";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                toggle = "toggle-options='false'";
                                                                                                            }
                                                                                                            string target = "";

                                                                                                            List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)colElement.TargetOptions;
                                                                                                            if (targets != null)
                                                                                                            {
                                                                                                                var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                                                string toption = "";
                                                                                                                string taction = "";

                                                                                                                foreach (var targetoption in result)
                                                                                                                {
                                                                                                                    if (result[0] == targetoption)
                                                                                                                    {
                                                                                                                        toption = "#" + targetoption.TargetId;

                                                                                                                        taction = targetoption.ShowHide;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        toption = toption + ",#" + targetoption.TargetId;

                                                                                                                        taction = taction + "," + targetoption.ShowHide;
                                                                                                                    }

                                                                                                                }

                                                                                                                if (!string.IsNullOrWhiteSpace(toption))
                                                                                                                {
                                                                                                                    target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                                }
                                                                                                            }

                                                                                                            count++;
                                                                                                            var prClass = string.Empty;
                                                                                                            prClass = "mb-3";

                                                                                                            if (altTemplate)
                                                                                                            {
                                                                                                                if (count == 1)
                                                                                                                {
                                                                                                                    prClass = "pr-md-0";
                                                                                                                }
                                                                                                                else if (count == colElement.SelectOptions.Count)
                                                                                                                {
                                                                                                                    prClass = "pl-md-0";
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    prClass = "p-md-0";
                                                                                                                }
                                                                                                            }
                                                                                                            List<SelectBox.PopUpTargetOption> popUpTargetOptions = (List<SelectBox.PopUpTargetOption>)colElement.PopUpTarget;
                                                                                                            popup = string.Empty;
                                                                                                            if (colElement.EnablePopup)
                                                                                                            {
                                                                                                                string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                                popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                                if (colElement.DisablePopupClose)
                                                                                                                {
                                                                                                                    popup = popup + " data-popup-close-disable='true'";
                                                                                                                }
                                                                                                                if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                                                {
                                                                                                                    var result = popUpTargetOptions.Where(x => x.SelectId == soption.Value).ToList();
                                                                                                                    foreach (SelectBox.PopUpTargetOption popTarget in result)
                                                                                                                    {
                                                                                                                        if ((bool)popTarget.EnablePopup)
                                                                                                                        {
                                                                                                                            popup = popup + " data-popup-enable='true'";
                                                                                                                            string t = string.Empty;


                                                                                                                            if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                                            {
                                                                                                                                t = t + "," + popTarget.TargetId;
                                                                                                                            }

                                                                                                                            if (t != string.Empty)
                                                                                                                            {
                                                                                                                                t = t.Substring(1);
                                                                                                                                popup = popup + " data-popup-target='" + t + "'";

                                                                                                                                popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                                            }

                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            popup = " data-popup-enable='false'";
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }

                                                                                                            seloptions = seloptions + "<div " + target + " class=\"  col-md-3 " + prClass + "\"><a onclick='setValueOn(\"elm" + colElement.ElementId + "\",\"" + soption.Value + "\")' class='Image-target module-tab" + selected + "' value ='" + soption.Value + "' " + selected + " " + target + " " + popup + " " + toggle + "name=''><i class=\"module-icon module-icon--check\"></i><div class='module-type module-type--img d-flex justify-content-center align-items-center'><img src = '/images/" + soption.IconUrl + ".png' alt = '" + soption.IconUrl + "' ></div><p class='m-0'>" + soption.Text + "</p></a></div>";

                                                                                                        }
                                                                                                        seloptions += "<input name = 'elm" + colElement.ElementId + "[]' type= 'text-box'" + colElmAttrAlt + this.GenerateValidationData(colElement) + " value = '" + value + "' data-elm-select = 'elmSelect' class='custom-control-input' id='elm" + colElement.ElementId + "[]' " + dataAttrChangeEvent + " " + dataAttrLoadEvent + " " + colAttr + "/>";
                                                                                                        seloptions += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + colElement.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";
                                                                                                        colElms = string.Format(colElementStr, seloptions, " ", colElmAttrAlt + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent);
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (colElement.UseFromDatabase == false)
                                                                                                        {

                                                                                                            foreach (var soption in colElement.SelectOptions)
                                                                                                            {
                                                                                                                if (soption.Selected == true)
                                                                                                                {
                                                                                                                    selected = "selected";
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    selected = "";
                                                                                                                }

                                                                                                                string target = "";

                                                                                                                List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)colElement.TargetOptions;
                                                                                                                if (targets != null)
                                                                                                                {
                                                                                                                    var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                                                    string toption = "";
                                                                                                                    string taction = "";

                                                                                                                    foreach (var targetoption in result)
                                                                                                                    {
                                                                                                                        if (result[0] == targetoption)
                                                                                                                        {
                                                                                                                            toption = "#" + targetoption.TargetId;

                                                                                                                            taction = targetoption.ShowHide;
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            toption = toption + ",#" + targetoption.TargetId;

                                                                                                                            taction = taction + "," + targetoption.ShowHide;
                                                                                                                        }

                                                                                                                    }

                                                                                                                    if (!string.IsNullOrWhiteSpace(toption))
                                                                                                                    {
                                                                                                                        target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                                    }
                                                                                                                }
                                                                                                                List<SelectBox.PopUpTargetOption> popUpTargetOptions = (List<SelectBox.PopUpTargetOption>)colElement.PopUpTarget;
                                                                                                                popup = string.Empty;
                                                                                                                if (colElement.EnablePopup)
                                                                                                                {
                                                                                                                    string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                                    popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                                    if (colElement.DisablePopupClose)
                                                                                                                    {
                                                                                                                        popup = popup + " data-popup-close-disable='true'";
                                                                                                                    }
                                                                                                                    if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                                                    {
                                                                                                                        var result = popUpTargetOptions.Where(x => x.SelectId == soption.Value).ToList();
                                                                                                                        foreach (SelectBox.PopUpTargetOption popTarget in result)
                                                                                                                        {
                                                                                                                            if ((bool)popTarget.EnablePopup)
                                                                                                                            {
                                                                                                                                popup = popup + " data-popup-enable='true'";
                                                                                                                                string t = string.Empty;


                                                                                                                                if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                                                {
                                                                                                                                    t = t + "," + popTarget.TargetId;
                                                                                                                                }

                                                                                                                                if (t != string.Empty)
                                                                                                                                {
                                                                                                                                    t = t.Substring(1);
                                                                                                                                    popup = popup + " data-popup-target='" + t + "'";

                                                                                                                                    popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                                                }


                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                popup = " data-popup-enable='false'";
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }

                                                                                                                seloptions = seloptions + "<option name='" + colElement.ElementId + "-" + soption.Value + "' value='" + soption.Value + "' " + selected + " " + target + " " + popup + ">" + soption.Text + "</option>";

                                                                                                            }
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            string optionText = colElement.SelectFromDbFieldText;
                                                                                                            string optionValue = colElement.SelectFromDbFieldValue;
                                                                                                            string optionIcon = colElement.SelectFromDbIconUrl;
                                                                                                            string tableName = colElement.SelectFromDbTable;
                                                                                                            int selectedRow = colElement.SelectFromDbFieldSelectedOnRow;
                                                                                                            DataTable dataTable = new DataTable();
                                                                                                            var CommandText = "Select [" + optionText + "], [" + optionValue + "],[" + optionIcon + "] from " + tableName;
                                                                                                            if (this._db == null) this._db = this.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
                                                                                                            using (var command = _db.Database.GetDbConnection().CreateCommand())
                                                                                                            {
                                                                                                                command.CommandText = CommandText;
                                                                                                                _db.Database.OpenConnection();
                                                                                                                try
                                                                                                                {
                                                                                                                    SqlConnection connection = new SqlConnection(command.Connection.ConnectionString);
                                                                                                                    SqlCommand sql = new SqlCommand(command.CommandText, connection);
                                                                                                                    using (SqlDataAdapter result = new SqlDataAdapter(sql))
                                                                                                                    {
                                                                                                                        result.Fill(dataTable);
                                                                                                                    }
                                                                                                                }
                                                                                                                catch (Exception ex)
                                                                                                                {

                                                                                                                }

                                                                                                            }


                                                                                                            if (dataTable.Rows.Count > 0)
                                                                                                            {
                                                                                                                int p = 0;
                                                                                                                foreach (DataRow dRow in dataTable.Rows)
                                                                                                                {
                                                                                                                    p = p + 1;
                                                                                                                    if (Convert.ToInt16(colElement.SelectFromDbFieldSelectedOnRow) == p)
                                                                                                                    {
                                                                                                                        selected = "selected";
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        selected = "";
                                                                                                                    }

                                                                                                                    string target = "";

                                                                                                                    //List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                                                    //if (targets != null)
                                                                                                                    //{
                                                                                                                    //    var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                                                    //    string toption = "";
                                                                                                                    //    string taction = "";

                                                                                                                    //    foreach (var targetoption in result)
                                                                                                                    //    {
                                                                                                                    //        if (result[0] == targetoption)
                                                                                                                    //        {
                                                                                                                    //            toption = "#" + targetoption.TargetId;

                                                                                                                    //            taction = "" + targetoption.ShowHide + "";
                                                                                                                    //        }
                                                                                                                    //        else
                                                                                                                    //        {
                                                                                                                    //            toption = toption + ",#" + targetoption.TargetId;

                                                                                                                    //            taction = taction + "," + targetoption.ShowHide + "";
                                                                                                                    //        }

                                                                                                                    //    }

                                                                                                                    //    if (!string.IsNullOrWhiteSpace(toption))
                                                                                                                    //    {
                                                                                                                    //        target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                                    //    }
                                                                                                                    //}


                                                                                                                    seloptions = seloptions + "<option value='" + dRow[optionValue] + "' " + selected + " " + target + ">" + dRow[optionText] + "</option>";

                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        colElms = string.Format(colElementStr, colAttr + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent + " select-target", seloptions);
                                                                                                    }

                                                                                                }

                                                                                                break;
                                                                                            #endregion
                                                                                            #region MultiSelectBox
                                                                                            case "multiselectbox":
                                                                                                mulseloptions = "";
                                                                                                dataAttrChangeEvent = string.Empty;
                                                                                                dataAttrLoadEvent = string.Empty;
                                                                                                if (colElement.IsOnChangeEvent)
                                                                                                {
                                                                                                    dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                                                    if (colElement.OnChangeEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnChangeEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnChangeEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnChangeEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnChangeEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnChangeEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + colElement.OnChangeEvent.StartState + "' data-work-onchange-end='" + colElement.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                                                }


                                                                                                if (colElement.IsOnLoadEvent)
                                                                                                {
                                                                                                    dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                                                    if (colElement.OnLoadEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnLoadEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnLoadEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnLoadEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnLoadEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnLoadEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + colElement.OnLoadEvent.StartState + "' data-work-onload-end='" + colElement.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                                                }
                                                                                                if (colElement.SelectOptions != null && colElement.SelectOptions.Count > 0)
                                                                                                {

                                                                                                    selected = string.Empty;
                                                                                                    foreach (var msoption in colElement.SelectOptions)
                                                                                                    {
                                                                                                        if (msoption.Selected == true)
                                                                                                        {
                                                                                                            selected = "selected";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            selected = "";
                                                                                                        }

                                                                                                        mulseloptions = mulseloptions + "<option value='" + msoption.Value + "' " + selected + ">" + msoption.Text + "</option>";
                                                                                                    }
                                                                                                }

                                                                                                colElms = string.Format(colElementStr, colAttr + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent, mulseloptions);
                                                                                                break;
                                                                                            #endregion
                                                                                            #region RadioGroup
                                                                                            case "radiogroup":
                                                                                                radoptions = "";
                                                                                                dataAttrChangeEvent = string.Empty;
                                                                                                dataAttrLoadEvent = string.Empty;
                                                                                                if (colElement.IsOnChangeEvent)
                                                                                                {
                                                                                                    dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                                                    if (colElement.OnChangeEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnChangeEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnChangeEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnChangeEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnChangeEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnChangeEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + colElement.OnChangeEvent.StartState + "' data-work-onchange-end='" + colElement.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                                                }


                                                                                                if (colElement.IsOnLoadEvent)
                                                                                                {
                                                                                                    dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                                                    if (colElement.OnLoadEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnLoadEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnLoadEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnLoadEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnLoadEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnLoadEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + colElement.OnLoadEvent.StartState + "' data-work-onload-end='" + colElement.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                                                }

                                                                                                if (colElement.RadioOptions != null && colElement.RadioOptions.Count > 0)
                                                                                                {
                                                                                                    selected = string.Empty;
                                                                                                    int i = 0;
                                                                                                    if (((this.Side == "frontend" && colElement.FrontendClass != null && colElement.FrontendClass.ToUpper().Contains("ONOFF")) || (this.Side == "backend" && colElement.BackendClass != null && colElement.BackendClass.ToUpper().Contains("ONOFF"))) && colElement.RadioOptions.Count == 2)
                                                                                                    {
                                                                                                        string targetY = "";
                                                                                                        string targetN = "";
                                                                                                        string vals = "false";
                                                                                                        popup = string.Empty;
                                                                                                        foreach (var roption in colElement.RadioOptions)
                                                                                                        {
                                                                                                            if (roption.Value == "Y" || roption.Value == "Yes")
                                                                                                            {
                                                                                                                vals = "true";
                                                                                                                List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)colElement.TargetOptions;
                                                                                                                if (targets != null)
                                                                                                                {
                                                                                                                    var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                                    string toption = "";
                                                                                                                    string taction = "";

                                                                                                                    foreach (var targetoption in result)
                                                                                                                    {
                                                                                                                        if (result[0] == targetoption)
                                                                                                                        {
                                                                                                                            toption = "#" + targetoption.TargetId;

                                                                                                                            taction = targetoption.ShowHide;
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            toption = toption + ",#" + targetoption.TargetId;

                                                                                                                            taction = taction + "," + targetoption.ShowHide;
                                                                                                                        }

                                                                                                                    }

                                                                                                                    if (!string.IsNullOrWhiteSpace(toption))
                                                                                                                    {
                                                                                                                        targetY = "data-targety='" + toption + "' data-actiony='" + taction + "'";
                                                                                                                    }
                                                                                                                }
                                                                                                                List<RadioGroup.PopUpTargetOption> popUpTargetOptions = (List<RadioGroup.PopUpTargetOption>)colElement.PopUpTarget;

                                                                                                                if (colElement.EnablePopup)
                                                                                                                {
                                                                                                                    string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                                    popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                                    if (colElement.DisablePopupClose)
                                                                                                                    {
                                                                                                                        popup = popup + " data-popup-close-disable='true'";
                                                                                                                    }
                                                                                                                    if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                                                    {
                                                                                                                        var result = popUpTargetOptions.Where(x => x.SelectId == roption.Value).ToList();
                                                                                                                        foreach (RadioGroup.PopUpTargetOption popTarget in result)
                                                                                                                        {
                                                                                                                            if ((bool)popTarget.EnablePopup)
                                                                                                                            {
                                                                                                                                popup = popup + " data-popup-enable='true'";
                                                                                                                                string t = string.Empty;


                                                                                                                                if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                                                {
                                                                                                                                    t = t + "," + popTarget.TargetId;
                                                                                                                                }
                                                                                                                                if (t != string.Empty)
                                                                                                                                {
                                                                                                                                    t = t.Substring(1);
                                                                                                                                    popup = popup + " data-popup-target='" + t + "'";

                                                                                                                                    popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                                                }


                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                popup = " data-popup-enable='false'";
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                                if (roption.Selected)
                                                                                                                {
                                                                                                                    selected = "checked";
                                                                                                                }

                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)colElement.TargetOptions;
                                                                                                                if (targets != null)
                                                                                                                {
                                                                                                                    var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                                    string toption = "";
                                                                                                                    string taction = "";

                                                                                                                    foreach (var targetoption in result)
                                                                                                                    {
                                                                                                                        if (result[0] == targetoption)
                                                                                                                        {
                                                                                                                            toption = "#" + targetoption.TargetId;

                                                                                                                            taction = targetoption.ShowHide;
                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            toption = toption + ",#" + targetoption.TargetId;

                                                                                                                            taction = taction + "," + targetoption.ShowHide;
                                                                                                                        }

                                                                                                                    }
                                                                                                                    List<RadioGroup.PopUpTargetOption> popUpTargetOptions = (List<RadioGroup.PopUpTargetOption>)colElement.PopUpTarget;

                                                                                                                    if (!string.IsNullOrWhiteSpace(toption))
                                                                                                                    {
                                                                                                                        targetN = "data-targetn='" + toption + "' data-actionn='" + taction + "'";
                                                                                                                    }
                                                                                                                }
                                                                                                            }

                                                                                                            i++;
                                                                                                        }
                                                                                                        string v = "False";
                                                                                                        if (selected == "checked")
                                                                                                        {
                                                                                                            v = "True";
                                                                                                        }

                                                                                                        radoptions += "<input type='text' value=" + v + " name='elm" + colElement.ElementId + "[0]' data-repeat-name='elm" + colElement.ElementId + "[{0}]' hidden data-for-switch='check" + colElement.ElementId + "[0]' data-repeat-for-switch='check" + colElement.ElementId + "[{0}]'/> <div  class='custom-control custom-switch " + ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass) + " '> <input  " + targetY + " " + targetN + " data-toggle='slider' name='check" + colElement.ElementId + "[0]' data-repeat-name='check" + colElement.ElementId + "[{0}]' type='checkbox'" + colElmAttrAlt + " " + dataAttrChangeEvent + " " + popup + " " + dataAttrLoadEvent + this.GenerateValidationData(colElement) + " value='" + vals + "' class='custom-control-input radio-slider-target' id='" + colElement.ElementId + "[0]switch' data-repeat-id='" + colElement.ElementId + "[{0}]switch' " + selected + " data-for-check='elm" + colElement.ElementId + "[0]' data-repeat-for-check='elm" + colElement.ElementId + "[{0}]' />  <label class='custom-control-label' for='" + colElement.ElementId + "[0]switch' data-repeat-for='" + colElement.ElementId + "[{0}]switch'" + colElmAttrAlt + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent + ">  </label> </div>";

                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        foreach (var roption in colElement.RadioOptions)
                                                                                                        {
                                                                                                            if (roption.Selected == true)
                                                                                                            {
                                                                                                                selected = "checked";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                selected = "";
                                                                                                            }
                                                                                                            string target = "";

                                                                                                            List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)colElement.TargetOptions;
                                                                                                            if (targets != null)
                                                                                                            {
                                                                                                                var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                                string toption = "";
                                                                                                                string taction = "";

                                                                                                                foreach (var targetoption in result)
                                                                                                                {
                                                                                                                    if (result[0] == targetoption)
                                                                                                                    {
                                                                                                                        toption = "#" + targetoption.TargetId;

                                                                                                                        taction = targetoption.ShowHide;
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        toption = toption + ",#" + targetoption.TargetId;

                                                                                                                        taction = taction + "," + targetoption.ShowHide;
                                                                                                                    }

                                                                                                                }

                                                                                                                if (!string.IsNullOrWhiteSpace(toption))
                                                                                                                {
                                                                                                                    target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                                }
                                                                                                            }
                                                                                                            List<RadioGroup.PopUpTargetOption> popUpTargetOptions = (List<RadioGroup.PopUpTargetOption>)colElement.PopUpTarget;
                                                                                                            popup = string.Empty;
                                                                                                            if (colElement.EnablePopup)
                                                                                                            {
                                                                                                                string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                                popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                                if (colElement.DisablePopupClose)
                                                                                                                {
                                                                                                                    popup = popup + " data-popup-close-disable='true'";
                                                                                                                }
                                                                                                                if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                                                {
                                                                                                                    var result = popUpTargetOptions.Where(x => x.SelectId == roption.Value).ToList();
                                                                                                                    foreach (RadioGroup.PopUpTargetOption popTarget in result)
                                                                                                                    {
                                                                                                                        if ((bool)popTarget.EnablePopup)
                                                                                                                        {
                                                                                                                            popup = popup + " data-popup-enable='true'";
                                                                                                                            string t = string.Empty;


                                                                                                                            if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                                            {
                                                                                                                                t = t + "," + popTarget.TargetId;
                                                                                                                            }

                                                                                                                            if (t != string.Empty)
                                                                                                                            {
                                                                                                                                t = t.Substring(1);
                                                                                                                                popup = popup + " data-popup-target='" + t + "'";

                                                                                                                                popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                                            }


                                                                                                                        }
                                                                                                                        else
                                                                                                                        {
                                                                                                                            popup = " data-popup-enable='false'";
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            var hasIcon = string.Empty;
                                                                                                            hasIcon = ((roption.Icon == null || roption.Icon == "") ? "" : "has-icon");
                                                                                                            //radoptions += "<div class='custom-control custom-radio custom-control-inline'><input class='custom-control-input' type= 'radio' " + selected + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + (Utils.ConvertToString(roption.Value) == "" ? (ElementBody.ElementId + i) : roption.Value) + "'><label class='custom-control-label' for='elm" + roption.Value + "'>" + roption.Text + "</label></div>";
                                                                                                            radoptions += "<div " + target + " class='custom-control custom-radio custom-control-inline " + hasIcon + "'><input " + colElmAttrAlt + this.GenerateValidationData(colElement) + " type= 'radio' " + selected + " " + target + " " + popup + " value = '" + roption.Value + "' name = 'elm" + colElement.ElementId + "[0]' data-repeat-name='elm" + colElement.ElementId + "[{0}]' id='elm" + roption.Value + "[0]' data-repeat-id='elm" + roption.Value + "[{0}]'><i class='" + roption.Icon + "' for='elm" + roption.Value + "[0]' data-repeat-for='elm" + roption.Value + "[{0}]' " + colAttr + "></i><label class='custom-control-label' for='elm" + roption.Value + "[0]' data-repeat-for='elm" + roption.Value + "[{0}]'" + colElmAttrAlt + "> " + roption.Text + "</label></div>";
                                                                                                            i++;
                                                                                                        }
                                                                                                    }
                                                                                                }

                                                                                                colElms = "<div class='mb-2 '" + colElmAttrAlt + ">" + ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel) + "</div><div>" + radoptions + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + colElement.ElementId + "[]\" data-valmsg-replace=\"true\"></span></div></div>";
                                                                                                break;
                                                                                            #endregion
                                                                                            #region CheckBoxGroup
                                                                                            case "checkboxgroup":
                                                                                                isInline = (this.Side == "frontend") ? colElement.FrontendCheckboxStyle : colElement.BackendCheckboxStyle;
                                                                                                inlineClass = string.Empty;
                                                                                                if (isInline == "1")
                                                                                                {
                                                                                                    inlineClass = "custom-control-inline";
                                                                                                }
                                                                                                chkoptions = "";
                                                                                                dataAttrChangeEvent = string.Empty;
                                                                                                dataAttrLoadEvent = string.Empty;
                                                                                                if (colElement.IsOnChangeEvent)
                                                                                                {
                                                                                                    dataAttrChangeEvent = "data-work-onchange='true' data-action-onchange='onChange'";
                                                                                                    if (colElement.OnChangeEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnChangeEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnChangeEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnChangeEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-parname='" + t + "' data-val-onchange-parelm='" + t1 + "' data-val-onchange-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnChangeEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnChangeEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnChangeEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnChangeEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnChangeEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnChangeEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrChangeEvent = dataAttrChangeEvent + " data-val-onchange-resname='" + r + "' data-val-onchange-reselm='" + r1 + "' data-val-onchange-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrChangeEvent = dataAttrChangeEvent + " data-work-onchange-start='" + colElement.OnChangeEvent.StartState + "' data-work-onchange-end='" + colElement.OnChangeEvent.EndState + "' data-work-onchange-formid='" + formid + "'";
                                                                                                }


                                                                                                if (colElement.IsOnLoadEvent)
                                                                                                {
                                                                                                    dataAttrLoadEvent = "data-work-onload='true' data-action-onload='onLoad'";
                                                                                                    if (colElement.OnLoadEvent.TakeValueFromName != null)
                                                                                                    {
                                                                                                        string t = string.Empty;
                                                                                                        string t1 = string.Empty;
                                                                                                        string t2 = string.Empty;
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.TakeValueFromName.Count; i++)
                                                                                                        {
                                                                                                            t = t + "," + colElement.OnLoadEvent.TakeValueFromName[i];
                                                                                                            t1 = t1 + "," + colElement.OnLoadEvent.TakeValueFromElement[i];
                                                                                                            t2 = t2 + "," + colElement.OnLoadEvent.TakeValueFromElementValidation[i];
                                                                                                        }
                                                                                                        t = t.Substring(1);
                                                                                                        t1 = t1.Substring(1);
                                                                                                        t2 = t2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-parname='" + t + "' data-val-onload-parelm='" + t1 + "' data-val-onload-parname-validation='" + t2 + "'";
                                                                                                    }
                                                                                                    if (colElement.OnLoadEvent.SetValueToName != null)
                                                                                                    {
                                                                                                        string r = string.Empty;
                                                                                                        string r1 = string.Empty;
                                                                                                        string r2 = string.Empty;
                                                                                                        bool EventTriggerNull = false;
                                                                                                        if (colElement.OnLoadEvent.SetValueToEventTrigger == null)
                                                                                                        {
                                                                                                            EventTriggerNull = true;
                                                                                                        }
                                                                                                        for (int i = 0; i < colElement.OnLoadEvent.SetValueToName.Count; i++)
                                                                                                        {
                                                                                                            r = r + "," + colElement.OnLoadEvent.SetValueToName[i];
                                                                                                            r1 = r1 + "," + colElement.OnLoadEvent.SetValueToElement[i];
                                                                                                            r2 = r2 + "," + (!EventTriggerNull ? colElement.OnLoadEvent.SetValueToEventTrigger[i] : "True");
                                                                                                        }
                                                                                                        r = r.Substring(1);
                                                                                                        r1 = r1.Substring(1);
                                                                                                        r2 = r2.Substring(1);
                                                                                                        dataAttrLoadEvent = dataAttrLoadEvent + " data-val-onload-resname='" + r + "' data-val-onload-reselm='" + r1 + "' data-val-onload-restrigger='" + r2 + "'";
                                                                                                    }
                                                                                                    dataAttrLoadEvent = dataAttrLoadEvent + " data-work-onload-start='" + colElement.OnLoadEvent.StartState + "' data-work-onload-end='" + colElement.OnLoadEvent.EndState + "' data-work-onload-formid='" + formid + "'";
                                                                                                }

                                                                                                if (colElement.CheckOptions != null && colElement.CheckOptions.Count > 0)
                                                                                                {

                                                                                                    selected = string.Empty;
                                                                                                    foreach (var coption in colElement.CheckOptions)
                                                                                                    {
                                                                                                        var hasIcon = string.Empty;
                                                                                                        hasIcon = ((coption.Icon == null || coption.Icon == "") ? "" : "has-icon");
                                                                                                        if (coption.Selected == true)
                                                                                                        {
                                                                                                            selected = "checked";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            selected = "";
                                                                                                        }

                                                                                                        string target = "";

                                                                                                        List<CheckBoxGroup.TargetOption> targets = (List<CheckBoxGroup.TargetOption>)colElement.TargetOptions;
                                                                                                        if (targets != null)
                                                                                                        {
                                                                                                            var result = targets.Where(x => x.SelectId == coption.Value).ToList();

                                                                                                            string toption = "";
                                                                                                            string taction = "";

                                                                                                            foreach (var targetoption in result)
                                                                                                            {
                                                                                                                if (result[0] == targetoption)
                                                                                                                {
                                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                                    taction = targetoption.ShowHide;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                                }

                                                                                                            }

                                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                                            {
                                                                                                                target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                            }
                                                                                                        }
                                                                                                        List<CheckBoxGroup.PopUpTargetOption> popUpTargetOptions = (List<CheckBoxGroup.PopUpTargetOption>)colElement.PopUpTarget;
                                                                                                        popup = string.Empty;
                                                                                                        if (colElement.EnablePopup)
                                                                                                        {
                                                                                                            string popupClass = (colElement.PopUpClass == null) ? "modal-lg" : colElement.PopUpClass;
                                                                                                            popup = popup + " data-popup-class='" + popupClass + "'";
                                                                                                            if (colElement.DisablePopupClose)
                                                                                                            {
                                                                                                                popup = popup + " data-popup-close-disable='true'";
                                                                                                            }
                                                                                                            if (popUpTargetOptions != null && popUpTargetOptions.Count > 0)
                                                                                                            {
                                                                                                                var result = popUpTargetOptions.Where(x => x.SelectId == coption.Value).ToList();
                                                                                                                foreach (CheckBoxGroup.PopUpTargetOption popTarget in result)
                                                                                                                {
                                                                                                                    if ((bool)popTarget.EnablePopup)
                                                                                                                    {
                                                                                                                        popup = popup + " data-popup-enable='true'";
                                                                                                                        string t = string.Empty;


                                                                                                                        if (popTarget.TargetId != "" && popTarget.TargetId != null)
                                                                                                                        {
                                                                                                                            t = t + "," + popTarget.TargetId;
                                                                                                                        }

                                                                                                                        if (t != string.Empty)
                                                                                                                        {
                                                                                                                            t = t.Substring(1);
                                                                                                                            popup = popup + " data-popup-target='" + t + "'";

                                                                                                                            popup = popup + " data-popup-title='" + popTarget.PopUpTitle + "'";
                                                                                                                        }

                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        popup = " data-popup-enable='false'";
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                        var iHtml = string.Empty;
                                                                                                        if (coption.Icon != null)
                                                                                                        {
                                                                                                            iHtml = "<i class='" + coption.Icon + "' for='elm" + coption.Value + "'></i>";
                                                                                                        }
                                                                                                        chkoptions += "<div class='custom-control custom-checkbox " + inlineClass + " " + hasIcon + "'><input class='custom-control-input' " + colElmAttrAlt + " " + target + " " + popup + " data-checkbox=true" + " " + dataAttrChangeEvent + " " + dataAttrLoadEvent + " " + this.GenerateValidationData(ElementBody) + " type= 'checkbox' " + selected + " value = '" + coption.Value + "' name = 'elm" + colElement.ElementId + "[0]' data-repeat-name='elm" + colElement.ElementId + "[{0}]' id='elm" + colElement.ElementId + coption.Value + "[0]' data-repeat-id='elm" + colElement.ElementId + coption.Value + "[{0}]'" + colAttr + ">" + iHtml + "<label class='custom-control-label' for='elm" + colElement.ElementId + coption.Value + "[0]' data-repeat-for='elm" + colElement.ElementId + coption.Value + "[{0}]'" + colAttr + ">" + coption.Text + "</label></div>";
                                                                                                    }
                                                                                                }

                                                                                                colElms = "<div class='mb-2'>" + ((this.Side == "frontend") ? colElement.FrontendLabel : colElement.BackendLabel) + "</div><div>" + chkoptions + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + colElement.ElementId + "\" data-valmsg-replace=\"true\"></span></div></div>";
                                                                                                break;
                                                                                            #endregion
                                                                                            #region Media
                                                                                            case "media":
                                                                                                //elementstr = elementstr;
                                                                                                elementidside = ((this.Side == "frontend") ? colElement.FrontendId : colElement.BackendId) + "[]";
                                                                                                elmId = "Media" + colElement.ElementId + "[]";
                                                                                                elmName = "Media" + colElement.ElementId + "[]";
                                                                                                media = @"<div class='fileuploader__list'><ul class='fileuploader__items'>";

                                                                                                if (colElement.Images != null && colElement.Images.Count > 0)
                                                                                                {
                                                                                                    foreach (var image in colElement.Images)
                                                                                                    {
                                                                                                        switch (System.IO.Path.GetExtension(image.Url))
                                                                                                        {
                                                                                                            case ".pdf":
                                                                                                                media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/pdf-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? colElement.FrontendId : colElement.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                                                break;
                                                                                                            case ".doc":
                                                                                                                media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? colElement.FrontendId : colElement.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                                                break;
                                                                                                            case ".docx":
                                                                                                                media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? colElement.FrontendId : colElement.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                                                break;
                                                                                                            case ".txt":
                                                                                                                media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? colElement.FrontendId : colElement.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                                                break;
                                                                                                            default:
                                                                                                                media += "<li class=\"fileuploader__item\"><a class='pop' href=\"javascript: void(0)\"><div class='fileuploader__item-image'><img src=\"/uploads/" + image.Url + "\"></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' data-toggle='tooltip' data-placement='top' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? colElement.FrontendId : colElement.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                                                break;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                media += @" </ul></div><div class='fileuploader__input'><div class='fileuploader__input-caption'><span>Choose file</span></div><button type='button' class='btn fileuploader__input-button'" + colElmAttrAlt + " data-media='{OnInit:function(){this.size=\"modal-lg\";this.button.cancel=\"Cancel\";this.button.insert=\"Insert Media\";this.open();},OnInsert:function(e){Cicero.Form.InsertImages(e , \"" + elementidside + "\")},OnCancelled:function(){this.close();}}'>" +
                                                                                                "<span>Browse</span></button> </div><div class='d-flex'><span class='text danger field-validation-valid' data-valmsg-for='" + elementidside + "' data-valmsg-replace='true'></span></div>";
                                                                                                colElms = string.Format(colElementStr, ((this.Side == "frontend") ? colElement.FrontendClass : colElement.BackendClass), media, elmId, elmName);
                                                                                                break;
                                                                                            #endregion
                                                                                            default:
                                                                                                break;
                                                                                        }
                                                                                        //string elms = string.Format(elementstr, attr);

                                                                                        if (colElement.WrapperTemplate == "")
                                                                                        {
                                                                                            colElement.WrapperTemplate = "<div class=\"form-group {0}\">{1}</div>";
                                                                                        }
                                                                                        string displayData = "<div class='display-data' data-display-for='elm" + colElement.ElementId + "[0]' data-repeat-display-for='elm" + colElement.ElementId + "[{0}]'" + showHideLabel + ">No Data</div>";
                                                                                        elementCol = elementCol + string.Format(colElement.WrapperTemplate, "", displayData + colElms);

                                                                                        if (string.IsNullOrWhiteSpace(elementCol))
                                                                                        {
                                                                                            elementCol = colElms;
                                                                                        }
                                                                                        tableColStr = tableColStr + elementCol + "</td>";
                                                                                    }

                                                                                }
                                                                            }
                                                                            tableColStr = tableColStr + rowOption;
                                                                            tableColStr = tableColStr + "</tr></tbody>";
                                                                            tableStr = tableStr + tableColStr;
                                                                        }
                                                                        elementstr = ElementBody.Template;
                                                                        elms = string.Format(elementstr, attr, tableStr);
                                                                        break;
                                                                    #endregion
                                                                    default:
                                                                        break;
                                                                }
                                                                //string elms = string.Format(elementstr, attr);

                                                                if (ElementBody.WrapperTemplate == "")
                                                                {
                                                                    ElementBody.WrapperTemplate = "<div class=\"form-group {0}\">{1}</div>";
                                                                }
                                                                element = element + string.Format(ElementBody.WrapperTemplate, "", elms);

                                                                if (string.IsNullOrWhiteSpace(element))
                                                                {
                                                                    element = elms;
                                                                }
                                                            }

                                                        }
                                                    }

                                                }
                                                string colElmAppend = string.Empty;
                                                if (ColumnBody.AppendElement != null && ColumnBody.AppendElement.Count > 0)
                                                {

                                                    string appendElm = string.Empty;
                                                    foreach (var item in ColumnBody.AppendElement)
                                                    {
                                                        if (item != "0" && item != null)
                                                        {
                                                            appendElm = appendElm + "," + item;
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(appendElm))
                                                    {
                                                        appendElm = appendElm.Substring(1);
                                                        colElmAppend = "data-appendElm ='" + appendElm + "'";
                                                    }

                                                }

                                                var columnstr = (this.Side == "frontend") ? string.Format(strcolumn, ColumnBody.FrontendClass, element, ColumnBody.ElementId, colElmAppend) : string.Format(strcolumn, ColumnBody.BackendClass, element, ColumnBody.ElementId, colElmAppend);
                                                column = column + columnstr;
                                            }

                                        }
                                    }

                                    string rowElmAppend = string.Empty;
                                    if (RowBody.AppendElement != null && RowBody.AppendElement.Count > 0)
                                    {

                                        string appendElm = string.Empty;
                                        foreach (var item in RowBody.AppendElement)
                                        {
                                            if (item != "0" && item != null)
                                            {
                                                appendElm = appendElm + "," + item;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(appendElm))
                                        {
                                            appendElm = appendElm.Substring(1);
                                            rowElmAppend = "data-appendElm ='" + appendElm + "'";
                                        }

                                    }
                                    string rowstr = (this.Side == "frontend") ? string.Format(strrow, RowBody.FrontendClass, column, RowBody.ElementId, rowElmAppend) : string.Format(strrow, RowBody.BackendClass, column, RowBody.ElementId, rowElmAppend);
                                    row = row + rowstr;
                                }

                            }
                        }

                        string tabstr = "";

                        if (this.FormData.Tab.IndexOf(TabBody) == 0)
                        {
                            tabstr = (this.Side == "frontend") ? string.Format(strtab, TabBody.FrontendClass + " show active", TabBody.FrontendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'") : string.Format(strtab, TabBody.BackendClass + " show active", TabBody.BackendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'");

                        }
                        else
                        {
                            tabstr = (this.Side == "frontend") ? string.Format(strtab, TabBody.FrontendClass, TabBody.FrontendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'") : string.Format(strtab, TabBody.BackendClass, TabBody.BackendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'");

                        }

                        // Case Timeline
                        if (TabBody.TabType == (int)TabType.CaseTimeLine)
                        {
                            if (Convert.ToInt32(caseId) > 0)
                            {
                                caseStateHistoryItems = caseService.GetStateHistory(Convert.ToInt32(caseId));
                                if (caseStateHistoryItems.Count > 0 && caseStateHistoryItems != null)
                                {
                                    foreach (var item in caseStateHistoryItems)
                                    {
                                        var userImage = item.User.UserMedias != null ? item.User.UserMedias.FirstOrDefault().Media.Url : "";
                                        var updatedBy = item.User != null ? item.User.FirstName + " " + item.User.LastName : "";
                                        caseContent = caseContent + "<li class='case-state-timeline__item'><div class='case-state__title'>"
                                                                     + item.PreviousStateName +
                                                                   "</div><div class='transiton-arrow'></div>" +
                                             "<div class='case-state__user'><span class='case-state__user-img'><img src='/uploads/" + item.UpdatedByImg +
                                            "' alt='" + updatedBy + "'></span><span class='case-state__user-title'>" +
                                              " " + updatedBy + "</span></div><div class='case-" +
                                              "" +
                                              "state__date'>" +
                                             item.UpdatedAt + "</div></li>";
                                    }
                                    caseContent = caseContent + "<li class='case-state-timeline__item case-state-timeline__item--last'><div class='case-state__title'>"
                                                                    + caseStateHistoryItems.Last().CurrentStateName +
                                                                  "</div></li>";
                                }
                                else
                                {
                                    caseContent = "No history found.";
                                }
                                caseContent = " <ul class='case-state-timeline'>" + caseContent + "</ul>";
                                tabstr = tabstr.Replace("</div>", " ");
                                tabstr = tabstr + "<h3 class = 'casetimeline-title mb-4'>Case Timeline</h3>" + caseContent + "</div>";
                            }
                        }
                        this.Collection = this.Collection + tabstr;
                    }
                }
                //static tab class
                tab = "<ul class='nav nav-tabs nav-tabs-custom' role='tablist'>" + tab + "</ul>";
                if (this.Side == "frontend")
                    this.Collection = "<div class='form-layout__tabs'>" + tab + "</div>"
                    + "<div class='form-layout__content'>"
                    //+ "<div class='form-builder-header'><h2>Home and Contents Claim Form</h2><p class='text-muted'>Please make sure this claim form is completed clearly and in full to ensure the correct assessment of your claim.</p></div>" //.form-builder-header
                    + "<div class='form-layout__content-wrapper'><div class='form-layout-card'><div class='form-layout-card__content'><div class='tab-content tab-content-custom' id='myTabContent'>" + this.Collection + "</div></div></div></div></div>";
                // Note: Above .form-layout__content, .form-layout__content-wrapper and .form-layout-card class's closing is located in \Themes\Blue\Form\Edit.cshtml file.
                else
                    this.Collection = tab + "<div class ='tab-content'>" + this.Collection + "</div>";
            }
            catch (Exception ex)
            {

            }
            this.Collection += "<input hidden id='isFrontEnd' value='" + this.Side + "'/>";
            return this.Collection;
        }


        private void LoadCaseTimeline(string tab)
        {
            //Case Timeline
            var caseTimelineTab = new Elements.Tab();
            caseTimelineTab.BackendLabel = "Case Timeline";
            caseTimelineTab.FrontendLabel = "Case Timeline";
            tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + caseTimelineTab.ElementId + "' href='#" + caseTimelineTab.TabIndex + "' data-toggle='tab' role='tab' aria-controls = '" + caseTimelineTab.FrontendLabel + "' aria-selected = 'true' > " + caseTimelineTab.BackendIcon + caseTimelineTab.FrontendLabel + "</a></li>";

            this.FormData.Tab.Add(caseTimelineTab);
        }

        public string RenderEdit(dynamic jbojData, string sides = "backend")
        {
            try
            {
                JObject EditFinalData = JObject.Parse(jbojData.ToString());
                if (this.commonService == null) this.commonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                string tab = string.Empty;

                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);

                foreach (Elements.Tab TabBody in this.FormData.Tab)
                {
                    bool temp = (this.Side == "frontend") ? TabBody.FrontendVisible : TabBody.BackendVisible;

                    if (temp == true)
                    {

                        string tabid = (this.Side == "frontend") ? TabBody.FrontendId : TabBody.BackendId;
                        string tablabel = (this.Side == "frontend") ? TabBody.FrontendLabel : TabBody.BackendLabel;

                        //static li and a class\
                        if (this.Side == "frontend")
                        {


                            if (this.FormData.Tab.IndexOf(TabBody) == 0)
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link active position-relative' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' ><span class='nav-count'><i>1</i></span>" + tablabel + "</a></li>";

                            }
                            else
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > <span class='nav-count'><i>" + (Convert.ToInt32(this.FormData.Tab.IndexOf(TabBody)) + 1).ToString() + "</i></span>" + tablabel + "</a></li>";

                            }
                        }
                        else
                        {
                            if (this.FormData.Tab.IndexOf(TabBody) == 0)
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link active' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > " + tablabel + "</a></li>";

                            }
                            else
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > " + tablabel + "</a></li>";

                            }
                        }

                        string row = string.Empty;
                        //string strtab = (TabBody.Template == null ? "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>" : TabBody.Template);
                        string strtab = "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>";
                        if (TabBody.Row != null && TabBody.Row.Count() > 0)
                        {
                            foreach (Row RowBody in TabBody.Row)
                            {
                                bool rowVisible = (this.Side == "frontend") ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                if (rowVisible)
                                {
                                    string strrow = RowBody.Template;
                                    string column = string.Empty;
                                    //this.Collection= this.Collection+TabBody.
                                    if (RowBody.Column != null && RowBody.Column.Count() > 0)
                                    {
                                        foreach (Elements.Column ColumnBody in RowBody.Column)
                                        {
                                            bool ColVisible = (this.Side == "frontend") ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                            if (ColVisible)
                                            {
                                                string element = "";
                                                string strcolumn = ColumnBody.Template;
                                                //this.Collection= this.Collection+TabBody.
                                                if (ColumnBody.Element != null && ColumnBody.Element.Count() > 0)
                                                {
                                                    foreach (dynamic ElementBody in ColumnBody.Element)
                                                    {

                                                        if (ElementBody != null)
                                                        {
                                                            bool EleVisible = (this.Side == "frontend") ? ElementBody.FrontendVisible : ElementBody.BackendVisible;
                                                            if (EleVisible)
                                                            {
                                                                //replace with another logic for view

                                                                string priviledge = "data";
                                                                string priviledgeAlt = "data";
                                                                if (roleid != " ")
                                                                {
                                                                    if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                                    {

                                                                        foreach (var permission in ElementBody.Permissions)
                                                                        {
                                                                            if (permission.RoleId == roleid)
                                                                            {
                                                                                if (permission.Read == true && permission.Write == false)
                                                                                {
                                                                                    priviledge = "disabled";
                                                                                    priviledgeAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                                }
                                                                                else if (permission.Read == false && permission.Write == false)
                                                                                {
                                                                                    priviledge = "hidden";
                                                                                    priviledgeAlt = "hidden";
                                                                                }
                                                                                else
                                                                                {
                                                                                    priviledge = "";
                                                                                    priviledgeAlt = "";
                                                                                }
                                                                            }

                                                                        }
                                                                        if (priviledge == "data")
                                                                        {
                                                                            priviledge = "hidden";
                                                                            priviledgeAlt = "hidden";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    priviledge = "";
                                                                    priviledgeAlt = "";
                                                                }
                                                                //till here

                                                                string elementstr = string.Empty;

                                                                var label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "' " + priviledge + ">" + label + "</label>";
                                                                if (priviledge != "hidden")
                                                                {
                                                                    elementstr = elementstr + ElementBody.Template + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                }
                                                                else
                                                                {
                                                                    elementstr = elementstr + ElementBody.Template;
                                                                }
                                                                string attr = string.Empty;



                                                                if (this.Side == "frontend")
                                                                {
                                                                    attr = this.GenerateValidationData(ElementBody) + priviledge + " name=\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.FrontendClass + "\"";
                                                                }
                                                                else
                                                                {
                                                                    attr = this.GenerateValidationData(ElementBody) + priviledge + " name =\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.BackendClass + "\"";

                                                                }

                                                                string elms = "";

                                                                switch (ElementBody.GetType().Name.ToLower())
                                                                {

                                                                    #region paragraph
                                                                    case "paragraph":
                                                                        elementstr = ElementBody.Template;
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), ElementBody.ParagraphText);
                                                                        break;
                                                                    #endregion
                                                                    #region TextArea
                                                                    case "textarea":
                                                                        string taval = "";
                                                                        if (EditFinalData["elm" + ElementBody.ElementId] != null)
                                                                        {
                                                                            taval = EditFinalData["elm" + ElementBody.ElementId].Value;
                                                                        }
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), taval, attr);
                                                                        break;
                                                                    #endregion
                                                                    #region Heading
                                                                    case "heading":
                                                                        elms = "<" + ElementBody.HeaderType + " class = '" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "'> " + ElementBody.HeaderText + " </" + ElementBody.HeaderType + ">";
                                                                        break;
                                                                    #endregion
                                                                    #region TextBox
                                                                    case "textbox":
                                                                        elms = string.Format(elementstr, attr);
                                                                        break;
                                                                    #endregion
                                                                    #region Number
                                                                    case "number":
                                                                        string ntarget = "";
                                                                        string defaultValue = "";
                                                                        List<Number.TargetOption> ntargets = (List<Number.TargetOption>)ElementBody.TargetOptions;
                                                                        if (ntargets != null)
                                                                        {
                                                                            foreach (var targetoption in ntargets)
                                                                            {
                                                                                if (targetoption.TargetId != "0")
                                                                                {
                                                                                    if (ntargets[0] == targetoption)
                                                                                    {
                                                                                        ntarget = "#" + targetoption.TargetId;

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        ntarget = ntarget + ",#" + targetoption.TargetId;

                                                                                    }
                                                                                }

                                                                            }
                                                                        }
                                                                        if (ElementBody.DefaultValue != null)
                                                                        {
                                                                            defaultValue = " value='" + ElementBody.DefaultValue + "'";
                                                                        }
                                                                        if (!string.IsNullOrWhiteSpace(ntarget))
                                                                        {
                                                                            ntarget = " data-target='" + ntarget + "' number-target";
                                                                        }

                                                                        elms = string.Format(elementstr, attr + ntarget + defaultValue);
                                                                        break;
                                                                    #endregion
                                                                    #region SelectBox
                                                                    case "selectbox":

                                                                        string seloptions = "";
                                                                        if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                        {

                                                                            if ((this.Side == "frontend" && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("IMAGE")) || (this.Side == "backend" && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("IMAGE")))
                                                                            {
                                                                                label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "' " + priviledgeAlt + ">" + label + "</label>" + ElementBody.AltTemplate;

                                                                                //elementstr = ElementBody.AltTemplate;
                                                                                string value = string.Empty;
                                                                                string selected = string.Empty;
                                                                                foreach (var soption in ElementBody.SelectOptions)
                                                                                {
                                                                                    if (soption.Selected == true)
                                                                                    {
                                                                                        selected = " show ";
                                                                                        value = soption.Value;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }

                                                                                    string target = "";

                                                                                    List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }


                                                                                    seloptions = seloptions + "<div " + target + " class=\"  col-md-3 mb-3\"><a onclick='setValueOn(\"elm" + ElementBody.ElementId + "\",\"" + soption.Value + " \")' class='Image-target module-tab" + selected + "' value ='" + soption.Value + "' " + selected + " " + target + "><i class=\"module-icon module-icon--check\"></i><div class='module-type module-type--img d-flex justify-content-center align-items-center'><img src = '/images/" + soption.IconUrl + ".png' alt = '" + soption.IconUrl + "' ></div><p class='m-0'>" + soption.Text + "</p></a></div>";

                                                                                }
                                                                                seloptions += "<input name = 'elm" + ElementBody.ElementId + "' type= 'text-box'" + priviledgeAlt + this.GenerateValidationData(ElementBody) + " value = '" + value + "' class='custom-control-input' id='elm" + ElementBody.ElementId + "'/>";
                                                                                seloptions += "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";
                                                                                elms = string.Format(elementstr, seloptions, " ", priviledgeAlt);
                                                                            }
                                                                            else
                                                                            {
                                                                                string selected = string.Empty;
                                                                                foreach (var soption in ElementBody.SelectOptions)
                                                                                {
                                                                                    if (soption.Selected == true)
                                                                                    {
                                                                                        selected = "selected";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }

                                                                                    string target = "";

                                                                                    List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }


                                                                                    seloptions = seloptions + "<option value='" + soption.Value + "' " + selected + " " + target + ">" + soption.Text + "</option>";

                                                                                }
                                                                                elms = string.Format(elementstr, attr + " select-target", seloptions);
                                                                            }

                                                                        }

                                                                        break;
                                                                    #endregion
                                                                    #region MultiSelectBox
                                                                    case "multiselectbox":
                                                                        string mulseloptions = "";
                                                                        if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                        {

                                                                            string selected = string.Empty;
                                                                            foreach (var msoption in ElementBody.SelectOptions)
                                                                            {
                                                                                if (msoption.Selected == true)
                                                                                {
                                                                                    selected = "selected";
                                                                                }
                                                                                else
                                                                                {
                                                                                    selected = "";
                                                                                }

                                                                                mulseloptions = mulseloptions + "<option value='" + msoption.Value + "' " + selected + ">" + msoption.Text + "</option>";
                                                                            }
                                                                        }

                                                                        elms = string.Format(elementstr, attr, mulseloptions);
                                                                        break;
                                                                    #endregion
                                                                    #region RadioGroup
                                                                    case "radiogroup":
                                                                        string radoptions = "";
                                                                        if (ElementBody.RadioOptions != null && ElementBody.RadioOptions.Count > 0)
                                                                        {
                                                                            string selected = string.Empty;
                                                                            string radval = "";
                                                                            if (EditFinalData["elm" + ElementBody.ElementId] != null)
                                                                            {
                                                                                radval = EditFinalData["elm" + ElementBody.ElementId].Value;
                                                                            }
                                                                            int i = 0;
                                                                            if (((this.Side == "frontend" && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("ONOFF")) || (this.Side == "backend" && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("ONOFF"))) && ElementBody.RadioOptions.Count == 2)
                                                                            {
                                                                                string targetY = "";
                                                                                string targetN = "";
                                                                                string vals = "false";
                                                                                foreach (var roption in ElementBody.RadioOptions)
                                                                                {
                                                                                    if (roption.Value == "Y" || roption.Value == "Yes")
                                                                                    {
                                                                                        vals = "true";
                                                                                        List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                targetY = "data-targety='" + toption + "' data-actiony='" + taction + "'";
                                                                                            }
                                                                                        }

                                                                                        if (roption.Selected || radval == "true")
                                                                                        {
                                                                                            selected = "checked";
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                targetN = "data-targetn='" + toption + "' data-actionn='" + taction + "'";
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    i++;
                                                                                }



                                                                                radoptions += "<div  class='custom-control custom-switch " + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + " '> <input  " + targetY + " " + targetN + " data-toggle='slider' name='elm" + ElementBody.ElementId + "' type='checkbox'" + priviledgeAlt + this.GenerateValidationData(ElementBody) + " value='" + vals + "' class='custom-control-input radio-slider-target' id='" + ElementBody.ElementId + "switch'  " + selected + " />  <label class='custom-control-label' for='" + ElementBody.ElementId + "switch' >  </label> </div>";

                                                                            }
                                                                            else
                                                                            {
                                                                                foreach (var roption in ElementBody.RadioOptions)
                                                                                {
                                                                                    if (roption.Selected == true)
                                                                                    {
                                                                                        selected = "checked";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }
                                                                                    string target = "";

                                                                                    List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }


                                                                                    //radoptions += "<div class='custom-control custom-radio custom-control-inline'><input class='custom-control-input' type= 'radio' " + selected + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + (Utils.ConvertToString(roption.Value) == "" ? (ElementBody.ElementId + i) : roption.Value) + "'><label class='custom-control-label' for='elm" + roption.Value + "'>" + roption.Text + "</label></div>";
                                                                                    radoptions += "<div " + target + " class='custom-control custom-radio custom-control-inline'><input  class='custom-control-input radio-target' " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'radio' " + selected + " " + target + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + ElementBody.ElementId + roption.Value + "'><label class='custom-control-label' for='elm" + ElementBody.ElementId + roption.Value + "'>" + roption.Text + "</label></div>";
                                                                                    i++;
                                                                                }
                                                                            }


                                                                        }

                                                                        elms = "<div class='mb-2 '>" + ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel) + "</div><div>" + radoptions + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div></div>";
                                                                        break;
                                                                    #endregion
                                                                    #region CheckBoxGroup
                                                                    case "checkboxgroup":
                                                                        string chkoptions = "";
                                                                        string currentval = "";
                                                                        if (EditFinalData["elm" + ElementBody.ElementId] != null)
                                                                        {
                                                                            currentval = EditFinalData["elm" + ElementBody.ElementId].Value;
                                                                        }
                                                                        if (ElementBody.CheckOptions != null && ElementBody.CheckOptions.Count > 0)
                                                                        {

                                                                            string selected = string.Empty;
                                                                            foreach (var coption in ElementBody.CheckOptions)
                                                                            {
                                                                                if (currentval.Contains(coption.Value))
                                                                                {
                                                                                    selected = "checked";
                                                                                }
                                                                                else
                                                                                {
                                                                                    selected = "";
                                                                                }

                                                                                chkoptions += "<div class='custom-control custom-checkbox custom-control-inline'><input class='custom-control-input' " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'checkbox' " + selected + " value = '" + coption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + ElementBody.ElementId + "_" + coption.Value + "'><label class='custom-control-label' for='elm" + ElementBody.ElementId + "_" + coption.Value + "'>" + coption.Text + "</label></div>";
                                                                            }
                                                                        }

                                                                        elms = "<div class='mb-2'>" + ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel) + "</div><div>" + chkoptions + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div></div>";
                                                                        break;
                                                                    #endregion
                                                                    #region Media
                                                                    case "media":
                                                                        elementstr = ElementBody.Template;
                                                                        string elementidside = ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]";
                                                                        string media = @"<div class='fileuploader__list'><ul class='fileuploader__items'>";

                                                                        if (ElementBody.Images != null && ElementBody.Images.Count > 0)
                                                                        {
                                                                            foreach (var image in ElementBody.Images)
                                                                            {
                                                                                switch (System.IO.Path.GetExtension(image.Url))
                                                                                {
                                                                                    case ".pdf":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/pdf-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    case ".doc":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    case ".docx":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    case ".txt":
                                                                                        media += "<li class=\"fileuploader__item\"><div class=\"fileuploader__item-image\"><a target='_blank' href=\"javascript: void(0)\"><img src=\"/uploads/doc-icon.png\"><i class=\"fas\" aria-hidden=\"true\"><img src='/frontend/img/delete.png' /></i></a></div><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                    default:
                                                                                        media += "<li class=\"fileuploader__item\"><a class='pop' href=\"javascript: void(0)\"><div class='fileuploader__item-image'><img src=\"/uploads/" + image.Url + "\"></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' data-toggle='tooltip' data-placement='top' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>";
                                                                                        break;
                                                                                }
                                                                            }
                                                                        }
                                                                        media += @" </ul></div><div class='fileuploader__input'><div class='fileuploader__input-caption'><span>Choose file</span></div><button type='button' class='btn fileuploader__input-button'" + priviledgeAlt + " data-media='{OnInit:function(){this.size=\"modal-lg\";this.button.cancel=\"Cancel\";this.button.insert=\"Insert Media\";this.open();},OnInsert:function(e){Cicero.Form.InsertImages(e , \"" + elementidside + "\")},OnCancelled:function(){this.close();}}'>" +
                                                                        "<span>Browse</span></button> </div><div class='d-flex'><span class='text-danger field-validation-valid' data-valmsg-for='" + elementidside + "' data-valmsg-replace='true'></span></div>";
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), media);
                                                                        break;
                                                                    #endregion
                                                                    default:
                                                                        break;
                                                                }
                                                                //string elms = string.Format(elementstr, attr);
                                                                element = element + string.Format(ElementBody.WrapperTemplate, "", elms);

                                                                if (string.IsNullOrWhiteSpace(element))
                                                                {
                                                                    element = elms;
                                                                }
                                                            }


                                                        }
                                                    }
                                                }
                                                var columnstr = (this.Side == "frontend") ? string.Format(strcolumn, ColumnBody.FrontendClass, element, ColumnBody.ElementId) : string.Format(strcolumn, ColumnBody.BackendClass, element, ColumnBody.ElementId);
                                                column = column + columnstr;
                                            }

                                        }
                                    }
                                    string rowstr = (this.Side == "frontend") ? string.Format(strrow, RowBody.FrontendClass, column, RowBody.ElementId) : string.Format(strrow, RowBody.BackendClass, column, RowBody.ElementId);
                                    row = row + rowstr;
                                }

                            }
                        }

                        string tabstr = "";

                        if (this.FormData.Tab.IndexOf(TabBody) == 0)
                        {
                            tabstr = (this.Side == "frontend") ? string.Format(strtab, TabBody.FrontendClass + " show active", TabBody.FrontendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'") : string.Format(strtab, TabBody.BackendClass + " show active", TabBody.BackendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'");

                        }
                        else
                        {
                            tabstr = (this.Side == "frontend") ? string.Format(strtab, TabBody.FrontendClass, TabBody.FrontendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'") : string.Format(strtab, TabBody.BackendClass, TabBody.BackendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'");

                        }

                        this.Collection = this.Collection + tabstr;
                    }

                }

                //static tab class
                tab = "<ul class='nav nav-tabs nav-tabs-custom px-3 border-bottom' role='tablist'>" + tab + "</ul>";
                if (this.Side == "frontend")
                    this.Collection = "<div class='nav-tabs-bg'>" + tab + "</div>"
                    + "<div class='container'>"
                    + "<div class='form-builder-header'><h2>Home and Contents Claim Form</h2><p class='text-muted'>Please make sure this claim form is completed clearly and in full to ensure the correct assessment of your claim.</p></div>" //.form-builder-header
                    + "<div class='form-builder my-5'><div class='card'><div class='card-body'><div class='tab-content tab-content-custom' id='myTabContent'>" + this.Collection + "</div></div>";
                // Note: Above .container, .form-builder and .card class's closing is located in C:\Projects\Cicero\Cicero\Themes\Blue\Form\Edit.cshtml file.
                else
                    this.Collection = tab + "<div class ='tab-content'>" + this.Collection + "</div>";
            }
            catch (Exception ex)
            {

            }

            return this.Collection;
        }
        public string RenderView(string sides = "backend")
        {
            try
            {
                if (this.commonService == null) this.commonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                string tab = string.Empty;

                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);

                foreach (Elements.Tab TabBody in this.FormData.Tab)
                {
                    bool temp = (this.Side == "frontend") ? TabBody.FrontendVisible : TabBody.BackendVisible;

                    if (temp == true)
                    {

                        string tabid = (this.Side == "frontend") ? TabBody.FrontendId : TabBody.BackendId;
                        string tablabel = (this.Side == "frontend") ? TabBody.FrontendLabel : TabBody.BackendLabel;

                        //static li and a class\
                        if (this.Side == "frontend")
                        {


                            if (this.FormData.Tab.IndexOf(TabBody) == 0)
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link active position-relative' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' ><span class='nav-count'><i>1</i></span>" + tablabel + "</a></li>";

                            }
                            else
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > <span class='nav-count'><i>" + (Convert.ToInt32(this.FormData.Tab.IndexOf(TabBody)) + 1).ToString() + "</i></span>" + tablabel + "</a></li>";

                            }
                        }
                        else
                        {
                            if (this.FormData.Tab.IndexOf(TabBody) == 0)
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link active' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > " + tablabel + "</a></li>";

                            }
                            else
                            {
                                tab = tab + "<li class='nav-item'><a class='nav-link' id='tab" + TabBody.ElementId + "' href='#" + tabid + "' data-toggle='tab' role='tab' aria-controls = '" + tablabel + "' aria-selected = 'true' > " + tablabel + "</a></li>";

                            }
                        }

                        string row = string.Empty;
                        //string strtab = (TabBody.Template == null ? "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>" : TabBody.Template);
                        string strtab = "<div class=\"{0}\" id=\"{1}\" {3}>{2}</div>";
                        if (TabBody.Row != null && TabBody.Row.Count() > 0)
                        {
                            foreach (Row RowBody in TabBody.Row)
                            {
                                bool rowVisible = (this.Side == "frontend") ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                if (rowVisible)
                                {
                                    string strrow = RowBody.Template;
                                    string column = string.Empty;
                                    //this.Collection= this.Collection+TabBody.
                                    if (RowBody.Column != null && RowBody.Column.Count() > 0)
                                    {
                                        foreach (Elements.Column ColumnBody in RowBody.Column)
                                        {
                                            bool ColVisible = (this.Side == "frontend") ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                            if (ColVisible)
                                            {
                                                string element = "";
                                                string strcolumn = ColumnBody.Template;
                                                //this.Collection= this.Collection+TabBody.
                                                if (ColumnBody.Element != null && ColumnBody.Element.Count() > 0)
                                                {
                                                    foreach (dynamic ElementBody in ColumnBody.Element)
                                                    {

                                                        if (ElementBody != null)
                                                        {
                                                            bool EleVisible = (this.Side == "frontend") ? ElementBody.FrontendVisible : ElementBody.BackendVisible;
                                                            if (EleVisible)
                                                            {
                                                                //replace with another logic for view

                                                                string priviledge = "disabled";
                                                                string priviledgeAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                if (roleid != " ")
                                                                {
                                                                    if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                                    {

                                                                        foreach (var permission in ElementBody.Permissions)
                                                                        {
                                                                            if (permission.RoleId == roleid)
                                                                            {
                                                                                if (permission.Read == false && permission.Write == false)
                                                                                {
                                                                                    priviledge = "hidden";
                                                                                    priviledgeAlt = "hidden";
                                                                                }

                                                                            }

                                                                        }
                                                                        if (priviledge == "data")
                                                                        {
                                                                            priviledge = "hidden";
                                                                            priviledgeAlt = "hidden";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    priviledge = "disabled";
                                                                    priviledgeAlt = "disabled style='pointer-events:none;opacity:0.5;'";
                                                                }
                                                                //till here

                                                                string elementstr = string.Empty;

                                                                var label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "' " + priviledge + ">" + label + "</label>";
                                                                if (priviledge != "hidden")
                                                                {
                                                                    elementstr = elementstr + ElementBody.Template + "<div class='d-flex'><span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";

                                                                }
                                                                else
                                                                {
                                                                    elementstr = elementstr + ElementBody.Template;
                                                                }
                                                                string attr = string.Empty;



                                                                if (this.Side == "frontend")
                                                                {
                                                                    attr = this.GenerateValidationData(ElementBody) + priviledge + " name=\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.FrontendClass + "\"";
                                                                }
                                                                else
                                                                {
                                                                    attr = this.GenerateValidationData(ElementBody) + priviledge + " name =\"elm" + ElementBody.ElementId + "\" id=\"elm" + ElementBody.ElementId + "\" class=\"valid " + ElementBody.BackendClass + "\"";

                                                                }

                                                                string elms = "";

                                                                switch (ElementBody.GetType().Name.ToLower())
                                                                {
                                                                    //case "":
                                                                    //    break;

                                                                    case "paragraph":
                                                                        elementstr = ElementBody.Template;
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), ElementBody.ParagraphText);
                                                                        break;

                                                                    case "textarea":
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), "", attr);
                                                                        break;

                                                                    case "heading":
                                                                        elms = "<" + ElementBody.HeaderType + " class = '" + ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass) + "'> " + ElementBody.HeaderText + " </" + ElementBody.HeaderType + ">";
                                                                        break;

                                                                    case "textbox":
                                                                        elms = string.Format(elementstr, attr);
                                                                        break;

                                                                    case "number":
                                                                        string ntarget = "";
                                                                        string defaultValue = "";

                                                                        List<Number.TargetOption> ntargets = (List<Number.TargetOption>)ElementBody.TargetOptions;
                                                                        if (ntargets != null)
                                                                        {
                                                                            foreach (var targetoption in ntargets)
                                                                            {
                                                                                if (targetoption.TargetId != "0")
                                                                                {
                                                                                    if (ntargets[0] == targetoption)
                                                                                    {
                                                                                        ntarget = "#" + targetoption.TargetId;

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        ntarget = ntarget + ",#" + targetoption.TargetId;

                                                                                    }
                                                                                }

                                                                            }
                                                                        }
                                                                        if (ElementBody.DefaultValue != null)
                                                                        {
                                                                            defaultValue = " value='" + ElementBody.DefaultValue + "'";
                                                                        }
                                                                        if (!string.IsNullOrWhiteSpace(ntarget))
                                                                        {
                                                                            ntarget = " data-target='" + ntarget + "' number-target";
                                                                        }

                                                                        elms = string.Format(elementstr, attr + ntarget + defaultValue);
                                                                        break;

                                                                    case "selectbox":

                                                                        string seloptions = "";
                                                                        if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                        {

                                                                            if ((this.Side == "frontend" && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("IMAGE")) || (this.Side == "backend" && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("IMAGE")))
                                                                            {
                                                                                label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                                elementstr = "<label for='elm" + ElementBody.ElementId + "' " + priviledgeAlt + ">" + label + "</label>" + ElementBody.AltTemplate;

                                                                                //elementstr = ElementBody.AltTemplate;

                                                                                string selected = string.Empty;
                                                                                foreach (var soption in ElementBody.SelectOptions)
                                                                                {
                                                                                    if (soption.Selected == true)
                                                                                    {
                                                                                        selected = " show ";

                                                                                        string target = "";

                                                                                        List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                            }
                                                                                        }


                                                                                        seloptions = seloptions + "<div " + target + " class=\"  col-md-3 mb-3\"><a class='Image-target card card-secondary card-hover p-4 d-flex flex-column justify-content-center text-center " + selected + "' value ='" + soption.Value + "' " + selected + " " + target + "><i class=\"module-icon module-icon--check\"></i><div class='module-type module-type--img d-flex justify-content-center align-items-center'><img src = '/images/" + soption.IconUrl + ".png' alt = '" + soption.IconUrl + "' ></div><p class='m-0'>" + soption.Text + "</p></a></div>";
                                                                                    }
                                                                                }
                                                                                elms = string.Format(elementstr, seloptions, " ", priviledgeAlt);
                                                                            }
                                                                            else
                                                                            {
                                                                                string selected = string.Empty;
                                                                                foreach (var soption in ElementBody.SelectOptions)
                                                                                {
                                                                                    if (soption.Selected == true)
                                                                                    {
                                                                                        selected = "selected";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }

                                                                                    string target = "";

                                                                                    List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == soption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }


                                                                                    seloptions = seloptions + "<option value='" + soption.Value + "' " + selected + " " + target + ">" + soption.Text + "</option>";

                                                                                }
                                                                                elms = string.Format(elementstr, attr + " select-target", seloptions);
                                                                            }

                                                                        }


                                                                        break;

                                                                    case "multiselectbox":
                                                                        string mulseloptions = "";
                                                                        if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                        {

                                                                            string selected = string.Empty;
                                                                            foreach (var msoption in ElementBody.SelectOptions)
                                                                            {
                                                                                if (msoption.Selected == true)
                                                                                {
                                                                                    selected = "selected";
                                                                                }
                                                                                else
                                                                                {
                                                                                    selected = "";
                                                                                }

                                                                                mulseloptions = mulseloptions + "<option value='" + msoption.Value + "' " + selected + ">" + msoption.Text + "</option>";
                                                                            }
                                                                        }

                                                                        elms = string.Format(elementstr, attr, mulseloptions);
                                                                        break;

                                                                    case "radiogroup":
                                                                        string radoptions = "";
                                                                        if (ElementBody.RadioOptions != null && ElementBody.RadioOptions.Count > 0)
                                                                        {
                                                                            string selected = string.Empty;
                                                                            int i = 0;
                                                                            if (((this.Side == "frontend" && ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToUpper().Contains("ONOFF")) || (this.Side == "backend" && ElementBody.BackendClass != null && ElementBody.BackendClass.ToUpper().Contains("ONOFF"))) && ElementBody.RadioOptions.Count == 2)
                                                                            {
                                                                                string targetY = "";
                                                                                string targetN = "";
                                                                                string vals = "false";
                                                                                foreach (var roption in ElementBody.RadioOptions)
                                                                                {
                                                                                    if (roption.Value == "Y" || roption.Value == "Yes")
                                                                                    {
                                                                                        vals = "true";
                                                                                        List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                targetY = "data-targety='" + toption + "' data-actiony='" + taction + "'";
                                                                                            }
                                                                                        }

                                                                                        if (roption.Selected)
                                                                                        {
                                                                                            selected = "checked";
                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                        if (targets != null)
                                                                                        {
                                                                                            var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                            string toption = "";
                                                                                            string taction = "";

                                                                                            foreach (var targetoption in result)
                                                                                            {
                                                                                                if (result[0] == targetoption)
                                                                                                {
                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                    taction = targetoption.ShowHide;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                }

                                                                                            }

                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                            {
                                                                                                targetN = "data-targetn='" + toption + "' data-actionn='" + taction + "'";
                                                                                            }
                                                                                        }
                                                                                    }

                                                                                    i++;
                                                                                }



                                                                                radoptions += "<div  class='custom-control custom-switch '> <input  " + targetY + " " + targetN + " data-toggle='slider' type='checkbox'" + priviledgeAlt + this.GenerateValidationData(ElementBody) + " value='" + vals + "' class='custom-control-input radio-slider-target' id='" + ElementBody.ElementId + "switch'  " + selected + " />  <label class='custom-control-label' for='" + ElementBody.ElementId + "switch' >  </label> </div>";

                                                                            }
                                                                            else
                                                                            {
                                                                                foreach (var roption in ElementBody.RadioOptions)
                                                                                {
                                                                                    if (roption.Selected == true)
                                                                                    {
                                                                                        selected = "checked";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        selected = "";
                                                                                    }
                                                                                    string target = "";

                                                                                    List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                    if (targets != null)
                                                                                    {
                                                                                        var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                        string toption = "";
                                                                                        string taction = "";

                                                                                        foreach (var targetoption in result)
                                                                                        {
                                                                                            if (result[0] == targetoption)
                                                                                            {
                                                                                                toption = "#" + targetoption.TargetId;

                                                                                                taction = targetoption.ShowHide;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                toption = toption + ",#" + targetoption.TargetId;

                                                                                                taction = taction + "," + targetoption.ShowHide;
                                                                                            }

                                                                                        }

                                                                                        if (!string.IsNullOrWhiteSpace(toption))
                                                                                        {
                                                                                            target = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                        }
                                                                                    }


                                                                                    //radoptions += "<div class='custom-control custom-radio custom-control-inline'><input class='custom-control-input' type= 'radio' " + selected + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + (Utils.ConvertToString(roption.Value) == "" ? (ElementBody.ElementId + i) : roption.Value) + "'><label class='custom-control-label' for='elm" + roption.Value + "'>" + roption.Text + "</label></div>";
                                                                                    radoptions += "<div " + target + " class='custom-control custom-radio custom-control-inline'><input  class='custom-control-input radio-target' " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'radio' " + selected + " " + target + " value = '" + roption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + ElementBody.ElementId + roption.Value + "'><label class='custom-control-label' for='elm" + ElementBody.ElementId + roption.Value + "'>" + roption.Text + "</label></div>";
                                                                                    i++;
                                                                                }
                                                                            }


                                                                        }

                                                                        elms = "<div class='mb-2 '>" + ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel) + "</div><div>" + radoptions + "<span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";
                                                                        break;
                                                                    case "checkboxgroup":
                                                                        string chkoptions = "";
                                                                        if (ElementBody.CheckOptions != null && ElementBody.CheckOptions.Count > 0)
                                                                        {

                                                                            string selected = string.Empty;
                                                                            foreach (var coption in ElementBody.CheckOptions)
                                                                            {
                                                                                if (coption.Selected == true)
                                                                                {
                                                                                    selected = "checked";
                                                                                }
                                                                                else
                                                                                {
                                                                                    selected = "";
                                                                                }

                                                                                chkoptions += "<div class='custom-control custom-checkbox custom-control-inline'><input class='custom-control-input' " + priviledgeAlt + this.GenerateValidationData(ElementBody) + " type= 'checkbox' " + selected + " value = '" + coption.Value + "' name = 'elm" + ElementBody.ElementId + "' id='elm" + coption.Value + "'><label class='custom-control-label' for='elm" + coption.Value + "'>" + coption.Text + "</label></div>";
                                                                            }
                                                                        }

                                                                        elms = "<div class='mb-2'>" + ((this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel) + "</div><div>" + chkoptions + "<span class=\"text-danger field-validation-valid\" data-valmsg-for=\"elm" + ElementBody.ElementId + "\" data-valmsg-replace=\"true\"></span></div>";
                                                                        break;
                                                                    case "media":
                                                                        elementstr = ElementBody.Template;
                                                                        string elementidside = ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]";
                                                                        string media = "<div class='thumbs add fileuploader__input'><button type='button' class='btn fileuploader__input-button'" + priviledgeAlt + " data-media='{OnInit:function(){this.size=\"modal-lg\";this.button.cancel=\"Cancel\";this.button.insert=\"Insert Media\";this.open();},OnInsert:function(e){Cicero.Form.InsertImages(e , \"" + elementidside + "\")},OnCancelled:function(){this.close();}}'><span>Browse</span></button></div>";

                                                                        if (ElementBody.Images != null && ElementBody.Images.Count > 0)
                                                                        {
                                                                            media += "<div class='fileuploader__list'>";
                                                                            media += "<ul class='fileuploader__items'>";
                                                                            foreach (var image in ElementBody.Images)
                                                                            {
                                                                                switch (System.IO.Path.GetExtension(image.Url))
                                                                                {
                                                                                    case ".pdf":
                                                                                        media = "<li class=\"thumbs fileuploader__item file-type file-type--pdf\"><a class='fileuploader-item-inner' target='_blank' href=\"javascript: void(0)\"><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src=\"/uploads/pdf.png\"></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>" + media;
                                                                                        break;
                                                                                    case ".doc":
                                                                                        media = "<li class=\"thumbs fileuploader__item file-type file-type--doc\"><a class='fileuploader-item-inner' target='_blank' href=\"javascript: void(0)\"><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src=\"/uploads/doc.png\"></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>" + media;
                                                                                        break;
                                                                                    case ".docx":
                                                                                        media = "<li class=\"thumbs fileuploader__item file-type file-type--doc\"><a class='fileuploader-item-inner' target='_blank' href=\"javascript: void(0)\"><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src=\"/uploads/doc.png\"></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>" + media;
                                                                                        break;
                                                                                    case ".txt":
                                                                                        media = "<li class=\"thumbs fileuploader__item file-type file-type--doc\"><a class='fileuploader-item-inner' target='_blank' href=\"javascript: void(0)\"><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src=\"/uploads/doc.png\"></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>" + media;
                                                                                        break;
                                                                                    default:
                                                                                        media = "<li class=\"thumbs fileuploader__item file-type file-type--image\"><a class='pop fileuploader-item-inner' href=\"javascript: void(0)\"><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src=\"/uploads/" + image.Url + "\"></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class=\"fileuploader__icon-remove remixicon-close-circle-fill\" aria-hidden=\"true\"></i></button></div></a><input type=\"hidden\" name=\"" + ((this.Side == "frontend") ? ElementBody.FrontendId : ElementBody.BackendId) + "[]\" value=\"" + image.Url + "\"></li>" + media;
                                                                                        break;
                                                                                }
                                                                            }
                                                                            media += "</ul>";
                                                                            media += "</div>";
                                                                        }
                                                                        media += "<span class=\"alert alert-danger field-validation-valid\" data-valmsg-for=\"" + elementidside + "\" data-valmsg-replace=\"true\"></span>";
                                                                        elms = string.Format(elementstr, ((this.Side == "frontend") ? ElementBody.FrontendClass : ElementBody.BackendClass), media);
                                                                        break;
                                                                    default:
                                                                        break;
                                                                }
                                                                //string elms = string.Format(elementstr, attr);
                                                                element = element + string.Format(ElementBody.WrapperTemplate, "", elms);

                                                                if (string.IsNullOrWhiteSpace(element))
                                                                {
                                                                    element = elms;
                                                                }
                                                            }


                                                        }
                                                    }
                                                }
                                                var columnstr = (this.Side == "frontend") ? string.Format(strcolumn, ColumnBody.FrontendClass, element, ColumnBody.ElementId) : string.Format(strcolumn, ColumnBody.BackendClass, element, ColumnBody.ElementId);
                                                column = column + columnstr;
                                            }

                                        }
                                    }
                                    string rowstr = (this.Side == "frontend") ? string.Format(strrow, RowBody.FrontendClass, column, RowBody.ElementId) : string.Format(strrow, RowBody.BackendClass, column, RowBody.ElementId);
                                    row = row + rowstr;
                                }

                            }
                        }

                        string tabstr = "";

                        if (this.FormData.Tab.IndexOf(TabBody) == 0)
                        {
                            tabstr = (this.Side == "frontend") ? string.Format(strtab, TabBody.FrontendClass + " show active", TabBody.FrontendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'") : string.Format(strtab, TabBody.BackendClass + " show active", TabBody.BackendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'");

                        }
                        else
                        {
                            tabstr = (this.Side == "frontend") ? string.Format(strtab, TabBody.FrontendClass, TabBody.FrontendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'") : string.Format(strtab, TabBody.BackendClass, TabBody.BackendId, row, "role='tabpanel' aria-labelledby='" + tablabel + "-tab'");

                        }

                        this.Collection = this.Collection + tabstr;
                    }

                }

                //static tab class
                tab = "<ul class='nav nav-tabs nav-tabs-custom px-3 border-bottom' role='tablist'>" + tab + "</ul>";
                if (this.Side == "frontend")
                    this.Collection = "<div class='nav-tabs-bg'>" + tab + "</div>"
                    + "<div class='container'>"
                    + "<div class='form-builder-header'><h2>Home and Contents Claim Form</h2><p class='text-muted'>Please make sure this claim form is completed clearly and in full to ensure the correct assessment of your claim.</p></div>" //.form-builder-header
                    + "<div class='form-builder my-5'><div class='card'><div class='card-body'><div class='tab-content tab-content-custom' id='myTabContent'>" + this.Collection + "</div></div>";
                // Note: Above .container, .form-builder and .card class's closing is located in C:\Projects\Cicero\Cicero\Themes\Blue\Form\Edit.cshtml file.
                else
                    this.Collection = tab + "<div class ='tab-content'>" + this.Collection + "</div>";
            }
            catch (Exception ex)
            {

            }

            return this.Collection;
        }

        public string RenderForView(dynamic formData = null, string sides = "backend")
        {
            try
            {
                if (this.commonService == null) this.commonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
                string div = string.Empty;

                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);

                var data = new List<ClaimDetailHeaderData>();

                foreach (Elements.Tab TabBody in this.FormData.Tab)
                {
                    var heading = TabBody.FrontendLabel;
                    var detailData = new List<ClaimDetailData>();
                    bool temp = (this.Side == "frontend") ? TabBody.FrontendVisible : TabBody.BackendVisible;

                    if (temp == true)
                    {

                        string row = string.Empty;
                        if (TabBody.Row != null && TabBody.Row.Count() > 0)
                        {
                            foreach (Row RowBody in TabBody.Row)
                            {
                                bool rowVisible = (this.Side == "frontend") ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                if (rowVisible)
                                {
                                    string column = string.Empty;
                                    //this.Collection= this.Collection+TabBody.
                                    if (RowBody.Column != null && RowBody.Column.Count() > 0)
                                    {
                                        foreach (Elements.Column ColumnBody in RowBody.Column)
                                        {
                                            bool ColVisible = (this.Side == "frontend") ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                            if (ColVisible)
                                            {
                                                if (ColumnBody.Element != null && ColumnBody.Element.Count() > 0)
                                                {
                                                    foreach (dynamic ElementBody in ColumnBody.Element)
                                                    {

                                                        if (ElementBody != null)
                                                        {
                                                            bool EleVisible = (this.Side == "frontend") ? ElementBody.FrontendVisible : ElementBody.BackendVisible;
                                                            if (EleVisible)
                                                            {


                                                                //replace with another logic for view
                                                                #region priviledge
                                                                string priviledge = "data";
                                                                // if (!EleVisible) { priviledge = "hidden"; priviledgeAlt = "hidden"; }
                                                                if (roleid != " ")
                                                                {
                                                                    if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                                    {

                                                                        foreach (var permission in ElementBody.Permissions)
                                                                        {
                                                                            if (permission.RoleId == roleid)
                                                                            {
                                                                                if (permission.Read == false && permission.Write == false)
                                                                                {
                                                                                    priviledge = "hidden";
                                                                                }
                                                                                else
                                                                                {
                                                                                    priviledge = "";
                                                                                }
                                                                            }

                                                                        }
                                                                        if (priviledge == "data")
                                                                        {
                                                                            priviledge = "hidden";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    priviledge = "";
                                                                }
                                                                //till here
                                                                #endregion
                                                                string elementstr = string.Empty;

                                                                var label = string.Empty;// (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;
                                                                if (ElementBody.GetType().Name.ToLower() != "heading")
                                                                {
                                                                    if (priviledge != "hidden")
                                                                    {
                                                                        detailData.Add(new ClaimDetailData
                                                                        {
                                                                            Title = ElementBody.FrontendLabel
                                                                        });
                                                                    }

                                                                }
                                                            }

                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }


                        data.Add(new ClaimDetailHeaderData
                        {
                            Header = heading,
                            DetailDatas = detailData
                        });
                    }

                }

                Console.WriteLine(data);

            }
            catch (Exception ex)
            {

            }
            return "";
        }

        public async Task<string> RenderView2(dynamic jobjData, string sides = "backend")
        {
            try
            {
                if (this.userService == null) this.userService = this.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                var checkRole = userService.UserHasPolicy();
                if (checkRole == "backend" || userService.IsSuperAdmin().Result == true)
                {
                    this.Side = "backend";
                }
                JObject Data = JObject.Parse(jobjData.ToString());
                if (this.commonService == null) this.commonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;

                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);
                var isSuperAdmin = await commonService.IsSuperAdmin();
                var count = 0;
                var tabCount = 0;
                foreach (Elements.Tab TabBody in this.FormData.Tab)
                {
                    if (count == 0)
                    {
                        bool temp = (this.Side == "frontend") ? TabBody.FrontendVisible : TabBody.BackendVisible;
                        var checkPermission = new Element.Permission();
                        if (TabBody.Permissions != null)
                        {
                            checkPermission = TabBody.Permissions.Where(x => x.RoleId == roleid).FirstOrDefault();
                        }
                        if (isSuperAdmin || (checkPermission != null && checkPermission.Read == true && temp == true))
                        {

                            string tabid = (this.Side == "frontend") ? TabBody.FrontendId : TabBody.BackendId;
                            string tablabel = (this.Side == "frontend") ? TabBody.FrontendLabel : TabBody.BackendLabel;


                            string row = string.Empty;
                            if (TabBody.Row != null && TabBody.Row.Count() > 0)
                            {
                                var header = 0;
                                var targetIdLists = new List<TargetOptionModel>();
                                var selectBoxTargets = new List<string>();
                                var headerText = string.Empty;
                                foreach (Row RowBody in TabBody.Row)
                                {
                                    bool rowVisible = (this.Side == "frontend") ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                    if (rowVisible)
                                    {
                                        var lastElement = TabBody.Row.Last();
                                        var RepeatItemData = targetIdLists.Where(x => x.TargetId == RowBody.ElementId).FirstOrDefault();
                                        var repeat = 0;
                                        var isRadioGroupChild = false;
                                        var repeatItemLists = new List<string>();
                                        var radioGroupChild = new List<string>();
                                        if (RepeatItemData != null)
                                        {
                                            repeat = RepeatItemData.Count;
                                            isRadioGroupChild = RepeatItemData.IsRadioGroupChild;
                                        }

                                        var checkTargetOption = targetIdLists.Where(x => x.TargetId == RowBody.ElementId).FirstOrDefault();
                                        if (checkTargetOption != null)
                                        {
                                            if (checkTargetOption.ShowHide == "false")
                                            {
                                                continue;
                                            }
                                        }


                                        // string strrow = RowBody.Template;
                                        string column = string.Empty;
                                        var countRepeat = 0;
                                        var tavalDatasList = new List<string>();
                                        //this.Collection= this.Collection+TabBody.
                                        if (RowBody.Column != null && RowBody.Column.Count() > 0)
                                        {
                                            foreach (Elements.Column ColumnBody in RowBody.Column)
                                            {
                                                bool ColVisible = (this.Side == "frontend") ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                                if (ColVisible)
                                                {
                                                    string element = "";
                                                    //   string strcolumn = ColumnBody.Template;
                                                    //this.Collection= this.Collection+TabBody.
                                                    if (ColumnBody.Element != null && ColumnBody.Element.Count() > 0)
                                                    {

                                                        foreach (dynamic ElementBody in ColumnBody.Element)
                                                        {

                                                            if (ElementBody != null)
                                                            {
                                                                bool EleVisible = (this.Side == "frontend") ? ElementBody.FrontendVisible : ElementBody.BackendVisible;
                                                                if (EleVisible)
                                                                {
                                                                    //replace with another logic for view

                                                                    string priviledge = "";
                                                                    if (roleid != " ")
                                                                    {
                                                                        if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                                        {

                                                                            foreach (var permission in ElementBody.Permissions)
                                                                            {
                                                                                if (permission.RoleId == roleid)
                                                                                {
                                                                                    if (permission.Read == false && permission.Write == false)
                                                                                    {
                                                                                        priviledge = "hidden";
                                                                                    }

                                                                                }

                                                                            }
                                                                        }
                                                                    }
                                                                    //till here
                                                                    string elementstr = string.Empty;
                                                                    string taval = "";
                                                                    if (!(priviledge == "hidden"))
                                                                    {
                                                                        if (Data["elm" + ElementBody.ElementId] != null)
                                                                        {
                                                                            if (Data["elm" + ElementBody.ElementId].Value != null)
                                                                            {
                                                                                taval = Data["elm" + ElementBody.ElementId].Value;
                                                                            }
                                                                        }

                                                                        var label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;


                                                                        if (repeat == 0)
                                                                        {
                                                                            elementstr = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div>";
                                                                            if (ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                            {
                                                                                var random = new Random();
                                                                                var randomValue = random.Next();
                                                                                var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                    "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                    "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                elementstr = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            var random = new Random();
                                                                            var accordionId = "exampleAccordion" + random.Next();
                                                                            for (int i = 0; i < repeat; i++)
                                                                            {
                                                                                if (Data["elm" + ElementBody.ElementId] != null)
                                                                                {
                                                                                    var tavalData = Data["elm" + ElementBody.ElementId];
                                                                                    if (tavalData.Value != "Null")
                                                                                    {
                                                                                        taval = (tavalData[i.ToString()] == null ? "" : tavalData[i.ToString()]);
                                                                                    }
                                                                                }

                                                                                var accordionUl = "<ul class='case__item-child case__accordion accordion' id=" + accordionId + ">";
                                                                                var accordianData = "<li class='card'><div class='card-header' id='heading" + (i + 1) + "' data-toggle='collapse' data-target='#collapse" + (i + 1) + "' aria-expanded='false' aria-controls='collapse" + (i + 1) + "'>" +
                    "<h5 class='mb-0'>Item" + (i + 1) + "</h5></div><div id = 'collapse" + (i + 1) + "' class='collapse' aria-labelledby='heading" + (i + 1) + "' data-parent='#" + accordionId + "'>" +
                    "<div class='card-body'><ul class='case__list'>";
                                                                                var elmType = ElementBody.GetType().Name.ToLower();
                                                                                var descriptionClass = (elmType == "textarea" || elmType == "textbox") ? "case__item--description" : "";
                                                                                var liData = "<li class='case__item " + descriptionClass + "'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";

                                                                                if (ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                                {
                                                                                    var random2 = new Random();
                                                                                    var randomValue = random2.Next();
                                                                                    var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                    var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                        "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                        "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                    liData = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                                }

                                                                                if (countRepeat == 0)
                                                                                {
                                                                                    if (i == 0)
                                                                                    {
                                                                                        repeatItemLists.Add(accordionUl + accordianData + liData);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        repeatItemLists.Add(accordianData + liData);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    var lastelm = RowBody.Column.Last();
                                                                                    repeatItemLists[i] += liData;
                                                                                }


                                                                            }


                                                                            countRepeat = 1;

                                                                        }


                                                                        if (isRadioGroupChild)
                                                                        {
                                                                            var elmType = ElementBody.GetType().Name.ToLower();
                                                                            var descriptionClass = (elmType == "textarea" || elmType == "textbox") ? "case__item--description" : "";
                                                                            var childData = "<li class='case__item " + descriptionClass + "'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                            if (ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                            {
                                                                                var random = new Random();
                                                                                var randomValue = random.Next();
                                                                                var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                    "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                    "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                childData = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                            }

                                                                            if (radioGroupChild.Count == 0)
                                                                            {
                                                                                radioGroupChild.Add("<ul class='case__item-child'>" + childData);
                                                                            }
                                                                            else
                                                                            {
                                                                                radioGroupChild.Add(childData);
                                                                            }

                                                                        }

                                                                        string elms = "";
                                                                        if (header == 0 && ElementBody.GetType().Name.ToLower() != "heading")
                                                                        {
                                                                            var ul = "<div class='case__list-wrapper'><ul class='list-unstyled case__list'>";
                                                                            row = row + ul;
                                                                            header = 1;
                                                                        }
                                                                        switch (ElementBody.GetType().Name.ToLower())
                                                                        {
                                                                            //case "":
                                                                            //    break;
                                                                            #region paragraph
                                                                            case "paragraph":
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region textarea
                                                                            case "textarea":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region heading
                                                                            case "heading":
                                                                                headerText = ElementBody.HeaderText;
                                                                                if (header == 0)
                                                                                {
                                                                                    var ul = "<div class='case__list-wrapper'><ul class='list-unstyled case__list'>";
                                                                                    elms = "<" + "h5" + " class = '" + "claim-detail-header" + "'> " + ElementBody.HeaderText + " </" + "h5" + ">"
                                                                                    + ul;

                                                                                    header = 1;
                                                                                }
                                                                                break;
                                                                            #endregion

                                                                            #region textbox
                                                                            case "textbox":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region number
                                                                            case "number":
                                                                                List<Number.TargetOption> ntargets = (List<Number.TargetOption>)ElementBody.TargetOptions;
                                                                                if (ntargets != null)
                                                                                {
                                                                                    foreach (var targetoption in ntargets)
                                                                                    {
                                                                                        if (targetoption.TargetId != "0")
                                                                                        {
                                                                                            var checkTargetIdExist = targetIdLists.Where(x => x.TargetId == targetoption.TargetId).ToList();
                                                                                            var targetRow = TabBody.Row.Where(x => x.ElementId == targetoption.TargetId).FirstOrDefault();
                                                                                            var targetColumn = targetRow.Column.FirstOrDefault();
                                                                                            var targetElement = targetColumn.Element.Where(x => x.Type != "heading").FirstOrDefault();
                                                                                            if (Data["elm" + targetElement.ElementId] != null)
                                                                                            {
                                                                                                dynamic tavalDatas = Data["elm" + targetElement.ElementId];
                                                                                                if (tavalDatas.Value != "Null")
                                                                                                {
                                                                                                    var tavalValues = (JToken)tavalDatas;
                                                                                                    var countItem = tavalValues.Count();
                                                                                                    if (taval != countItem.ToString())
                                                                                                    {
                                                                                                        taval = countItem.ToString();
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (checkTargetIdExist.Count > 0)
                                                                                            {
                                                                                                foreach (var item in checkTargetIdExist)
                                                                                                {
                                                                                                    item.ShowHide = (taval == "" ? "false" : "true");
                                                                                                    item.Count = Convert.ToInt32((taval == "" ? "0" : taval));
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                targetIdLists.Add(new TargetOptionModel
                                                                                                {
                                                                                                    ShowHide = (taval == "" ? "false" : "true"),
                                                                                                    TargetId = targetoption.TargetId,
                                                                                                    Count = Convert.ToInt32((taval == "" ? "0" : taval))
                                                                                                });
                                                                                            }

                                                                                        }

                                                                                    }
                                                                                }

                                                                                if (ntargets != null && ntargets.Where(x => x.TargetId == "0").Select(x => x.TargetId).ToList().Count == 0 && !(ElementBody.IsCurrency) && taval != "")
                                                                                {
                                                                                    elementstr = "<li class='case__item case__item--has-child case__item--has-accordian'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div>";
                                                                                }
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region selectbox
                                                                            case "selectbox":
                                                                                if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                                {
                                                                                    var option = new SelectBox.SelectOption();
                                                                                    foreach (var soption in ElementBody.SelectOptions)
                                                                                    {
                                                                                        var soptionValue = soption.Value;
                                                                                        if (soptionValue != null)
                                                                                        {
                                                                                            soptionValue = soptionValue.Trim();
                                                                                            if (soptionValue == taval.Trim())
                                                                                            {
                                                                                                List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                                List<SelectBox.SelectOption> selectOptions = (List<SelectBox.SelectOption>)ElementBody.SelectOptions;
                                                                                                if (targets != null)
                                                                                                {
                                                                                                    var result = targets.Where(x => x.SelectId == soption.Value).ToList();
                                                                                                    if (selectOptions != null)
                                                                                                    {
                                                                                                        option = selectOptions.Where(x => x.Value == soption.Value).FirstOrDefault();
                                                                                                    }
                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        if (option.ToggleOptions)
                                                                                                        {
                                                                                                            targetIdLists.Add(new TargetOptionModel
                                                                                                            {
                                                                                                                ShowHide = "true",
                                                                                                                TargetId = targetoption.TargetId
                                                                                                            });

                                                                                                            selectBoxTargets.Add(targetoption.TargetId);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            targetIdLists.Add(new TargetOptionModel
                                                                                                            {
                                                                                                                ShowHide = targetoption.ShowHide,
                                                                                                                TargetId = targetoption.TargetId
                                                                                                            });

                                                                                                            if (targetoption.ShowHide == "true")
                                                                                                            {
                                                                                                                selectBoxTargets.Add(targetoption.TargetId);
                                                                                                            }
                                                                                                        }

                                                                                                    }

                                                                                                    if (option.ToggleOptions)
                                                                                                    {
                                                                                                        var selectBoxHmtl = "<select hidden>";
                                                                                                        var optionsHtml = "";

                                                                                                        foreach (var opt in selectOptions)
                                                                                                        {
                                                                                                            var targetResult = targets.Where(x => x.SelectId == opt.Value).ToList();
                                                                                                            string targetHtml = "";
                                                                                                            string toption = "";
                                                                                                            string taction = "";

                                                                                                            foreach (var targetoption in targetResult)
                                                                                                            {
                                                                                                                if (result[0] == targetoption)
                                                                                                                {
                                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                                    taction = targetoption.ShowHide;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                                }

                                                                                                            }

                                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                                            {
                                                                                                                targetHtml = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                            }
                                                                                                            optionsHtml = optionsHtml + "<option " + targetHtml + ">" + opt.Text + "</option>";
                                                                                                        }

                                                                                                        selectBoxHmtl = selectBoxHmtl + optionsHtml + "</select>";

                                                                                                        elementstr = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data which'>" + selectBoxHmtl + "" + "</div></div>";
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }


                                                                                    }

                                                                                    elms = elementstr;

                                                                                }


                                                                                break;
                                                                            #endregion

                                                                            #region multiselectbox
                                                                            case "multiselectbox":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region radiogroup
                                                                            case "radiogroup":
                                                                                if (ElementBody.RadioOptions != null && ElementBody.RadioOptions.Count > 0)
                                                                                {
                                                                                    string selected = string.Empty;
                                                                                    foreach (var roption in ElementBody.RadioOptions)
                                                                                    {
                                                                                        var roptionValue = roption.Value;
                                                                                        if (roptionValue != null)
                                                                                        {
                                                                                            roptionValue = roptionValue.Trim();
                                                                                        }
                                                                                        if (!string.IsNullOrWhiteSpace(ElementBody.FrontendClass) && !ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                                        {
                                                                                            if (roptionValue == taval.Trim())
                                                                                            {
                                                                                                List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                                if (targets != null)
                                                                                                {
                                                                                                    var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                    var showChild = result.Where(x => x.ShowHide == "true" && x.TargetId != "0").ToList();

                                                                                                    if (showChild.Count > 0)
                                                                                                    {
                                                                                                        elementstr = "<li class='case__item case__item--has-child'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div>";
                                                                                                    }

                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        targetIdLists.Add(new TargetOptionModel
                                                                                                        {
                                                                                                            ShowHide = targetoption.ShowHide,
                                                                                                            TargetId = targetoption.TargetId,
                                                                                                            IsRadioGroupChild = true
                                                                                                        });
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                            if (targets != null)
                                                                                            {
                                                                                                if ((taval.Trim().ToLower() == "true" ? "Yes" : "No") == roptionValue)
                                                                                                {
                                                                                                    var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                    var showChild = result.Where(x => x.ShowHide == "true" && x.TargetId != "0").ToList();

                                                                                                    if (showChild.Count > 0)
                                                                                                    {
                                                                                                        var random = new Random();
                                                                                                        var randomValue = random.Next();
                                                                                                        var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                                        var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                                            "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                                            "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                                        elementstr = "<li class='case__item case__item--has-child'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                                                    }

                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        targetIdLists.Add(new TargetOptionModel
                                                                                                        {
                                                                                                            ShowHide = targetoption.ShowHide,
                                                                                                            TargetId = targetoption.TargetId,
                                                                                                            IsRadioGroupChild = true
                                                                                                        });
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }

                                                                                    }
                                                                                }

                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region checkboxgroup
                                                                            case "checkboxgroup":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region media
                                                                            case "media":
                                                                                if (this.mediaService == null) this.mediaService = this.HttpContext.RequestServices.GetService(typeof(IMediaService)) as IMediaService;
                                                                                var div = string.Empty;
                                                                                if (Data["Media" + ElementBody.ElementId] != null && Data["Media" + ElementBody.ElementId].Value != "Null" && Data["Media" + ElementBody.ElementId].Value != "")
                                                                                {
                                                                                    var mediaDatas = Data["Media" + ElementBody.ElementId];
                                                                                    var mediaIds = new List<int>();
                                                                                    foreach (var item in mediaDatas)
                                                                                    {
                                                                                        mediaIds.Add(Convert.ToInt32(item.Value));
                                                                                    }
                                                                                    var filesData = mediaService.GetImagesByIds(mediaIds);
                                                                                    var li = string.Empty;
                                                                                    foreach (var item in filesData)
                                                                                    {
                                                                                        var extension = System.IO.Path.GetExtension(item.Url).ToLower();
                                                                                        var imgSrc = string.Empty;
                                                                                        imgSrc = "/uploads/" + item.Url;

                                                                                        if (extension == ".pdf")
                                                                                        {
                                                                                            li += "<li class='thumbs fileuploader__item file-type file-type--pdf'><a class='fileuploader-item-inner' target='_blank' href='" + imgSrc + "'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/images/pdf.png' data-imgtitle='" + item.Title + "'></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class='fileuploader__icon-remove remixicon-close-circle-fill' aria-hidden='true'></i></button></div></a><input type='hidden' name='images[]' value='80'></li>";
                                                                                        }
                                                                                        else if (extension == ".doc" || extension == ".docx" || extension == ".txt")
                                                                                        {
                                                                                            li += "<li class='thumbs fileuploader__item file-type file-type--doc'><a class='fileuploader-item-inner' target='_blank' href='" + imgSrc + "'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/images/doc.png' data-imgtitle='" + item.Title + "'></div></div></li>";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            li += "<li class='thumbs fileuploader__item file-type file-type--image'><a class='pop fileuploader-item-inner' href='javascript:void(0)'><div class=''><div class='fileuploader__item-image'><img src='" + imgSrc + "' data-imgtitle='" + item.Title + "'></div></div></a></li>";

                                                                                        }


                                                                                    }

                                                                                    div = "<div class='fileuploader__list'><ul class='fileuploader__items'>" + li + "</ul></div>";
                                                                                    // JObject array = mediaDatas;
                                                                                }
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + div + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion
                                                                            default:
                                                                                break;
                                                                        }

                                                                        //string elms = string.Format(elementstr, attr);
                                                                        element = element + elms;//string.Format(ElementBody.WrapperTemplate, "", elms);
                                                                    }

                                                                    tavalDatasList.Add(taval);
                                                                }



                                                            }
                                                        }
                                                    }
                                                    column = column + element;
                                                }
                                                countRepeat++;
                                            }
                                        }

                                        //var RepeatItemData = targetIdLists.Where(x => x.TargetId == RowBody.ElementId).FirstOrDefault();
                                        //if(RepeatItemData != null)
                                        //{
                                        //    var repeat = RepeatItemData.Count;

                                        //}


                                        if (repeat > 0)
                                        {
                                            for (int i = 0; i < repeatItemLists.Count; i++)
                                            {
                                                repeatItemLists[i] += "</ul></div></div></li>";
                                            }
                                            row += string.Join(" ", repeatItemLists) + "</ul></li>" + "</ul></div></div></li></ul>";
                                        }
                                        else if (isRadioGroupChild)
                                        {
                                            row += string.Join(" ", radioGroupChild) + "</ul></li>";
                                        }
                                        else
                                        {
                                            if (selectBoxTargets.Contains(RowBody.ElementId))
                                            {
                                                if (tavalDatasList.Where(x => x == "").ToList().Count == tavalDatasList.Count)
                                                {
                                                    column = "";
                                                }
                                                else
                                                {
                                                    var random = new Random();
                                                    var id = random.Next();
                                                    var accordionId = "exampleAccordion" + random.Next();
                                                    var accordionUl = "<ul class='case__item-child case__accordion accordion' id=" + accordionId + " data-rowId=" + RowBody.ElementId + ">";
                                                    var accordianData = "<li class='card'><div class='card-header' id='heading" + id + "' data-toggle='collapse' data-target='#collapse" + id + "' aria-expanded='false' aria-controls='collapse" + id + "'>" +
        "<h5 class='mb-0'>" + headerText + "</h5></div><div id = 'collapse" + id + "' class='collapse' aria-labelledby='heading" + id + "' data-parent='#" + accordionId + "'>" +
        "<div class='card-body'><ul class='case__list'>";
                                                    if (column.Contains("case__item--has-accordian"))
                                                    {
                                                        column = accordionUl + accordianData + column;
                                                    }
                                                    else
                                                    {
                                                        column = accordionUl + accordianData + column + "</ul></div></div></li></ul>";
                                                    }
                                                }
                                            }
                                            row = row + column;
                                        }

                                    }

                                }

                            }

                            var last = this.FormData.Tab.Last();
                            row += "</div></ul>";
                            if (tabCount == 0)
                            {
                                row = "<div class='row'><div class='col-lg-6 mb-4'><div class='card card--case'><div class='card-body'><div class='claim-detail-wrapper'>" + row + "</div></div></div></div>";

                                if (TabBody == last)
                                {
                                    row += "</div>";
                                }
                                tabCount++;
                            }
                            else
                            {
                                row = "<div class='col-lg-6 mb-4'><div class='card card--case'><div class='card-body'><div class='claim-detail-wrapper'>" + row + "</div></div></div></div></div>";
                                tabCount--;
                            }
                            this.Collection = this.Collection + row;

                        }
                    }

                    count = 0;


                }

            }
            catch (Exception ex)
            {

            }
            this.Collection += "<input hidden id='isFrontEnd' value='" + this.Side + "'/>";

            return this.Collection;
        }

        public async Task<string> RenderView3(dynamic jobjData, string sides = "backend")
        {
            try
            {
                if (this.userService == null) this.userService = this.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
                var checkRole = userService.UserHasPolicy();
                if (checkRole == "backend" || userService.IsSuperAdmin().Result == true)
                {
                    this.Side = "backend";
                }
                JObject Data = JObject.Parse(jobjData.ToString());
                if (this.commonService == null) this.commonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;

                string loggeduser = commonService.getLoggedInUserId();
                string roleid = commonService.GetRoleIdByUserId(loggeduser);
                var isSuperAdmin = await commonService.IsSuperAdmin();
                var count = 0;
                var tabCount = 0;
                foreach (Elements.Tab TabBody in this.FormData.Tab)
                {
                    if (count == 0)
                    {
                        bool temp = (this.Side == "frontend") ? TabBody.FrontendVisible : TabBody.BackendVisible;
                        var checkPermission = new Element.Permission();
                        if (TabBody.Permissions != null)
                        {
                            checkPermission = TabBody.Permissions.Where(x => x.RoleId == roleid).FirstOrDefault();
                        }
                        if (isSuperAdmin || (checkPermission != null && checkPermission.Read == true && temp == true))
                        {

                            string tabid = (this.Side == "frontend") ? TabBody.FrontendId : TabBody.BackendId;
                            string tablabel = (this.Side == "frontend") ? TabBody.FrontendLabel : TabBody.BackendLabel;


                            string row = string.Empty;
                            if (TabBody.Row != null && TabBody.Row.Count() > 0)
                            {
                                var header = 0;
                                var targetIdLists = new List<TargetOptionModel>();
                                var selectBoxTargets = new List<string>();
                                var headerText = string.Empty;
                                foreach (Row RowBody in TabBody.Row)
                                {
                                    bool rowVisible = (this.Side == "frontend") ? RowBody.FrontendVisible : RowBody.BackendVisible;
                                    if (rowVisible)
                                    {
                                        var lastElement = TabBody.Row.Last();
                                        var RepeatItemData = targetIdLists.Where(x => x.TargetId == RowBody.ElementId).FirstOrDefault();
                                        var repeat = 0;
                                        var isRadioGroupChild = false;
                                        var repeatItemLists = new List<string>();
                                        var radioGroupChild = new List<string>();
                                        if (RepeatItemData != null)
                                        {
                                            repeat = RepeatItemData.Count;
                                            isRadioGroupChild = RepeatItemData.IsRadioGroupChild;
                                        }

                                        var checkTargetOption = targetIdLists.Where(x => x.TargetId == RowBody.ElementId).FirstOrDefault();
                                        if (checkTargetOption != null)
                                        {
                                            if (checkTargetOption.ShowHide == "false")
                                            {
                                                continue;
                                            }
                                        }


                                        // string strrow = RowBody.Template;
                                        string column = string.Empty;
                                        var countRepeat = 0;
                                        var tavalDatasList = new List<string>();
                                        //this.Collection= this.Collection+TabBody.
                                        if (RowBody.Column != null && RowBody.Column.Count() > 0)
                                        {
                                            foreach (Elements.Column ColumnBody in RowBody.Column)
                                            {
                                                bool ColVisible = (this.Side == "frontend") ? ColumnBody.FrontendVisible : ColumnBody.BackendVisible;
                                                if (ColVisible)
                                                {
                                                    string element = "";
                                                    //   string strcolumn = ColumnBody.Template;
                                                    //this.Collection= this.Collection+TabBody.
                                                    if (ColumnBody.Element != null && ColumnBody.Element.Count() > 0)
                                                    {

                                                        foreach (dynamic ElementBody in ColumnBody.Element)
                                                        {

                                                            if (ElementBody != null)
                                                            {
                                                                bool EleVisible = (this.Side == "frontend") ? ElementBody.FrontendVisible : ElementBody.BackendVisible;
                                                                if (EleVisible)
                                                                {
                                                                    //replace with another logic for view

                                                                    string priviledge = "";
                                                                    if (roleid != " ")
                                                                    {
                                                                        if (ElementBody.Permissions != null && ElementBody.Permissions.Count > 0)
                                                                        {

                                                                            foreach (var permission in ElementBody.Permissions)
                                                                            {
                                                                                if (permission.RoleId == roleid)
                                                                                {
                                                                                    if (permission.Read == false && permission.Write == false)
                                                                                    {
                                                                                        priviledge = "hidden";
                                                                                    }

                                                                                }

                                                                            }
                                                                        }
                                                                    }
                                                                    //till here
                                                                    string elementstr = string.Empty;
                                                                    string taval = "";
                                                                    if (!(priviledge == "hidden"))
                                                                    {
                                                                        if (Data["elm" + ElementBody.ElementId] != null)
                                                                        {
                                                                            if (Data["elm" + ElementBody.ElementId].Value != null)
                                                                            {
                                                                                taval = Data["elm" + ElementBody.ElementId].Value;
                                                                            }
                                                                        }

                                                                        var label = (this.Side == "frontend") ? ElementBody.FrontendLabel : ElementBody.BackendLabel;


                                                                        if (repeat == 0)
                                                                        {
                                                                            elementstr = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div>";
                                                                            if (ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                            {
                                                                                var random = new Random();
                                                                                var randomValue = random.Next();
                                                                                var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                    "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                    "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                elementstr = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            var random = new Random();
                                                                            var accordionId = "exampleAccordion" + random.Next();
                                                                            for (int i = 0; i < repeat; i++)
                                                                            {
                                                                                if (Data["elm" + ElementBody.ElementId] != null)
                                                                                {
                                                                                    var tavalData = Data["elm" + ElementBody.ElementId];
                                                                                    if (tavalData.Value != "Null")
                                                                                    {
                                                                                        taval = (tavalData[i.ToString()] == null ? "" : tavalData[i.ToString()]);
                                                                                    }
                                                                                }

                                                                                var accordionUl = "<ul class='case__item-child case__accordion accordion' id=" + accordionId + ">";
                                                                                var accordianData = "<li class='card'><div class='card-header' id='heading" + (i + 1) + "' data-toggle='collapse' data-target='#collapse" + (i + 1) + "' aria-expanded='false' aria-controls='collapse" + (i + 1) + "'>" +
                    "<h5 class='mb-0'>Item" + (i + 1) + "</h5></div><div id = 'collapse" + (i + 1) + "' class='collapse' aria-labelledby='heading" + (i + 1) + "' data-parent='#" + accordionId + "'>" +
                    "<div class='card-body'><ul class='case__list'>";
                                                                                var elmType = ElementBody.GetType().Name.ToLower();
                                                                                var descriptionClass = (elmType == "textarea" || elmType == "textbox") ? "case__item--description" : "";
                                                                                var liData = "<li class='case__item " + descriptionClass + "'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";

                                                                                if (ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                                {
                                                                                    var random2 = new Random();
                                                                                    var randomValue = random2.Next();
                                                                                    var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                    var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                        "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                        "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                    liData = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                                }

                                                                                if (countRepeat == 0)
                                                                                {
                                                                                    if (i == 0)
                                                                                    {
                                                                                        repeatItemLists.Add(accordionUl + accordianData + liData);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        repeatItemLists.Add(accordianData + liData);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    var lastelm = RowBody.Column.Last();
                                                                                    repeatItemLists[i] += liData;
                                                                                }


                                                                            }


                                                                            countRepeat = 1;

                                                                        }


                                                                        if (isRadioGroupChild)
                                                                        {
                                                                            var elmType = ElementBody.GetType().Name.ToLower();
                                                                            var descriptionClass = (elmType == "textarea" || elmType == "textbox") ? "case__item--description" : "";
                                                                            var childData = "<li class='case__item " + descriptionClass + "'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                            if (ElementBody.FrontendClass != null && ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                            {
                                                                                var random = new Random();
                                                                                var randomValue = random.Next();
                                                                                var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                    "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                    "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                childData = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                            }

                                                                            if (radioGroupChild.Count == 0)
                                                                            {
                                                                                radioGroupChild.Add("<ul class='case__item-child'>" + childData);
                                                                            }
                                                                            else
                                                                            {
                                                                                radioGroupChild.Add(childData);
                                                                            }

                                                                        }

                                                                        string elms = "";

                                                                        switch (ElementBody.GetType().Name.ToLower())
                                                                        {
                                                                            //case "":
                                                                            //    break;
                                                                            #region paragraph
                                                                            case "paragraph":
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region textarea
                                                                            case "textarea":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region heading
                                                                            case "heading":
                                                                                headerText = ElementBody.HeaderText;
                                                                                if (header == 0)
                                                                                {
                                                                                    var ul = "<div class='case__list-wrapper'><ul class='list-unstyled case__list'>";
                                                                                    elms = "<" + "h5" + " class = '" + "claim-detail-header" + "'> " + ElementBody.HeaderText + " </" + "h5" + ">"
                                                                                    + ul;

                                                                                    header = 1;
                                                                                }
                                                                                break;
                                                                            #endregion

                                                                            #region textbox
                                                                            case "textbox":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region number
                                                                            case "number":
                                                                                List<Number.TargetOption> ntargets = (List<Number.TargetOption>)ElementBody.TargetOptions;
                                                                                if (ntargets != null)
                                                                                {
                                                                                    foreach (var targetoption in ntargets)
                                                                                    {
                                                                                        if (targetoption.TargetId != "0")
                                                                                        {
                                                                                            var checkTargetIdExist = targetIdLists.Where(x => x.TargetId == targetoption.TargetId).ToList();
                                                                                            var targetRow = TabBody.Row.Where(x => x.ElementId == targetoption.TargetId).FirstOrDefault();
                                                                                            var targetColumn = targetRow.Column.FirstOrDefault();
                                                                                            var targetElement = targetColumn.Element.Where(x => x.Type != "heading").FirstOrDefault();
                                                                                            if (Data["elm" + targetElement.ElementId] != null)
                                                                                            {
                                                                                                dynamic tavalDatas = Data["elm" + targetElement.ElementId];
                                                                                                if (tavalDatas.Value != "Null")
                                                                                                {
                                                                                                    var tavalValues = (JToken)tavalDatas;
                                                                                                    var countItem = tavalValues.Count();
                                                                                                    if (taval != countItem.ToString())
                                                                                                    {
                                                                                                        taval = countItem.ToString();
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (checkTargetIdExist.Count > 0)
                                                                                            {
                                                                                                foreach (var item in checkTargetIdExist)
                                                                                                {
                                                                                                    item.ShowHide = (taval == "" ? "false" : "true");
                                                                                                    item.Count = Convert.ToInt32((taval == "" ? "0" : taval));
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                targetIdLists.Add(new TargetOptionModel
                                                                                                {
                                                                                                    ShowHide = (taval == "" ? "false" : "true"),
                                                                                                    TargetId = targetoption.TargetId,
                                                                                                    Count = Convert.ToInt32((taval == "" ? "0" : taval))
                                                                                                });
                                                                                            }

                                                                                        }

                                                                                    }
                                                                                }

                                                                                if (ntargets != null && ntargets.Where(x => x.TargetId == "0").Select(x => x.TargetId).ToList().Count == 0 && !(ElementBody.IsCurrency) && taval != "")
                                                                                {
                                                                                    elementstr = "<li class='case__item case__item--has-child case__item--has-accordian'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div>";
                                                                                }
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region selectbox
                                                                            case "selectbox":
                                                                                if (ElementBody.SelectOptions != null && ElementBody.SelectOptions.Count > 0)
                                                                                {
                                                                                    var option = new SelectBox.SelectOption();
                                                                                    foreach (var soption in ElementBody.SelectOptions)
                                                                                    {
                                                                                        var soptionValue = soption.Value;
                                                                                        if (soptionValue != null)
                                                                                        {
                                                                                            soptionValue = soptionValue.Trim();
                                                                                            if (soptionValue == taval.Trim())
                                                                                            {
                                                                                                List<SelectBox.TargetOption> targets = (List<SelectBox.TargetOption>)ElementBody.TargetOptions;
                                                                                                List<SelectBox.SelectOption> selectOptions = (List<SelectBox.SelectOption>)ElementBody.SelectOptions;
                                                                                                if (targets != null)
                                                                                                {
                                                                                                    var result = targets.Where(x => x.SelectId == soption.Value).ToList();
                                                                                                    if (selectOptions != null)
                                                                                                    {
                                                                                                        option = selectOptions.Where(x => x.Value == soption.Value).FirstOrDefault();
                                                                                                    }
                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        if (option.ToggleOptions)
                                                                                                        {
                                                                                                            targetIdLists.Add(new TargetOptionModel
                                                                                                            {
                                                                                                                ShowHide = "true",
                                                                                                                TargetId = targetoption.TargetId
                                                                                                            });

                                                                                                            selectBoxTargets.Add(targetoption.TargetId);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            targetIdLists.Add(new TargetOptionModel
                                                                                                            {
                                                                                                                ShowHide = targetoption.ShowHide,
                                                                                                                TargetId = targetoption.TargetId
                                                                                                            });

                                                                                                            if (targetoption.ShowHide == "true")
                                                                                                            {
                                                                                                                selectBoxTargets.Add(targetoption.TargetId);
                                                                                                            }
                                                                                                        }

                                                                                                    }

                                                                                                    if (option.ToggleOptions)
                                                                                                    {
                                                                                                        var selectBoxHmtl = "<select hidden>";
                                                                                                        var optionsHtml = "";

                                                                                                        foreach (var opt in selectOptions)
                                                                                                        {
                                                                                                            var targetResult = targets.Where(x => x.SelectId == opt.Value).ToList();
                                                                                                            string targetHtml = "";
                                                                                                            string toption = "";
                                                                                                            string taction = "";

                                                                                                            foreach (var targetoption in targetResult)
                                                                                                            {
                                                                                                                if (result[0] == targetoption)
                                                                                                                {
                                                                                                                    toption = "#" + targetoption.TargetId;

                                                                                                                    taction = targetoption.ShowHide;
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    toption = toption + ",#" + targetoption.TargetId;

                                                                                                                    taction = taction + "," + targetoption.ShowHide;
                                                                                                                }

                                                                                                            }

                                                                                                            if (!string.IsNullOrWhiteSpace(toption))
                                                                                                            {
                                                                                                                targetHtml = "data-target='" + toption + "' data-target-action='" + taction + "'";
                                                                                                            }
                                                                                                            optionsHtml = optionsHtml + "<option " + targetHtml + ">" + opt.Text + "</option>";
                                                                                                        }

                                                                                                        selectBoxHmtl = selectBoxHmtl + optionsHtml + "</select>";

                                                                                                        elementstr = "<li class='case__item'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data which'>" + selectBoxHmtl + "" + "</div></div>";
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }


                                                                                    }

                                                                                    elms = elementstr;

                                                                                }


                                                                                break;
                                                                            #endregion

                                                                            #region multiselectbox
                                                                            case "multiselectbox":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region radiogroup
                                                                            case "radiogroup":
                                                                                if (ElementBody.RadioOptions != null && ElementBody.RadioOptions.Count > 0)
                                                                                {
                                                                                    string selected = string.Empty;
                                                                                    foreach (var roption in ElementBody.RadioOptions)
                                                                                    {
                                                                                        var roptionValue = roption.Value;
                                                                                        if (roptionValue != null)
                                                                                        {
                                                                                            roptionValue = roptionValue.Trim();
                                                                                        }
                                                                                        if (!ElementBody.FrontendClass.ToLower().Contains("onoff"))
                                                                                        {
                                                                                            if (roptionValue == taval.Trim())
                                                                                            {
                                                                                                List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                                if (targets != null)
                                                                                                {
                                                                                                    var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                    var showChild = result.Where(x => x.ShowHide == "true" && x.TargetId != "0").ToList();

                                                                                                    if (showChild.Count > 0)
                                                                                                    {
                                                                                                        elementstr = "<li class='case__item case__item--has-child'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div>";
                                                                                                    }

                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        targetIdLists.Add(new TargetOptionModel
                                                                                                        {
                                                                                                            ShowHide = targetoption.ShowHide,
                                                                                                            TargetId = targetoption.TargetId,
                                                                                                            IsRadioGroupChild = true
                                                                                                        });
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            List<RadioGroup.TargetOption> targets = (List<RadioGroup.TargetOption>)ElementBody.TargetOptions;
                                                                                            if (targets != null)
                                                                                            {
                                                                                                if ((taval.Trim().ToLower() == "true" ? "Yes" : "No") == roptionValue)
                                                                                                {
                                                                                                    var result = targets.Where(x => x.SelectId == roption.Value).ToList();

                                                                                                    var showChild = result.Where(x => x.ShowHide == "true" && x.TargetId != "0").ToList();

                                                                                                    if (showChild.Count > 0)
                                                                                                    {
                                                                                                        var random = new Random();
                                                                                                        var randomValue = random.Next();
                                                                                                        var c = taval.Trim().ToLower() == "true" ? "checked" : "";
                                                                                                        var checkBox = "<div class='custom-control custom-switch'>" +
                                                                                                            "<input type = 'checkbox' class='custom-control-input' id='" + randomValue + "' disabled " + c + ">" +
                                                                                                            "<label class='custom-control-label' for='" + randomValue + "'></label></div> ";
                                                                                                        elementstr = "<li class='case__item case__item--has-child'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + checkBox + "</div></div>";
                                                                                                    }

                                                                                                    foreach (var targetoption in result)
                                                                                                    {
                                                                                                        targetIdLists.Add(new TargetOptionModel
                                                                                                        {
                                                                                                            ShowHide = targetoption.ShowHide,
                                                                                                            TargetId = targetoption.TargetId,
                                                                                                            IsRadioGroupChild = true
                                                                                                        });
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }

                                                                                    }
                                                                                }

                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region checkboxgroup
                                                                            case "checkboxgroup":
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + taval + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion

                                                                            #region media
                                                                            case "media":
                                                                                if (this.mediaService == null) this.mediaService = this.HttpContext.RequestServices.GetService(typeof(IMediaService)) as IMediaService;
                                                                                var div = string.Empty;
                                                                                if (Data["Media" + ElementBody.ElementId] != null && Data["Media" + ElementBody.ElementId].Value != "Null" && Data["Media" + ElementBody.ElementId].Value != "")
                                                                                {
                                                                                    var mediaDatas = Data["Media" + ElementBody.ElementId];
                                                                                    var mediaIds = new List<int>();
                                                                                    foreach (var item in mediaDatas)
                                                                                    {
                                                                                        mediaIds.Add(Convert.ToInt32(item.Value));
                                                                                    }
                                                                                    var filesData = mediaService.GetImagesByIds(mediaIds);
                                                                                    var li = string.Empty;
                                                                                    foreach (var item in filesData)
                                                                                    {
                                                                                        var extension = System.IO.Path.GetExtension(item.Url).ToLower();
                                                                                        var imgSrc = string.Empty;
                                                                                        imgSrc = "/uploads/" + item.Url;

                                                                                        if (extension == ".pdf")
                                                                                        {
                                                                                            li += "<li class='thumbs fileuploader__item file-type file-type--pdf'><a class='fileuploader-item-inner' target='_blank' href='" + imgSrc + "'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/images/pdf.png' data-imgtitle='" + item.Title + "'></div></div><div class='actions-holder'><button class='btn fileuploader__action fileuploader__action-remove' type='button' title='Remove'><i class='fileuploader__icon-remove remixicon-close-circle-fill' aria-hidden='true'></i></button></div></a><input type='hidden' name='images[]' value='80'></li>";
                                                                                        }
                                                                                        else if (extension == ".doc" || extension == ".docx" || extension == ".txt")
                                                                                        {
                                                                                            li += "<li class='thumbs fileuploader__item file-type file-type--doc'><a class='fileuploader-item-inner' target='_blank' href='" + imgSrc + "'><div class='thumbnail-holder'><div class='fileuploader__item-image'><img src='/images/doc.png' data-imgtitle='" + item.Title + "'></div></div></li>";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            li += "<li class='thumbs fileuploader__item file-type file-type--image'><a class='pop fileuploader-item-inner' href='javascript:void(0)'><div class=''><div class='fileuploader__item-image'><img src='" + imgSrc + "' data-imgtitle='" + item.Title + "'></div></div></a></li>";

                                                                                        }


                                                                                    }

                                                                                    div = "<div class='fileuploader__list'><ul class='fileuploader__items'>" + li + "</ul></div>";
                                                                                    // JObject array = mediaDatas;
                                                                                }
                                                                                elementstr = "<li class='case__item case__item--description'><div class='case__item-wrapper'><div class='case__label'>" + label + "</div><div class='case__data'>" + div + "</div></div></li>";
                                                                                elms = elementstr;
                                                                                break;
                                                                            #endregion
                                                                            default:
                                                                                break;
                                                                        }

                                                                        //string elms = string.Format(elementstr, attr);
                                                                        element = element + elms;//string.Format(ElementBody.WrapperTemplate, "", elms);
                                                                    }

                                                                    tavalDatasList.Add(taval);
                                                                }



                                                            }
                                                        }
                                                    }
                                                    column = column + element;
                                                }
                                                countRepeat++;
                                            }
                                        }

                                        //var RepeatItemData = targetIdLists.Where(x => x.TargetId == RowBody.ElementId).FirstOrDefault();
                                        //if(RepeatItemData != null)
                                        //{
                                        //    var repeat = RepeatItemData.Count;

                                        //}


                                        if (repeat > 0)
                                        {
                                            for (int i = 0; i < repeatItemLists.Count; i++)
                                            {
                                                repeatItemLists[i] += "</ul></div></div></li>";
                                            }
                                            row += string.Join(" ", repeatItemLists) + "</ul></li>" + "</ul></div></div></li></ul>";
                                        }
                                        else if (isRadioGroupChild)
                                        {
                                            row += string.Join(" ", radioGroupChild) + "</ul></li>";
                                        }
                                        else
                                        {
                                            if (selectBoxTargets.Contains(RowBody.ElementId))
                                            {
                                                if (tavalDatasList.Where(x => x == "").ToList().Count == tavalDatasList.Count)
                                                {
                                                    column = "";
                                                }
                                                else
                                                {
                                                    var random = new Random();
                                                    var id = random.Next();
                                                    var accordionId = "exampleAccordion" + random.Next();
                                                    var accordionUl = "<ul class='case__item-child case__accordion accordion' id=" + accordionId + " data-rowId=" + RowBody.ElementId + ">";
                                                    var accordianData = "<li class='card'><div class='card-header' id='heading" + id + "' data-toggle='collapse' data-target='#collapse" + id + "' aria-expanded='false' aria-controls='collapse" + id + "'>" +
        "<h5 class='mb-0'>" + headerText + "</h5></div><div id = 'collapse" + id + "' class='collapse' aria-labelledby='heading" + id + "' data-parent='#" + accordionId + "'>" +
        "<div class='card-body'><ul class='case__list'>";
                                                    if (column.Contains("case__item--has-accordian"))
                                                    {
                                                        column = accordionUl + accordianData + column;
                                                    }
                                                    else
                                                    {
                                                        column = accordionUl + accordianData + column + "</ul></div></div></li></ul>";
                                                    }
                                                }
                                            }
                                            row = row + column;
                                        }

                                    }

                                }

                            }

                            var last = this.FormData.Tab.Last();
                            //row += "</div></ul>";
                            if (tabCount == 0)
                            {
                                row = "<div class='row'><div class='col-lg-6 mb-4'><div class='card card--case'><div class='card-body'><div class='claim-detail-wrapper'>" + row + "</div></div></div></div>";

                                if (TabBody == last)
                                {
                                    row += "</div>";
                                }
                                tabCount++;
                            }
                            else
                            {
                                row = "<div class='col-lg-6 mb-4'><div class='card card--case'><div class='card-body'><div class='claim-detail-wrapper'>" + row + "</div></div></div></div></div>";
                                tabCount--;
                            }
                            this.Collection = this.Collection + row;

                        }
                    }

                    count = 0;


                }

            }
            catch (Exception ex)
            {

            }
            this.Collection += "<input hidden id='isFrontEnd' value='" + this.Side + "'/>";

            return this.Collection;
        }

        public class FormResponse
        {
            public bool Success { get; set; } = true;
            public string Message { get; set; } = "Success";
            public string Content { set; get; }
        }

        public class ClaimDetailHeaderData
        {
            public string Header { get; set; }
            public List<ClaimDetailData> DetailDatas { get; set; }
        }

        public class ClaimDetailData
        {
            public string Title { get; set; }
            public string Value { get; set; }
        }

        public class TargetOptionModel
        {
            public string ShowHide { get; set; }
            public string TargetId { get; set; }
            public int Count { get; set; }
            public bool IsRadioGroupChild { get; set; }
        }
    }
}





