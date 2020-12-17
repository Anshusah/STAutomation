using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Areas.Admin.Controllers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Areas.Identity.Pages.Account;
//using Cicero.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.UI.Services;
using Cicero.Service.Models.Core;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ValidatorController : BaseController
    {
       // private readonly IArticleService _articleService;
        private readonly AppSetting _setting;
        private readonly IUserService UserService;
        private readonly IDashboardService _dashboardService;
        private readonly IActivityLogService activityLogService;
     //   private readonly ITenantConfig tenantConfig;
        private readonly Utils _utils;
        private readonly IFormBuilderService _formBuilderService;
        //private readonly IEmailSender _emailSender;
        private readonly IRazorToStringRender _renderRazorToString;
        private readonly IValidationService _validationService;

        public ValidatorController(IUserService _UserService, IArticleService articleService, AppSetting setting, IEmailSender emailSender, IRazorToStringRender renderRazorToString, IFormBuilderService formBuilderService, ITenantConfig _tenantConfig, Utils utils, IDashboardService dashboardService, IActivityLogService _activityLogService, IValidationService validationService) : base(_UserService)
        {
          //  _articleService = articleService;
            _setting = setting;
            UserService = _UserService;
        //    _emailSender = emailSender;
            _renderRazorToString = renderRazorToString;
            _dashboardService = dashboardService;
            activityLogService = _activityLogService;
        //    tenantConfig = _tenantConfig;
            _utils = utils;
            _formBuilderService = formBuilderService;
            _validationService = validationService;

        }

        [HttpPost]
        [Route("/admin/validate.html")]
        public IActionResult Index(string type, string val, string checkOp, string opVal, string opMesg)
        {
           return Json( _validationService.ValidateElement(type, val, checkOp, opVal,opMesg));
        }
        [HttpGet]
        [Route("/admin/validate.html")]
        public IActionResult Index(string type)
        {

            var keys = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            bool success = true;
            var types = keys[0];
            var op = keys[1];
            var opVal = Convert.ToInt32(keys[2]);
            var elmVal = keys[keys.Count - 1];
            var values = elmVal.Split(',');
            if (elmVal != "")
            {
                switch (op)
                {

                    case ">":
                        if (types == "multiselectbox" || types == "checkboxgroup")
                        {

                            if (values.Count() < opVal)
                            {
                                success = false;
                            }
                        }
                        break;
                    case "<":
                        if (types == "multiselectbox" || types == "checkboxgroup")
                        {
                            if (values.Count() > opVal)
                            { success = false; }
                        }
                        break;
                    case "between":
                        {
                            if (types == "multiselectbox" || types == "checkboxgroup")
                            {
                                var opVal1 = Convert.ToInt32(keys[3]);
                                if (values.Count() > opVal1 || values.Count() < opVal)
                                {
                                    success = false;
                                }
                            }
                        }
                        break;
                    case "=":
                        if (types == "multiselectbox" || types == "checkboxgroup")
                        {
                            if (values.Count() != opVal)
                            {
                                success = false;
                            }
                        }
                        break;

                }
            }
            else { success = false; }




            //string a = abc[0];
            //  string hello = bc[0];
            //List<string> value = Request.Query.Keys.FirstOrDefault()
            return Json(success);
        }

        [HttpPost]
        [Route("/admin/validateAll.html")]
        public IActionResult ValidateFormData(IFormCollection formData, string form, List<string> hiddenFormData, bool isFrontValidation)
        {
            List<ElementValidation> elmvalidations =  _validationService.ValidateFormData(formData,form,hiddenFormData, isFrontValidation);
            List<ElementValidation> elmval = elmvalidations.Where(x => x.IsValid == false).ToList();
            bool valid = (elmvalidations.Where(x => x.IsValid == false).Count() > 0)? false : true;
            Validations validations = new Validations()
            {
                ElementValidations = elmvalidations,
                isFormValid = valid
            };
            return Json(validations);
        }

    }
}