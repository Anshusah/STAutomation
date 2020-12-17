using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Models.SimpleTransfer.AutoSchedulerSetting;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExchangeRateSettingController : BaseController
    {
        private readonly IExchangeRateSettingService IExchangeRateSettingServices;
        private readonly ILogger<ExchangeRateSettingController> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public ExchangeRateSettingController(IPermissionService _permissionService,
            IExchangeRateSettingService _IExchangeRateSettingServices, ILogger<ExchangeRateSettingController> _Log,Utils _utils,
            IUserService _IUserService,IToastNotification toastNotification) : base(_IUserService)
        {
            IExchangeRateSettingServices = _IExchangeRateSettingServices;
            Log = _Log;
            utils = _utils;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/exchangeratesetting.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/ExchangeRateSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/exchangeratesetting.html")]
        public JsonResult Index(DTPostModel model)
        {
            var exchangeRates = IExchangeRateSettingServices.GetExchangeRateSettingListByFilter(model);
            return Json(new
            {
                draw = exchangeRates,
                recordsTotal = exchangeRates.recordsTotal,
                recordsFiltered = exchangeRates.recordsFiltered,
                data = exchangeRates.data
            });
        }

        //[Area("Admin")]
        //[HttpGet]
        //[Route("admin/exchangeratesetting/{id}/edit.html")]
        //public async Task<IActionResult> Edit(int id)
        //{

        //    AutoSchedulerSettingViewModel AutoSchedulerSettingViewModel = new AutoSchedulerSettingViewModel { Id = id, CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
        //    if (id != 0)
        //    {
        //        AutoSchedulerSettingViewModel = await IAutoSchedulerSettingService.GetAutoSchedulerSettingByIdAsync(id);
        //    }

        //    return View("/Areas/Admin/Views/SimpleTransfer/ExchangeRateSetup/Edit.cshtml", AutoSchedulerSettingViewModel);
        //}

        //[Area("Admin")]
        //[HttpPost]
        //[Route("admin/exchangeratesetting/{id}/edit.html")]
        //public async Task<IActionResult> Edit(AutoSchedulerSettingViewModel model)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            model = await IAutoSchedulerSettingService.CreateOrUpdate(model);
        //            _toastNotification.AddSuccessToastMessage("ExchangeRate Setting is saved.");
        //            return Redirect("~/admin" + "/exchangeratesetting/" + model.Id + "/edit.html");
        //        }

        //        utils.addModelError(ModelState);
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.LogError("ExchangeRateSetupServices:Edit - " + ex.ToString());
        //        _toastNotification.AddErrorToastMessage(ex.ToString());
        //        return View(model);
        //    }
        //}
    }
}