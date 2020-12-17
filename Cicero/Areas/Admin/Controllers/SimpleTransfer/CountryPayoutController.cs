using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.CountryPayout;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CountryPayoutController : BaseController
    {
        private readonly ICountryService ICountryService;
        private readonly ICountryPayoutService ICountryPayoutService;
      //  private readonly ILogger<CountryPayoutService> Log;
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IFormBuilderService formBuilderService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public CountryPayoutController(IPermissionService _permissionService,
            ICountryService _ICountryService, ICountryPayoutService _ICountryPayoutService, /*ILogger<CountryPayoutService> _Log,*/ IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            ICountryService = _ICountryService;
            ICountryPayoutService = _ICountryPayoutService;
           // Log = _Log;
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            commonService = _commonService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/countrypayout.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/CountryPayout/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/countrypayout.html")]
        public JsonResult Index(DTPostModel model)
        {
            var countryPayout = ICountryPayoutService.GetCountryPayoutListByFilter(model);
            return Json(new
            {
                draw = countryPayout.draw,
                recordsTotal = countryPayout.recordsTotal,
                recordsFiltered = countryPayout.recordsFiltered,
                data = countryPayout.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/countryPayout/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            CountryPayoutViewModel countryPayoutViewModel = new CountryPayoutViewModel { Id = id, CountryCode = "",PaymentMethodId = 0, CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), IsActive = false };
            if (id != 0)
            {
                countryPayoutViewModel = await ICountryPayoutService.GetCountryPayoutByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/CountryPayout/Edit.cshtml", countryPayoutViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/countryPayout/{id}/edit.html")]
        public async Task<IActionResult> Edit(CountryPayoutViewModel model)
        {
            try
            {
                var checkDuplicate = ICountryPayoutService.CheckDuplicate(model.Id, model.CountryCode, model.PaymentMethodId).Result;

                if (checkDuplicate)
                {
                    _toastNotification.AddErrorToastMessage("Duplicate Entry.");
                    return View("/Areas/Admin/Views/SimpleTransfer/CountryPayout/Edit.cshtml", model);
                }
                if (ModelState.IsValid)
                {
                    model = await ICountryPayoutService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("Country Payout is saved.");
                    return Redirect("~/admin" + "/countryPayout/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/CountryPayout/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/CountryPayout/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/countryPayout/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/countryPayout.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one country payout.");
                return Redirect("~/admin/countryPayout.html");
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
                        result = await ICountryPayoutService.ActiveCountryPayoutById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await ICountryPayoutService.InActiveCountryPayoutById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await ICountryPayoutService.DeleteCountryPayoutById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " country(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " country(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " country(s) " + state);
            }

            return Redirect("~/admin/countryPayout.html");

        }
    }
}