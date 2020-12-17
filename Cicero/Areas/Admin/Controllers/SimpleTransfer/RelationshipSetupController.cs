using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.RelationshipSetup;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Cicero.Service.Enums;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RelationshipSetupController : BaseController
    {
        private readonly IRelationshipSetupService IRelationshipSetupService;
      //  private readonly ILogger<RelationshipSetupService> Log;
        private readonly Utils utils;
        private readonly IToastNotification _toastNotification;

        public RelationshipSetupController(IPermissionService _permissionService,
            ICountryService _ICountryService, IRelationshipSetupService _IRelationshipSetupService, IStatus _status, Utils _utils,
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            IRelationshipSetupService = _IRelationshipSetupService;
            utils = _utils;            
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/RelationshipSetup.html")]
        [Route("admin/{tenant_identifier}/RelationshipSetup.html")]
        public IActionResult Index()
        {
            return View("/Areas/Admin/Views/SimpleTransfer/RelationshipSetup/Index.cshtml");
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/RelationshipSetup.html")]
        [Route("admin/{tenant_identifier}/RelationshipSetup.html")]
        public JsonResult Index(DTPostModel model)
        {
            var RelationshipSetup = IRelationshipSetupService.GetRelationshipSetupListByFilter(model);
            return Json(new
            {
                draw = RelationshipSetup.draw,
                recordsTotal = RelationshipSetup.recordsTotal,
                recordsFiltered = RelationshipSetup.recordsFiltered,
                data = RelationshipSetup.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/RelationshipSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/RelationshipSetup/{id}/edit.html")]
        public async Task<IActionResult> Edit(int id)
        {

            RelationshipSetupViewModel RelationshipSetupViewModel = new RelationshipSetupViewModel { Id = id, RelationshipName = "",CreatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedDate = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = false };
            if (id != 0)
            {
                RelationshipSetupViewModel = await IRelationshipSetupService.GetRelationshipSetupByIdAsync(id);
            }

            return View("/Areas/Admin/Views/SimpleTransfer/RelationshipSetup/Edit.cshtml", RelationshipSetupViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/RelationshipSetup/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/RelationshipSetup/{id}/edit.html")]

        public async Task<IActionResult> Edit(RelationshipSetupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IRelationshipSetupService.CheckDuplicate(model))
                    {
                        _toastNotification.AddErrorToastMessage("Duplicate Relationship Name.");
                        return View("/Areas/Admin/Views/SimpleTransfer/RelationshipSetup/Edit.cshtml", model);
                    }
                    model = await IRelationshipSetupService.CreateOrUpdate(model);
                    _toastNotification.AddSuccessToastMessage("Relationship is saved.");
                    return Redirect("~/admin" + "/RelationshipSetup/" + model.Id + "/edit.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/SimpleTransfer/RelationshipSetup/Edit.cshtml", model);
            }
            catch (Exception ex)
            {
                //Log.LogError("CountryServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/SimpleTransfer/RelationshipSetup/Edit.cshtml", model);
            }
        }

        [HttpPost]
        [Route("admin/RelationshipSetup/action.html")]
        public async Task<IActionResult> Action(IEnumerable<int> Ids, string action, string page)
        {
            var state = "";
            if (action == "" || action==null)
            {
                _toastNotification.AddErrorToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin/RelationshipSetup.html");
            }
            if (Ids.Count() <= 0)
            {
                _toastNotification.AddErrorToastMessage("Please select atleast one Relationship.");
                return Redirect("~/admin/RelationshipSetup.html");
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
                        result = await IRelationshipSetupService.ActiveRelationshipSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                    {
                        state = ButtonAction.inactive.ToDescription();
                        result = await IRelationshipSetupService.InActiveRelationshipSetupById(item);
                    }
                    else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                    {
                        state = ButtonAction.delete.ToDescription();
                        result = await IRelationshipSetupService.DeleteRelationshipSetupById(item);
                    }
                    if (result)
                    {
                        successCount++;
                    }
                }

            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " Relationship(s) " + state);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddSuccessToastMessage(successCount + " Relationship(s) " + state);
            }
            else
            {
                _toastNotification.AddErrorToastMessage(successCount + " Relationship(s) " + state);
            }

            return Redirect("~/admin/RelationshipSetup.html");

        }
    }
}