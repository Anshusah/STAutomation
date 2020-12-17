using Cicero.Data.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.JazzCash;
using Cicero.Service.Services;
using Cicero.Service.Services.JazzCash;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.JazzCash
{
    public class SecurityQuestionController : BaseController
    {
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;
        private readonly ISecurityQuestionService securityQuestionService;
        private readonly ILogger<SecurityQuestionController> Log;

        public SecurityQuestionController(IPermissionService _permissionService, IStatus _status, Utils _utils, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification, ISecurityQuestionService securityQuestionService, ILogger<SecurityQuestionController> _Log) : base(_IUserService)
        {
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            commonService = _commonService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
            this.securityQuestionService = securityQuestionService;
            Log = _Log;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/securityquestion.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/JazzCash/SecurityQuestion/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/securityquestion.html")]
        public JsonResult Index(DTPostModel model)
        {
            var securityQuestions = securityQuestionService.GetSecurityQuestionListByFilter(model);
            return Json(new
            {
                draw = securityQuestions.draw,
                recordsTotal = securityQuestions.recordsTotal,
                recordsFiltered = securityQuestions.recordsFiltered,
                data = securityQuestions.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/securityquestion/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {
            var data = new SecurityQuestionViewModel { Id = id, CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                data = await securityQuestionService.GetSecurityQuestionByIdAsync(id);
            }
            return View("/Areas/Admin/Views/JazzCash/SecurityQuestion/Edit.cshtml", data);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/securityquestion/{id}/edit.html")]
        public async Task<IActionResult> Edit(SecurityQuestionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model = await securityQuestionService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("Security Question is saved.");
                    return Redirect("~/admin" + "/securityquestion/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/JazzCash/SecurityQuestion/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.LogError("SecurityQuestionService:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/JazzCash/SecurityQuestion/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/securityquestion/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/securityquestion.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one question.");
                return Redirect("~/admin/securityquestion.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {

                bool result = false;
                if (item != 0)
                {
                    if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                    {
                        state = ButtonAction.active.ToDescription();
                        result = await securityQuestionService.ActiveSecurityQuestionById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await securityQuestionService.InActiveSecurityQuestionById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await securityQuestionService.DeleteSecurityQuestionById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " security question(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " security question(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " security question(s) " + state);
            }

            return Redirect("~/admin/securityquestion.html");

        }
    }
}
