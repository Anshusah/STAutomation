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
    public class PaymentPurposeSetupController : BaseController
    {
        private readonly IPaymentPurposeSetupService IPaymentPurposeSetupService;
      //  private readonly ILogger<PaymentPurposeSetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public PaymentPurposeSetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, IPaymentPurposeSetupService _IPaymentPurposeSetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IPaymentPurposeSetupService = _IPaymentPurposeSetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/PaymentPurposeSetup.html")]
        [Route("admin/{tenant_identifier}/PaymentPurposeSetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/PaymentPurposeSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/PaymentPurposeSetup.html")]
        [Route("admin/{tenant_identifier}/PaymentPurposeSetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var PaymentPurposeSetup = IPaymentPurposeSetupService.GetPaymentPurposeSetupListByFilter(model);
            return Json(new
            {
                draw = PaymentPurposeSetup.draw,
                recordsTotal = PaymentPurposeSetup.recordsTotal,
                recordsFiltered = PaymentPurposeSetup.recordsFiltered,
                data = PaymentPurposeSetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/PaymentPurposeSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/PaymentPurposeSetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            PaymentPurposeSetupViewModel paymentPurposeSetupViewModel = new PaymentPurposeSetupViewModel { Id = id, PurposeName = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            paymentPurposeSetupViewModel.TransfastRemittancePurposeList = IPaymentPurposeSetupService.GetTransfastRemittancePurposeAsync(0);
            if (id != 0)
            {
                paymentPurposeSetupViewModel = await IPaymentPurposeSetupService.GetPaymentPurposeSetupByIdAsync(id);
                paymentPurposeSetupViewModel.TransfastRemittancePurposeList = IPaymentPurposeSetupService.GetTransfastRemittancePurposeAsync(paymentPurposeSetupViewModel.TransfastId);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/PaymentPurposeSetup/Edit.cshtml", paymentPurposeSetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/PaymentPurposeSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/PaymentPurposeSetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(PaymentPurposeSetupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IPaymentPurposeSetupService.CheckDuplicate(model))
                    {
                        model.TransfastRemittancePurposeList = IPaymentPurposeSetupService.GetTransfastRemittancePurposeAsync(model.TransfastId);
                        _toastNotification.AddErrorToastMessage("Duplicate Payment Purpose.");
                        return View("/Areas/Admin/Views/SimpleTransfer/PaymentPurposeSetup/Edit.cshtml", model);
                    }
                    model = await IPaymentPurposeSetupService.CreateOrUpdate(model);
                    model.TransfastRemittancePurposeList = IPaymentPurposeSetupService.GetTransfastRemittancePurposeAsync(model.TransfastId);
                    _toastNotification.AddSuccessToastMessage("Payment Purpose is saved.");
                    return Redirect("~/admin" + "/PaymentPurposeSetup/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/PaymentPurposeSetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/PaymentPurposeSetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/PaymentPurposeSetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/PaymentPurposeSetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Payment Purpose.");
                return Redirect("~/admin/PaymentPurposeSetup.html");
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
                        result = await IPaymentPurposeSetupService.ActivePaymentPurposeSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IPaymentPurposeSetupService.InActivePaymentPurposeSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IPaymentPurposeSetupService.DeletePaymentPurposeSetupById(item);
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

            return Redirect("~/admin/PaymentPurposeSetup.html");

        }
    }
}