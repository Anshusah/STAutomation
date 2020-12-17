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
    public class SourceOfFundSetupController : BaseController
    {
        private readonly ISourceOfFundSetupService ISourceOfFundSetupService;
      //  private readonly ILogger<SourceOfFundSetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public SourceOfFundSetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, ISourceOfFundSetupService _ISourceOfFundSetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            ISourceOfFundSetupService = _ISourceOfFundSetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/SourceOfFundSetup.html")]
        [Route("admin/{tenant_identifier}/SourceOfFundSetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/SourceOfFundSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/SourceOfFundSetup.html")]
        [Route("admin/{tenant_identifier}/SourceOfFundSetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var SourceOfFundSetup = ISourceOfFundSetupService.GetSourceOfFundSetupListByFilter(model);
            return Json(new
            {
                draw = SourceOfFundSetup.draw,
                recordsTotal = SourceOfFundSetup.recordsTotal,
                recordsFiltered = SourceOfFundSetup.recordsFiltered,
                data = SourceOfFundSetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/SourceOfFundSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/SourceOfFundSetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            SourceOfFundSetupViewModel sourceOfFundSetupViewModel = new SourceOfFundSetupViewModel { Id = id, SourceOfFundName = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            sourceOfFundSetupViewModel.TransfastSourceOfFundList = ISourceOfFundSetupService.GetTransfastSourceOfFundAsync(0);
            if (id != 0)
            {
                sourceOfFundSetupViewModel = await ISourceOfFundSetupService.GetSourceOfFundSetupByIdAsync(id);
                sourceOfFundSetupViewModel.TransfastSourceOfFundList = ISourceOfFundSetupService.GetTransfastSourceOfFundAsync(sourceOfFundSetupViewModel.TransfastId);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/SourceOfFundSetup/Edit.cshtml", sourceOfFundSetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/SourceOfFundSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/SourceOfFundSetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(SourceOfFundSetupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!ISourceOfFundSetupService.CheckDuplicate(model))
                    {
                        model.TransfastSourceOfFundList = ISourceOfFundSetupService.GetTransfastSourceOfFundAsync(model.TransfastId);
                        _toastNotification.AddErrorToastMessage("Duplicate Payment Purpose.");
                        return View("/Areas/Admin/Views/SimpleTransfer/SourceOfFundSetup/Edit.cshtml", model);
                    }
                    model = await ISourceOfFundSetupService.CreateOrUpdate(model);
                    model.TransfastSourceOfFundList = ISourceOfFundSetupService.GetTransfastSourceOfFundAsync(model.TransfastId);
                    _toastNotification.AddSuccessToastMessage("Payment Purpose is saved.");
                    return Redirect("~/admin" + "/SourceOfFundSetup/" + model.Id + "/edit.html");
                }
                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/SourceOfFundSetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/SourceOfFundSetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/SourceOfFundSetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/SourceOfFundSetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Payment Purpose.");
                return Redirect("~/admin/SourceOfFundSetup.html");
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
                        result = await ISourceOfFundSetupService.ActiveSourceOfFundSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await ISourceOfFundSetupService.InActiveSourceOfFundSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await ISourceOfFundSetupService.DeleteSourceOfFundSetupById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Payment Purpose(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Payment Purpose(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Payment Purpose(s) " + state);
            }

            return Redirect("~/admin/SourceOfFundSetup.html");

        }
    }
}