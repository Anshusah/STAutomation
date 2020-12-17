using Cicero.Service.Services;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Core.Status;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using System.Collections.Generic;
using Cicero.Service.Models;
using Cicero.Service.Library.Toastr;

namespace Cicero.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SettingController : BaseController
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService userService;
        private readonly IStatus status;
        private readonly ITenantService tenantService;
        private readonly Utils utils;
        private readonly IMapper mapper;
        private readonly IToastNotification _toastNotification;
        private readonly ICommonService commonService;
        private readonly SimpleTransferApplicationDbContext stdb;

        public SettingController(ApplicationDbContext adb,IStatus _status, IUserService _userService,ITenantService _tenantService,
            Utils _utils, IMapper _mapper, IToastNotification toastNotification,ICommonService _commonService, SimpleTransferApplicationDbContext _stdb) : base( _userService)
        {
            db = adb;
            status = _status;
            userService = _userService;
            tenantService = _tenantService;
            utils = _utils;
            mapper = _mapper;
            _toastNotification = toastNotification;
            commonService = _commonService;
            stdb = _stdb;
        }

        [Route("admin/setting.html")]
        [Route("admin/{tenant_identifier}/setting.html")]
        public ActionResult Index()
        {
            var id = tenantService.GetTenantIdByIdentifier(GetTenantIdentifier());
            if (id == 0)
            {
                return Redirect("~/admin/" + utils.GetTenantForUrl(true) + "setting.html");
            }
            var setting = mapper.Map<List<SettingViewModel>>(db.Setting.Where(x => x.TenantId == id).ToList());

            setting.Add(mapper.Map<SettingViewModel>(db.Setting.Where(x => x.TenantId == null && x.FieldKey.Equals("app_themes", StringComparison.OrdinalIgnoreCase)).FirstOrDefault()));

            return View(setting);
        }

        [HttpPost]
        [Route("admin/setting.html")]
        [Route("admin/{tenant_identifier}/setting.html")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection data)
        {
            var id = tenantService.GetTenantIdByIdentifier(GetTenantIdentifier());
            if(id==0){
                _toastNotification.AddWarningToastMessage("Please choose a Tenant.");
                return Redirect("~/admin/" + utils.GetTenantForUrl(true) + "setting.html");
            }
            using (db)
            {
                var Forms = data.Keys;
                foreach (var _Form in Forms)
                {
                    var _SettingModel = db.Setting.Where(x => x.FieldKey == _Form && x.TenantId == id).SingleOrDefault();
                    if (_SettingModel != null)
                    {
                        _SettingModel.FieldValue = Request.Form[_Form].ToString();
                        db.SaveChanges();

                    }
                }
            }
            _toastNotification.AddSuccessToastMessage("Setting is saved.");
            return Redirect("~/admin" + utils.GetTenantForUrl(false) + "/setting.html");
        }
        [HttpGet]
        [Route("admin/uatsetting.html")]
        public async Task<IActionResult> UatSetting()
        {
            UatSettingViewModel uatSetting = await commonService.GetUatSettingByIdAsync();           
            return View("/Areas/Admin/Views/Setting/UatSetting.cshtml", uatSetting);
        }

        [HttpPost]
        [Route("admin/uatsetting.html")]
        public async Task<IActionResult> UatSetting(UatSettingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = 1;
                    model = await commonService.CreateOrUpdateUatSetting(model);
                    _toastNotification.AddSuccessToastMessage("Phone Number is saved.");
                    return Redirect("~/admin/uatsetting.html");
                }

                utils.addModelError(ModelState);
                return View("/Areas/Admin/Views/Setting/UatSetting.cshtml", model);
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Areas/Admin/Views/UatSetting/Edit.cshtml", model);
            }
        }
    }
}