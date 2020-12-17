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
    public class GenderSetupController : BaseController
    {
        private readonly IGenderSetupService IGenderSetupService;
      //  private readonly ILogger<GenderSetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public GenderSetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, IGenderSetupService _IGenderSetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IGenderSetupService = _IGenderSetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/GenderSetup.html")]
        [Route("admin/{tenant_identifier}/GenderSetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/GenderSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/GenderSetup.html")]
        [Route("admin/{tenant_identifier}/GenderSetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var GenderSetup = IGenderSetupService.GetGenderSetupListByFilter(model);
            return Json(new
            {
                draw = GenderSetup.draw,
                recordsTotal = GenderSetup.recordsTotal,
                recordsFiltered = GenderSetup.recordsFiltered,
                data = GenderSetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/GenderSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/GenderSetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            GenderSetupViewModel GenderSetupViewModel = new GenderSetupViewModel { Id = id, Name = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                GenderSetupViewModel = await IGenderSetupService.GetGenderSetupByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/GenderSetup/Edit.cshtml", GenderSetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/GenderSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/GenderSetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(GenderSetupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(!IGenderSetupService.CheckDuplicate(model))
                    {
                        _toastNotification.AddErrorToastMessage("Duplicate Gender Code.");
                        return View("/Areas/Admin/Views/SimpleTransfer/GenderSetup/Edit.cshtml", model);
                    }
                    model = await IGenderSetupService.CreateOrUpdate(model);                   
                    _toastNotification.AddSuccessToastMessage("Gender is saved.");
                    return Redirect("~/admin" + "/GenderSetup/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/GenderSetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/GenderSetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/GenderSetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action == null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/GenderSetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Gender.");
                return Redirect("~/admin/GenderSetup.html");
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
                        result = await IGenderSetupService.ActiveGenderSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IGenderSetupService.InActiveGenderSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IGenderSetupService.DeleteGenderSetupById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Gender(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Gender(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Gender(s) " + state);
            }

            return Redirect("~/admin/GenderSetup.html");

        }
    }
}