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
    public class RateSupplierFeeConfigController : BaseController
    {
        private readonly IRateSupplierFeeConfigService IRateSupplierFeeConfigService;
      //  private readonly ILogger<RateSupplierFeeConfigService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;
        private readonly IListService listService;

        public RateSupplierFeeConfigController(IPermissionService _permissionService,
            ICountryService _ICountryService, IRateSupplierFeeConfigService _IRateSupplierFeeConfigService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification,IListService listService) : base(_IUserService)
        {
            IRateSupplierFeeConfigService = _IRateSupplierFeeConfigService;
            utils = _utils;            
            _toastNotification = toastNotification;
            this.listService = listService;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/RateSupplierFeeConfig.html")]
        [Route("admin/{tenant_identifier}/RateSupplierFeeConfig.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/RateSupplierFeeConfig/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/RateSupplierFeeConfig.html")]
        [Route("admin/{tenant_identifier}/RateSupplierFeeConfig.html")]
        public JsonResult Index(DTPostModel model)
        {
            var RateSupplierFeeConfig = IRateSupplierFeeConfigService.GetRateSupplierFeeConfigListByFilter(model);
            return Json(new
            {
                draw = RateSupplierFeeConfig.draw,
                recordsTotal = RateSupplierFeeConfig.recordsTotal,
                recordsFiltered = RateSupplierFeeConfig.recordsFiltered,
                data = RateSupplierFeeConfig.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/RateSupplierFeeConfig/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/RateSupplierFeeConfig/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            RateSupplierFeeConfigViewModel RateSupplierFeeConfigViewModel = new RateSupplierFeeConfigViewModel { Id = id,CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                RateSupplierFeeConfigViewModel = await IRateSupplierFeeConfigService.GetRateSupplierFeeConfigByIdAsync(id);
            }
            RateSupplierFeeConfigViewModel.SupplierList = await listService.GetSupplierList();
            RateSupplierFeeConfigViewModel.CountryList = await listService.GetCountryList();
            RateSupplierFeeConfigViewModel.PayoutModeList = await listService.GetPayoutModeList();

            return View("/Areas/Admin/Views/SimpleTransfer/RateSupplierFeeConfig/Edit.cshtml", RateSupplierFeeConfigViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/RateSupplierFeeConfig/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/RateSupplierFeeConfig/{id}/edit.html")]

        public async Task<IActionResult> Edit(RateSupplierFeeConfigViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(!IRateSupplierFeeConfigService.CheckDuplicate(model))
                    {
                        _toastNotification.AddErrorToastMessage("Duplicate Upper Limit/Lower Limit/Transfer Fee.");
                        model.SupplierList = await listService.GetSupplierList();
                        model.CountryList = await listService.GetCountryList();
                        model.PayoutModeList = await listService.GetPayoutModeList();
                        return View("/Areas/Admin/Views/SimpleTransfer/RateSupplierFeeConfig/Edit.cshtml", model);
                    }
                    model = await IRateSupplierFeeConfigService.CreateOrUpdate(model);                   
                    _toastNotification.AddSuccessToastMessage("TransferFee is saved.");
                    return Redirect("~/admin" + "/RateSupplierFeeConfig/" + model.Id + "/edit.html");
                }
                model.SupplierList = await listService.GetSupplierList();
                model.CountryList = await listService.GetCountryList();
                model.PayoutModeList = await listService.GetPayoutModeList();
                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplierFeeConfig/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/RateSupplierFeeConfig/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/RateSupplierFeeConfig/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/RateSupplierFeeConfig.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Transfer Fee Config.");
                return Redirect("~/admin/RateSupplierFeeConfig.html");
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
                        result = await IRateSupplierFeeConfigService.ActiveRateSupplierFeeConfigById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IRateSupplierFeeConfigService.InActiveRateSupplierFeeConfigById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IRateSupplierFeeConfigService.DeleteRateSupplierFeeConfigById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " TransferFee(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " TransferFee(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " TransferFee(s) " + state);
            }

            return Redirect("~/admin/RateSupplierFeeConfig.html");

        }
    }
}