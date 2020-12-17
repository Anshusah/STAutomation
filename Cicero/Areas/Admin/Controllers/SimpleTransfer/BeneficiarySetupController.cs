using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.Beneficiary;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BeneficiarySetupController : BaseController
    {
        private readonly IBeneficiarySetupService IBeneficiarySetupService;
      //  private readonly ILogger<BeneficiarySetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public BeneficiarySetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, IBeneficiarySetupService _IBeneficiarySetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IBeneficiarySetupService = _IBeneficiarySetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/BeneficiarySetup.html")]
        [Route("admin/{tenant_identifier}/BeneficiarySetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/BeneficiarySetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/BeneficiarySetup.html")]
        [Route("admin/{tenant_identifier}/BeneficiarySetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var BeneficiarySetup = IBeneficiarySetupService.GetBeneficiarySetupListByFilter(model);
            return Json(new
            {
                draw = BeneficiarySetup.draw,
                recordsTotal = BeneficiarySetup.recordsTotal,
                recordsFiltered = BeneficiarySetup.recordsFiltered,
                data = BeneficiarySetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/BeneficiarySetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/BeneficiarySetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            BeneficiarySetupViewModel beneficiarySetupViewModel = new BeneficiarySetupViewModel { Id = id, FirstName = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                beneficiarySetupViewModel = await IBeneficiarySetupService.GetBeneficiarySetupByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/BeneficiarySetup/Edit.cshtml", beneficiarySetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/BeneficiarySetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/BeneficiarySetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(BeneficiarySetupViewModel model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    model = await IBeneficiarySetupService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("Beneficiary is saved.");
                    return Redirect("~/admin" + "/BeneficiarySetup/" + model.Id + "/edit.html");
                //}

                //utils.addModelError(ModelState);
                //return View("/Areas/Admin/Views/SimpleTransfer/BeneficiarySetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/BeneficiarySetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/BeneficiarySetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/BeneficiarySetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Beneficiary.");
                return Redirect("~/admin/BeneficiarySetup.html");
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
                        result = await IBeneficiarySetupService.ActiveBeneficiarySetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IBeneficiarySetupService.InActiveBeneficiarySetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IBeneficiarySetupService.DeleteBeneficiarySetupById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Beneficiary(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Beneficiary(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Beneficiary(s) " + state);
            }

            return Redirect("~/admin/BeneficiarySetup.html");

        }
    }
}