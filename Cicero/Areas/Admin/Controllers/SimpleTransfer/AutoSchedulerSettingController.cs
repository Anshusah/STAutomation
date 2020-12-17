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
    public class AutoSchedulerSettingController : BaseController
    {
        private readonly IAutoSchedulerSettingService IAutoSchedulerSettingService;
        private readonly ILogger<AutoSchedulerSettingController> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public AutoSchedulerSettingController(IPermissionService _permissionService,
            IAutoSchedulerSettingService _IAutoSchedulerSettingService, ILogger<AutoSchedulerSettingController> _Log,Utils _utils,
            IUserService _IUserService,IToastNotification toastNotification) : base(_IUserService)
        {
            IAutoSchedulerSettingService = _IAutoSchedulerSettingService;
            Log = _Log;
            utils = _utils;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/autoschedulersetting.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/AutoSchedulerSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/autoschedulersetting.html")]
        public JsonResult Index(DTPostModel model)
        {
            var AutoSchedulerSetting = IAutoSchedulerSettingService.GetAutoSchedulerSettingByFilter(model);
            return Json(new
            {
                draw = AutoSchedulerSetting.draw,
                recordsTotal = AutoSchedulerSetting.recordsTotal,
                recordsFiltered = AutoSchedulerSetting.recordsFiltered,
                data = AutoSchedulerSetting.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/autoschedulersetting/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            AutoSchedulerSettingViewModel AutoSchedulerSettingViewModel = new AutoSchedulerSettingViewModel { Id = id, CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                AutoSchedulerSettingViewModel = await IAutoSchedulerSettingService.GetAutoSchedulerSettingByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/AutoSchedulerSetup/Edit.cshtml",AutoSchedulerSettingViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/autoschedulersetting/{id}/edit.html")]
        public async Task<IActionResult> Edit(AutoSchedulerSettingViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    model = await IAutoSchedulerSettingService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("AutoScheduler Setting is saved.");
                    return Redirect("~/admin" + "/autoschedulersetting/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View(model);
            }
            catch (Exception ex)
            {
                Log.LogError("AutoSchedulerSettingServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View(model);
            }
        }
    }
}