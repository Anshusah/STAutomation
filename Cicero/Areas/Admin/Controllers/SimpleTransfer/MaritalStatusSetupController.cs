using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MaritalStatusSetupController : BaseController
    {
        private readonly IMaritalStatusSetupService IMaritalStatusSetupService;
      //  private readonly ILogger<MaritalStatusSetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public MaritalStatusSetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, IMaritalStatusSetupService _IMaritalStatusSetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IMaritalStatusSetupService = _IMaritalStatusSetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/MaritalStatusSetup.html")]
        [Route("admin/{tenant_identifier}/MaritalStatusSetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/MaritalStatusSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/MaritalStatusSetup.html")]
        [Route("admin/{tenant_identifier}/MaritalStatusSetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var MaritalStatusSetup = IMaritalStatusSetupService.GetMaritalStatusSetupListByFilter(model);
            return Json(new
            {
                draw = MaritalStatusSetup.draw,
                recordsTotal = MaritalStatusSetup.recordsTotal,
                recordsFiltered = MaritalStatusSetup.recordsFiltered,
                data = MaritalStatusSetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/MaritalStatusSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/MaritalStatusSetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            MaritalStatusSetupViewModel MaritalStatusSetupViewModel = new MaritalStatusSetupViewModel { Id = id, MaritalStatusName = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                MaritalStatusSetupViewModel = await IMaritalStatusSetupService.GetMaritalStatusSetupByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/MaritalStatusSetup/Edit.cshtml", MaritalStatusSetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/MaritalStatusSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/MaritalStatusSetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(MaritalStatusSetupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IMaritalStatusSetupService.CheckDuplicate(model))
                    {
                        _toastNotification.AddErrorToastMessage("Duplicate Marital Status.");
                        return View("/Areas/Admin/Views/SimpleTransfer/MaritalStatusSetup/Edit.cshtml", model);
                    }
                    model = await IMaritalStatusSetupService.CreateOrUpdate(model);                    
                    _toastNotification.AddSuccessToastMessage("Marital Status is saved.");
                    return Redirect("~/admin" + "/MaritalStatusSetup/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/MaritalStatusSetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/MaritalStatusSetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/MaritalStatusSetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/MaritalStatusSetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Marital Status.");
                return Redirect("~/admin/MaritalStatusSetup.html");
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
                        result = await IMaritalStatusSetupService.ActiveMaritalStatusSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IMaritalStatusSetupService.InActiveMaritalStatusSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IMaritalStatusSetupService.DeleteMaritalStatusSetupById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Marital Status(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Marital Status(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Marital Status(s) " + state);
            }

            return Redirect("~/admin/MaritalStatusSetup.html");

        }
    }
}