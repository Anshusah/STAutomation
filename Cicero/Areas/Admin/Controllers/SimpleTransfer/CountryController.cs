using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Models.SimpleTransfer.Country;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CountryController : BaseController
    {
        private readonly ICountryService ICountryService;
        private readonly ILogger<CountryController> Log;
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IFormBuilderService formBuilderService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public CountryController(IPermissionService _permissionService,
            ICountryService _ICountryService, ILogger<CountryController> _Log, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            ICountryService = _ICountryService;
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
        [Route("admin/country.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/Country/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/country.html")]
        public JsonResult Index(DTPostModel model)
        {
            var country = ICountryService.GetCountryListByFilter(model);
            return Json(new
            {
                draw = country.draw,
                recordsTotal = country.recordsTotal,
                recordsFiltered = country.recordsFiltered,
                data = country.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/country/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            CountryViewModel countryViewModel = new CountryViewModel { Id = id, CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                countryViewModel = await ICountryService.GetCountryByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/Country/Edit.cshtml",countryViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/country/{id}/edit.html")]
        public async Task<IActionResult> Edit(CountryViewModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    model = await ICountryService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("Country is saved.");
                    return Redirect("~/admin" + "/country/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/Country/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/Country/Edit.cshtml", model);
            }
        }
        [HttpPost]
        [Route("admin/country/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/country.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one country.");
                return Redirect("~/admin/country.html");
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
                        result = await ICountryService.ActiveCountryById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await ICountryService.InactiveCountryById(item);
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

            return Redirect("~/admin/country.html");

        }
        [Route("api/country/phonecode")]
        [HttpGet]
        public JsonResult GetPhoneCodeByCountryCode(string countryCode)
        {
            return Json(ICountryService.GetPhoneCodeByCountryCode(countryCode));
        }
    }
}