using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Models.SimpleTransfer.BankSetting;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BankSettingController : BaseController
    {
        private readonly IBankSettingService IBankSettingService;
        private readonly ILogger<BankSettingController> Log;
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IFormBuilderService formBuilderService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public BankSettingController(IPermissionService _permissionService,
            IBankSettingService _IBankSettingService, ILogger<BankSettingController> _Log, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IBankSettingService = _IBankSettingService;
            Log = _Log;
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            commonService = _commonService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/banksetting.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/BankSetting/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/banksetting.html")]
        public JsonResult Index(DTPostModel model)
        {
            var bankSetting = IBankSettingService.GetBankSettingListByFilter(model);
            return Json(new
            {
                draw = bankSetting.draw,
                recordsTotal = bankSetting.recordsTotal,
                recordsFiltered = bankSetting.recordsFiltered,
                data = bankSetting.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/banksetting/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            BankSettingViewModel BankSettingViewModel = new BankSettingViewModel { Id = id, CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                BankSettingViewModel = await IBankSettingService.GetBankSettingByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/BankSetting/Edit.cshtml",BankSettingViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/banksetting/{id}/edit.html")]
        public async Task<IActionResult> Edit(BankSettingViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    model = await IBankSettingService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("BankSetting is saved.");
                    return Redirect("~/admin" + "/banksetting/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/BankSetting/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.LogError("BankSettingServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/BankSetting/Edit.cshtml", model);
            }
        }
        [HttpPost]
        [Route("admin/banksetting/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/banksetting.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one BankSetting.");
                return Redirect("~/admin/banksetting.html");
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
                        result = await IBankSettingService.ActiveBankSettingById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IBankSettingService.InactiveBankSettingById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " BankSetting(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " BankSetting(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " BankSetting(s) " + state);
            }

            return Redirect("~/admin/banksetting.html");

        }
    }
}