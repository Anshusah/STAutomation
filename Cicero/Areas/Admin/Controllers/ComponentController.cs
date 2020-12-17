using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Cicero.Service.Services;
using Newtonsoft.Json;
using Cicero.Service.Helpers;
using Cicero.Data;
using Cicero.Service.Models.Core;
using Cicero.Service.Models;
using Themes;
using Newtonsoft.Json.Linq;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ComponentController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly Utils utils;
        private readonly IUserService userService;
        private readonly AppSetting Setting = null;


        public ComponentController(Utils _utils, ApplicationDbContext _ApplicationDbContext, AppSetting _Setting, IUserService _UserService) : base(_UserService)
        {
            db = _ApplicationDbContext;
            utils = _utils;
            Setting = _Setting;
            userService = _UserService;
        }

        [Route("admin/components.html")]
        [Route("admin/{tenant_identifier}/components.html")]
        public ActionResult Index()
        {

            return View();


        }
        [HttpGet]
        [Route("admin/manage/workflow/loadcomponentsbytype")]
        [Route("admin/{tenant_identifier}/manage/workflow/loadcomponentsbytype")]
        public IActionResult LoadComponents(string type, string removeObj, string condition, string formId)
        {
            //int id = utils.DecryptId(encrypted_state_id);

            try
            {

                Theme _theme = this.Theme as Theme;
                List<object> wget = _theme.GetComponentsByFormId(formId);
                List<object> components = new List<object>();
                if (condition != "" && condition != null)
                {
                    dynamic compon = _theme.GetComponent(condition);
                    components.Add(compon);
                }
                else
                {
                    if (wget != null && wget.Count() > 0)
                    {
                        foreach (dynamic comp in wget)
                        {
                            int check = 0;
                            if (comp.ComponentType == type)
                            {
                                if (removeObj != null)
                                {
                                    foreach (string item in removeObj.Split(","))
                                    {
                                        if (item == comp.ComponentId.ToString())
                                        { check = 1; break; }
                                    }
                                }

                                if (check == 0)
                                {
                                    components.Add(comp);
                                }
                            }
                        }

                }
            }


                switch (type)
            {
                case "Themes.Core.Components.CaseAutomation":
                    return PartialView("_AutomationModal", components);

                case "Themes.Core.Components.PolicySystem":
                    return PartialView("_SychronizationModal", components);
                case "Themes.Core.Components.AssignValue":
                    return PartialView("_AssignModal", components);
                default:
                    return Content("Component type is not defined");
            }

        }
            catch (Exception ex)
            {
                return Content(ex.ToString());
    }

}

        [HttpGet]
        [Route("admin/manage/workflow/loadcomponentById")]
        public IActionResult LoadComponentById(string id, string type, int formId)
        {
            try
            {
                Theme _theme = this.Theme as Theme;
                dynamic component = _theme.GetComponent(id);

                switch (type)
                {
                    case "api":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.PolicySystem();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedSyncData", component);
                    case "automation":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.CaseAutomation();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedAutomationData", component);
                    case "assign":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.AssignValue();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedAssignData", component);
                    default:
                        return Content("Component type is not defined");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpGet]
        [Route("admin/manage/workflow/element/loadcomponentsbytype")]
        [Route("admin/{tenant_identifier}/manage/workflow/element/loadcomponentsbytype")]
        public IActionResult LoadElementComponents(string type, string removeObj, string condition, string formId, string elementId, int eventType)
        {
            //int id = utils.DecryptId(encrypted_state_id);

            try
            {

                Theme _theme = this.Theme as Theme;
                List<object> wget = _theme.GetElementComponents(Convert.ToInt32(formId), elementId, eventType);
                List<object> components = new List<object>();
                if (condition != "" && condition != null)
                {
                    dynamic compon = _theme.GetComponent(condition);
                    components.Add(compon);
                }
                else
                {
                    if (wget != null && wget.Count() > 0)
                    {
                        foreach (dynamic comp in wget)
                        {
                            int check = 0;
                            if (comp.ComponentType == type)
                            {
                                if (removeObj != null)
                                {
                                    foreach (string item in removeObj.Split(","))
                                    {
                                        if (item == comp.ComponentId.ToString())
                                        { check = 1; break; }
                                    }
                                }

                                if (check == 0)
                                {
                                    components.Add(comp);
                                }
                            }
                        }

                    }
                }


                switch (type)
                {
                    case "Themes.Core.Components.CaseAutomation":
                        return PartialView("_AutomationModal", components);

                    case "Themes.Core.Components.PolicySystem":
                        return PartialView("_SychronizationModal", components);
                    case "Themes.Core.Components.AssignValue":
                        return PartialView("_AssignModal", components);
                    case "Themes.Core.Components.CaseAssignment":
                        return PartialView("_CaseAssignmentModal", components);
                    default:
                        return Content("Component type is not defined");
                }

            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        [HttpGet]
        [Route("admin/manage/workflow/element/loadcomponentbyid")]
        public IActionResult LoadComponentById(string id, string type, int formId, string elementId, int eventType)
        {
            try
            {
                Theme _theme = this.Theme as Theme;
                dynamic component = _theme.GetElementComponentById(id, formId, elementId, eventType);

                switch (type)
                {
                    case "api":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.PolicySystem();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedSyncData", component);
                    case "automation":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.CaseAutomation();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedAutomationData", component);
                    case "assign":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.AssignValue();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedAssignData", component);
                    case "caseassignment":
                        if (id == "0")
                        {
                            component = new Themes.Core.Components.CaseAssignment();
                            component.FormId = formId.ToString();
                        }
                        component.Theme = _theme;
                        return PartialView("SelectedCaseAssignmentData", component);
                    default:
                        return Content("Component type is not defined");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }
    }
}