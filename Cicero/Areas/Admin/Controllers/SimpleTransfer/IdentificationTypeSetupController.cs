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
    public class IdentificationTypeSetupController : BaseController
    {
        private readonly IIdentificationTypeSetupService IIdentificationTypeSetupService;
      //  private readonly ILogger<IdentificationTypeSetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public IdentificationTypeSetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, IIdentificationTypeSetupService _IIdentificationTypeSetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IIdentificationTypeSetupService = _IIdentificationTypeSetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/IdentificationTypeSetup.html")]
        [Route("admin/{tenant_identifier}/IdentificationTypeSetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/IdentificationTypeSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/IdentificationTypeSetup.html")]
        [Route("admin/{tenant_identifier}/IdentificationTypeSetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var IdentificationTypeSetup = IIdentificationTypeSetupService.GetIdentificationTypeSetupListByFilter(model);
            return Json(new
            {
                draw = IdentificationTypeSetup.draw,
                recordsTotal = IdentificationTypeSetup.recordsTotal,
                recordsFiltered = IdentificationTypeSetup.recordsFiltered,
                data = IdentificationTypeSetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/IdentificationTypeSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/IdentificationTypeSetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            IdentificationTypeSetupViewModel IdentificationTypeSetupViewModel = new IdentificationTypeSetupViewModel { Id = id, IdentificationTypeName = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                IdentificationTypeSetupViewModel = await IIdentificationTypeSetupService.GetIdentificationTypeSetupByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/IdentificationTypeSetup/Edit.cshtml", IdentificationTypeSetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/IdentificationTypeSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/IdentificationTypeSetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(IdentificationTypeSetupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IIdentificationTypeSetupService.CheckDuplicate(model))
                    {
                        _toastNotification.AddErrorToastMessage("Duplicate Identification Type Code.");
                        return View("/Areas/Admin/Views/SimpleTransfer/IdentificationTypeSetup/Edit.cshtml", model);
                    }
                    model = await IIdentificationTypeSetupService.CreateOrUpdate(model);                   
                    _toastNotification.AddSuccessToastMessage("Identification Type is saved.");
                    return Redirect("~/admin" + "/IdentificationTypeSetup/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/IdentificationTypeSetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/IdentificationTypeSetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/IdentificationTypeSetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/IdentificationTypeSetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Identification type.");
                return Redirect("~/admin/IdentificationTypeSetup.html");
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
                        result = await IIdentificationTypeSetupService.ActiveIdentificationTypeSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IIdentificationTypeSetupService.InActiveIdentificationTypeSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IIdentificationTypeSetupService.DeleteIdentificationTypeSetupById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Identification Type(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Identification Type(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Identification Type(s) " + state);
            }

            return Redirect("~/admin/IdentificationTypeSetup.html");

        }
    }
}