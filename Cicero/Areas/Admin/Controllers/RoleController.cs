using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cicero.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Cicero.Service.Helpers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cicero.Data;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Microsoft.Extensions.Logging;
using Core.Status;
using Cicero.Service.Library.Toastr;
using static Cicero.Service.Enums;
using Cicero.Service.Extensions;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoleController : BaseController
    {

        private readonly ApplicationDbContext db;
        private readonly IRoleService IRoleService;
        private readonly IRolePermissionService IRolePermissionService;
        private readonly ILogger<RoleController> Log;
        private readonly IStatus Status;
        private readonly Utils utils;
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly IFormBuilderService formBuilderService;
        private readonly IPermissionService permissionService;
        private readonly IToastNotification _toastNotification;

        public RoleController(ApplicationDbContext _db, IFormBuilderService _formBuilderService, IPermissionService _permissionService, 
            IRoleService _IRoleService, ILogger<RoleController> _Log, IStatus _status, Utils _utils, 
            IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification) : base(_IUserService)
        {
            db = _db;
            IRoleService = _IRoleService;
            Log = _Log;
            Status = _status;
            utils = _utils;
            IRolePermissionService = _IRolePermissionService;
            IUserService = _IUserService;
            commonService = _commonService;
            formBuilderService = _formBuilderService;
            permissionService = _permissionService;
            _toastNotification = toastNotification;
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/roles.html")]
        [Route("admin/{tenant_identifier}/roles.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/roles.html")]
        [Route("admin/{tenant_identifier}/roles.html")]
        public JsonResult Index(DTPostModel model)
        {
            var role = IRoleService.GetRoleListByFilter(model);
            return Json(new
            {
                draw = role.draw,
                recordsTotal = role.recordsTotal,
                recordsFiltered = role.recordsFiltered,
                data = role.data
            });
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/role/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/role/{id}/edit.html")]
        public async Task<IActionResult> Edit(string id)
        {
            RoleViewModel roleViewModel = new RoleViewModel { Id = id, CreatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now), UpdatedAt = Utils.GetDefaultDateFormatToDetail(DateTime.Now), Status = 1 };
            if (id != "0")
            {
                roleViewModel = await IRoleService.GetRoleByIdAsync(id);
            }

            roleViewModel.PermissionList = await IRolePermissionService.GetPermissionListAsync();
            roleViewModel.PermissionListGroup = await IRolePermissionService.GetPermissionGroupViewModels(roleViewModel.PermissionList);
            ViewBag.cases = formBuilderService.GetBuilderFormsForPermission();
            return View(roleViewModel);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/role/{id}/edit.html")]
        [Route("admin/{tenant_identifier}/role/{id}/edit.html")]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    model = await IRoleService.CreateOrUpdate(model);
                    await IRolePermissionService.UpdateRolePermission(model);
                    _toastNotification.AddSuccessToastMessage("Role is saved.");
                    return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/role/" + model.Id + "/edit.html");
                }

                model.PermissionList = await IRolePermissionService.GetPermissionListAsync();
                utils.addModelError(ModelState);
                //Status.Show("success", "Role is Successfully Saved",false);
                //return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/role/" + model.Id + "/edit.html");
                return View(model);
            }
            catch (Exception ex)
            {
                Log.LogError("RoleServices:Edit - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                //return Redirect(this.errorPageUrl);
                model.PermissionList = await IRolePermissionService.GetPermissionListAsync();
                return View(model);
            }
        }

        [Area("Admin")]
        [HttpGet]
        [Route("admin/role/getcases")]
        public JsonResult GetCases()
        {
            return Json(formBuilderService.GetBuilderFormsForPermission());
        }
        [HttpPost]
        [Route("admin/role/createpermissiongroup")]
        public JsonResult Createpermissiongroup(string name)
        {
            bool success = permissionService.CreatePermissionGroup(name);
            if (success)
            {
                PermissionGroupViewModel permissionGroupViewModel = permissionService.GetPermissionGroupViewModelsByTenantId(name);
                return Json(permissionGroupViewModel);
            }
            else
                return Json("false");

        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/role/deletepermissiongroup")]
        public JsonResult Deletepermissiongroup(List<int> permissionGroupIds)
        {
            bool success = permissionService.DeletePermissionGroup(permissionGroupIds);
            //List<PermissionGroupViewModel> permissionGroupViewModels = permissionService.GetPermissionGroupViewModelsByTenantId();
            _toastNotification.AddSuccessToastMessage("Permission deleted.");
            return Json(success);
        }

        [Area("Admin")]
        [HttpPost]
        [Route("admin/role/action.html")]
        [Route("admin/{tenant_identifier}/role/action.html")]
        public async Task<IActionResult> Action(IEnumerable<string> Ids, string action, string page)
        {
            var status = "";
            if (action == "")
            {
                _toastNotification.AddWarningToastMessage("Please select any action from Action select box.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/roles.html");
            }
            if (string.IsNullOrEmpty(Ids.ToString()) || Ids.Count() <= 0)
            {
                _toastNotification.AddWarningToastMessage("Please select atleast one role.");
                return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/roles.html");
            }
            int successCount = 0;
            foreach (var item in Ids)
            {
                bool result = false;
                if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.delete))
                {
                    status = ButtonAction.delete.ToDescription();
                    if (item == ApplicationConstants.AdministratorRoleId || item == ApplicationConstants.CustomerRoleId)
                    {
                        _toastNotification.AddErrorToastMessage("Cannot delete Customer or Administrator Role.");
                        result = false;
                    }
                    result = await IRoleService.DeleteRoleById(item);

                }
                else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.active))
                {
                    status = ButtonAction.active.ToDescription();
                    result = await IRoleService.ActiveRoleById(item);

                }
                else if (action == Enum.GetName(typeof(ButtonAction), ButtonAction.inactive))
                {
                    status = ButtonAction.inactive.ToDescription();
                    result = await IRoleService.InactiveRoleById(item);
                }
                if (result)
                {
                    successCount++;
                }
            }

            if (successCount == Ids.Count())
            {
                _toastNotification.AddSuccessToastMessage(Ids.Count() + " role(s) " + status);
            }
            else if (successCount > 0)
            {
                _toastNotification.AddInfoToastMessage(successCount + " role(s) " + status);
            }
            else
            {
                _toastNotification.AddInfoToastMessage(successCount + " role(s) " + status);
            }
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/roles.html");
        }

        [Area("Admin")]
        [HttpPost(Name = "CheckNameForTenantDuplication")]
        public JsonResult CheckNameForTenantDuplication(string DisplayName, string Id)
        {
            int TenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            return Json(IRoleService.IsDuplicateName(DisplayName, Id, TenantId));

        }


    }
}