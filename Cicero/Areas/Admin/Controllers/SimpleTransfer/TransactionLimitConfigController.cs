using Cicero.Data.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.TransactionLimitConfig;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TransactionLimitConfigController : BaseController
    {
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;
        private readonly ITransactionLimitConfigService transactionLimitConfigService;

        public TransactionLimitConfigController(IPermissionService _permissionService, IStatus _status, Utils _utils, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification, ITransactionLimitConfigService transactionLimitConfigService) : base(_IUserService)
        {
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            commonService = _commonService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
            this.transactionLimitConfigService = transactionLimitConfigService;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/transactionlimitconfig.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/TransactionLimitConfig/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/transactionlimitconfig.html")]
        public JsonResult Index(DTPostModel model)
        {
            var transactionLimitConfig = transactionLimitConfigService.GetTransactionLimitConfigByFilter(model);
            return Json(new
            {
                draw = transactionLimitConfig.draw,
                recordsTotal = transactionLimitConfig.recordsTotal,
                recordsFiltered = transactionLimitConfig.recordsFiltered,
                data = transactionLimitConfig.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/transactionlimitconfig/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {
            TransactionLimitConfigViewModel transactionLimitConfigViewModel = new TransactionLimitConfigViewModel { Id = id, CountryCode = "", CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                transactionLimitConfigViewModel = await transactionLimitConfigService.GetTransactionLimitConfigByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/TransactionLimitConfig/Edit.cshtml", transactionLimitConfigViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/transactionlimitconfig/{id}/edit.html")]
        public async Task<IActionResult> Edit(TransactionLimitConfigViewModel model)
        {
            try
            {
                var checkDuplicate = transactionLimitConfigService.CheckDuplicate(model.Id, model.CountryCode).Result;

                if (checkDuplicate)
                {
                    _toastNotification.AddErrorToastMessage("Duplicate Entry.");
                    return View("/Areas/Admin/Views/SimpleTransfer/TransactionLimitConfig/Edit.cshtml", model);
                }
                if (ModelState.IsValid)
                {
                    model = await transactionLimitConfigService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("transaction Limit Config is saved.");
                    return Redirect("~/admin" + "/transactionlimitconfig/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/TransactionLimitConfig/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/TransactionLimitConfig/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/transactionlimitconfig/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/transactionlimitconfig.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one transaction limit config.");
                return Redirect("~/admin/transactionlimitconfig.html");
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
                        result = await transactionLimitConfigService.ActiveTransactionLimitConfigById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await transactionLimitConfigService.InActiveTransactionLimitConfigById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await transactionLimitConfigService.DeleteTransactionLimitConfigById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " transactionLimitConfig(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " transactionLimitConfig(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " transactionLimitConfig(s) " + state);
            }

            return Redirect("~/admin/transactionlimitconfig.html");

        }
    }
}
