using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Themes.Core.Widgets;
using Themes.Core.Components;
using Cicero.Service.Models.Core;
using Cicero.Service.Models.Core.Elements;
using System;
using System.Reflection;
using Cicero.Service.Services;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Newtonsoft.Json;

namespace Themes.Blue
{
    public class Setup : Theme
    {
        public Setup(HttpContext e)
        {
            this.HttpContext = e;
            this.This = this;

        }

        public void Init()
        {

            // Theme Name
            this.Name = GetName(false);
            // Theme Version
            this.Version = "0.1";
            // Register Required CSS

            this.RegisterCss("site", "/Themes/" + GetName(false) + "/css/style.css");
            this.RegisterCss("drangepicker-css", "https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css");

            // Register Required JS
            this.RegisterJs("jquery", "/js/jquery-3.3.1.min.js");
            this.RegisterJs("popper", "/js/popper.min.js");
            this.RegisterJs("bootstrapjs", "/js/bootstrap4.1.3/bootstrap.min.js");
            this.RegisterJs("validate", "/lib/jquery-validation/dist/jquery.validate.js");
            this.RegisterJs("validateunobtrusive", "/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js");
            this.RegisterJs("site", "/Themes/" + GetName(false) + "/js/site.js");
            this.RegisterJs("selectize", "/js/selectize.min.js");
            this.RegisterJs("jquery-number", "/js/jquery-number.js");
            this.RegisterJs("moment", "https://cdn.jsdelivr.net/momentjs/latest/moment.min.js");
            this.RegisterJs("drangepicker-js", "https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js");


            // Register Required Menu Locations for this Theme
            this.RegisterNav("Primary");
            this.RegisterNav("Bottom");

            this.RegisterElement(
               new
               {
                   Title = "Button",
                   Description = "This element helps you to define buttons",
                   Class = new Button(),
                   Status = "Active",
                   Icon = "icon-button",
                   Visiblity = "visible"
               }
           );

            this.RegisterElement(
               new
               {
                   Title = "Hyperlink",
                   Description = "This element helps you to define hyperlink",
                   Class = new Hyperlink(),
                   Status = "Active",
                   Icon = "icon-hyperlink",
                   Visiblity = "visible"
               }
           );

            this.RegisterElement(
               new
               {
                   Title = "Label",
                   Description = "This element helps you to define labels",
                   Class = new Label(),
                   Status = "Active",
                   Icon = "icon-label",
                   Visiblity = "visible"
               }
           );

            this.RegisterElement(
          new
          {
              Title = "Recaptcha",
              Description = "This element helps you to define recaptcha",
              Class = new Recaptcha(),
              Status = "Active",
              Icon = "icon-recaptcha",
              Visiblity = "visible"
          }
      );

            this.RegisterElement(
                new
                {
                    Title = "Row",
                    Description = "This element helps you to define rows",
                    Class = new Row(),
                    Status = "Active",
                    Icon = "fa fa-database",
                    Visiblity = "d-none"
                }
            );
            this.RegisterElement(
                new
                {
                    Title = "Column",
                    Description = "This element helps you to define column",
                    Class = new Cicero.Service.Models.Core.Elements.Column(),
                    Status = "Active",
                    Icon = "fa fa-database",
                    Visiblity = "d-none"
                }
            );
            this.RegisterElement(
             new
             {
                 Title = "Table",
                 Description = "This element helps you to define Table",
                 Class = new Cicero.Service.Models.Core.Elements.Table(),
                 Status = "Active",
                 Visiblity = "visible",
                 Icon = "icon-table"
             }
         );
            this.RegisterElement(
             new
             {
                 Title = "CheckBoxGroup",
                 Description = "This element helps you to define CheckBox Group",
                 Class = new CheckBoxGroup(),
                 Status = "Active",
                 Visiblity = "visible",
                 Icon = "icon-checkbox-group"
             }
         );
            this.RegisterElement(
                new
                {
                    Title = "Textbox Field",
                    Description = "This element helps you to place Textbox",
                    Class = new Textbox(),
                    Status = "Active",
                    Visiblity = "visible",
                    Icon = "icon-text"
                }
            );
            this.RegisterElement(
                new
                {
                    Title = "Number Field",
                    Description = "This element helps you to place Number",
                    Class = new Number(),
                    Status = "Active",
                    Visiblity = "visible",
                    Icon = "icon-number"
                }
            );
            this.RegisterElement(
                new
                {
                    Title = "RadioGroup",
                    Description = "This element helps you to place RadioGroup",
                    Class = new RadioGroup(),
                    Status = "Active",
                    Visiblity = "visible",
                    Icon = "icon-radio-group"
                }
            );
            this.RegisterElement(
                new
                {
                    Title = "Heading",
                    Description = "This element helps you to place Heading",
                    Class = new Heading(),
                    Status = "Active",
                    Visiblity = "visible",
                    Icon = "icon-header"
                }
            );
            this.RegisterElement(
                new
                {
                    Title = "Paragraph",
                    Description = "This element helps you to place Paragraph",
                    Class = new Paragraph(),
                    Status = "Active",
                    Visiblity = "visible",
                    Icon = "icon-paragraph"
                }
            );
            this.RegisterElement(
              new
              {
                  Title = "SelectBox Field",
                  Description = "This element helps you to place SelectBox",
                  Class = new SelectBox(),
                  Status = "Active",
                  Visiblity = "visible",
                  Icon = "icon-select"
              }
          );
            this.RegisterElement(
              new
              {
                  Title = "MultiSelectBox Field",
                  Description = "This element helps you to place MultiSelectBox",
                  Class = new MultiSelectBox(),
                  Status = "Active",
                  Visiblity = "visible",
                  Icon = "icon-select"
              }
          );
            this.RegisterElement(
              new
              {
                  Title = "TextArea Field",
                  Description = "This element helps you to place TextArea",
                  Class = new TextArea(),
                  Status = "Active",
                  Visiblity = "visible",
                  Icon = "icon-textarea"
              }
          );
            this.RegisterElement(
               new
               {
                   Title = "Media",
                   Description = "This element Helps you to upload media files",
                   Class = new Media(),
                   Status = "Active",
                   Visiblity = "visible",
                   Icon = "icon-file"
               }
           );
            //this.RegisterElement(
            //   new
            //   {
            //       Title = "Component",
            //       Description = "This element Helps you to add component ID",
            //       Class = new Cicero.Service.Models.Core.Elements.Component(),
            //       Status = "Active",
            //       Visiblity = "visible",
            //       Icon = "icon-paragraph"
            //   }
            //);
            this.RegisterComponent(
                new
                {
                    Title = "Case Automation",
                    Description = "This component helps you to switch case in specific condition",
                    Class = new CaseAutomation(),
                    Status = "Active",
                    Icon = "fa fa-magic"
                }
            );
            this.RegisterComponent(
                new
                {
                    Title = "Policy System Connector",
                    Description = "This component helps you to sync with Policy Management System",
                    Class = new PolicySystem(),
                    Status = "Active",
                    Icon = "fa fa-database"
                }
            );
            //this.RegisterComponent(
            //    new
            //    {
            //        Title = "Case Automation",
            //        Description = "This component helps you to switch case in specific condition",
            //        Class = new CaseAutomation(),
            //        Status = "Active",
            //        Icon = "fa fa-magic"
            //    }
            //);

            this.RegisterWidget(
                new
                {
                    Title = "Text Widget",
                    Description = "This Widget helps you to Place text anywhere",
                    Class = new Text(),
                    Status = "Active",
                    Icon = "far fa-list-alt"
                }
            );

            this.RegisterWidget(
                new
                {
                    Title = "Social Icon",
                    Description = "This Widget Helps you to place social icons anywhere",
                    Class = new Social(),
                    Status = "Active",
                    Icon = "fa-share-alt"
                }
            );
            this.RegisterWidget(
                new
                {
                    Title = "Feature Article",
                    Description = "This Widget Helps you to list article anywhere in the theme",
                    Class = new FeatureArticle(),
                    Status = "Active",
                    Icon = "fa fa-deaf"
                }
            );

            this.RegisterWidget(
                new
                {
                    Title = "Custom Article",
                    Description = "This Widget Helps you to list/banner custom articles anywhere in the theme",
                    Class = new CustomArticle(),
                    Status = "Active",
                    Icon = "far fa-list-ul"
                }
            );
            this.RegisterWidgetLocation(new WidgetLocation
            {
                Id = "aboutus",
                Status = "active",
                Title = "This is for Footer About us",
                TitleWrap = "<h1>{0}</h1>",
                ContentWrap = "<p>{0}</p>"
            });
            this.RegisterWidgetLocation(new WidgetLocation
            {
                Id = "link",
                Status = "active",
                Title = "This is for Link List",
                TitleWrap = "<h2>{0}</h2>",
                ContentWrap = "<p>{0}</p>"
            });

            this.RegisterWidgetLocation(new WidgetLocation
            {
                Id = "social",
                Status = "active",
                Title = "This is for Purple 2 location",
                TitleWrap = "<h1>{0}</h1>",
                ContentWrap = "<p>{0}</p>"
            });

            this.AddAction("Purple", "Purple", 0);
            //this.AddFilter("nav_li_args", "Filter", 0);
            this.AddShortCode("form", "formCallback");
            this.AddAction("hook", "AjaxHook", 0);
            this.AddAction("hook1", "AjaxHook1", 0);
            this.AddAction("before_case_switch", "OnBeforeCaseSwitch", 0);
            this.AddAction("on_automation_component_form_change", "GetAutomationFilters", 0);

        }
        public string GetAutomationFilters(Dictionary<string, string> e)
        {
            e.TryGetValue("action", out string action);
            e.TryGetValue("form_id", out string form_id);
            Type _type = Type.GetType("Themes.Core.Components.CaseAutomation");
            dynamic objs = Activator.CreateInstance(_type);
            MethodInfo MethodInfo = _type.GetMethod("GetFormFilters");
            objs.HttpContext = this.HttpContext;
            objs.Theme = this;
            return MethodInfo.Invoke(objs, new object[] { Convert.ToString(form_id) });
        }
        public void OnBeforeCaseSwitch(int switch_from = 0, int switch_to = 0, object[] estimates = null)
        {
            Type _type = Type.GetType("Themes.Core.Components.CaseAutomation");
            dynamic objs = Activator.CreateInstance(_type);
            MethodInfo MethodInfo = _type.GetMethod("RunRules");
            objs.HttpContext = this.HttpContext;
            objs.Theme = this;
            var final = MethodInfo.Invoke(objs, new object[] { estimates });

        }
        public string AjaxHook(Dictionary<string, string> e)
        {
            e.TryGetValue("action", out string action);
            return "{\"action\":\"hook\"}";
        }
        public string AjaxHook1(Dictionary<string, string> e)
        {
            e.TryGetValue("action", out string action);
            return "{\"action\":\"hook\"}";
        }
        public string Test()
        {
            return "test string";
        }
        public object Filter(object e)
        {
            //var dd = "";
            return e;
        }
        public string formCallback(object e)
        {
            ICommonService _ICommonService = this.HttpContext.RequestServices.GetService(typeof(ICommonService)) as ICommonService;
            IUserService _userService = this.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
            IFormService _formService = this.HttpContext.RequestServices.GetService(typeof(IFormService)) as IFormService;
            IFormBuilderService _formBuilderService = this.HttpContext.RequestServices.GetService(typeof(IFormBuilderService)) as IFormBuilderService;
            IQueueService _queueService = this.HttpContext.RequestServices.GetService(typeof(IQueueService)) as IQueueService;
            ICaseService _caseService = this.HttpContext.RequestServices.GetService(typeof(ICaseService)) as ICaseService;
            Utils _utils = this.HttpContext.RequestServices.GetService(typeof(Utils)) as Utils;
            int tenantid = _ICommonService.GetTenantIdByIdentifier(_utils.GetTenantFromSession());
            string view = "";


            string form = "quote";
            int Caseid = 177;
            try
            {
                Cicero.Service.Models.CaseViewModel cvm = new Cicero.Service.Models.CaseViewModel
                {
                    Id = Caseid,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CaseGeneratedId = _formService.GenerateFormId(),
                };
                var ccvm = _formBuilderService.GetBuilderFormByUrl(form, tenantid);
                int case_id = ccvm.Id;

                if (_userService.UserHasPolicy() == "frontend")
                {
                    cvm.StateId = _queueService.GetFirstState(case_id, "front");
                }
                else
                {
                    cvm.StateId = _queueService.GetFirstState(case_id, "back");
                }

                if (Caseid != 0)
                    cvm = _caseService.GetCaseById(Caseid);

                int caseformid = case_id;

                cvm.CaseFormName = _caseService.GetCaseFormTypeNameByCaseFormId(caseformid);

                string roleid = _ICommonService.GetRoleIdByUserId(_ICommonService.getLoggedInUserId());

                cvm.StateList = _queueService.GetStateSelectListByFormId(caseformid, roleid);

                cvm.CaseTasks = _queueService.GetCaseMovement(caseformid, Caseid);
                if (cvm.UserId != null)
                {
                    var userName = _userService.GetUserById(cvm.UserId).Result;
                    cvm.UserFullName = userName.FirstName + " " + userName.LastName;
                    cvm.CaseOwner = _queueService.GetCaseOwner(cvm.CaseFormId, cvm.StateId, cvm.UserId);
                }

                cvm.QueueName = _queueService.GetQueueNameByFormId(caseformid, Convert.ToInt32(cvm.StateId));
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                cvm.FormBuilder = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                FormBuilderViewModel a = JsonConvert.DeserializeObject<FormBuilderViewModel>(ccvm.Fields, settings);
                List<FormBuilderViewModel.Form.Table> tables = a.Forms.Tables;
                var formData = _formService.GetTableData(Caseid, tables, a);
                if (cvm.CaseFormId == 0)
                {
                    cvm.CaseFormId = caseformid;
                }




                dynamic data = new object();
                if (cvm.Id != 0)
                {
                    data = ccvm.Fields;
                }
                FormBuilderViewModel fbvm = cvm.FormBuilder as FormBuilderViewModel;

                Cicero.Service.Models.Core.FormBuilder FB = new Cicero.Service.Models.Core.FormBuilder() { FormData = fbvm, Side = "frontend", HttpContext = this.HttpContext };
                return FB.Render(cvm.Id, cvm.StateId, cvm.CaseFormId);
            }
            catch (Exception ex)
            {
                //_log.LogError("CaseFormService:Edit - " + ex.ToString());
            }
            return "Shortcode Error";
        }


    }
}
