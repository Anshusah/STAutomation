using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.City;
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
    public class CityController: BaseController
    {
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICityService cityService;
        private readonly ICommonService commonService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public CityController(IPermissionService _permissionService, IStatus _status, Utils _utils, IUserService _IUserService, ICityService cityService,ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            Status = _status;
            utils = _utils;
            IUserService = _IUserService;
            this.cityService = cityService;
            commonService = _commonService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/city.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/City/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/city.html")]
        public JsonResult Index(DTPostModel model)
        {
            var cityList = cityService.GetCityListByFilter(model);
            return Json(new
            {
                draw = cityList.draw,
                recordsTotal = cityList.recordsTotal,
                recordsFiltered = cityList.recordsFiltered,
                data = cityList.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/city/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            CityViewModel cityViewModel = new CityViewModel { Id = id, CountryCode = "", SupplierId = 0, CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                cityViewModel = await cityService.GetCityByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/City/Edit.cshtml", cityViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/city/{id}/edit.html")]
        public async Task<IActionResult> Edit(CityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model = await cityService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("City is saved.");
                    return Redirect("~/admin" + "/city/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/City/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/City/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/city/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/city.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one city.");
                return Redirect("~/admin/city.html");
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
                        result = await cityService.ActiveCityById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await cityService.InActiveCityById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await cityService.DeleteCityById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " city(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " city(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " city(s) " + state);
            }

            return Redirect("~/admin/city.html");

        }
    }
}
