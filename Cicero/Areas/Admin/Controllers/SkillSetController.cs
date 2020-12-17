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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SkillSetController : BaseController
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
        private readonly ISkillSetService _skillSetService;


        public SkillSetController(ApplicationDbContext _db, IFormBuilderService _formBuilderService, IPermissionService _permissionService,
           IRoleService _IRoleService, ILogger<RoleController> _Log, IStatus _status, Utils _utils,
           IRolePermissionService _IRolePermissionService, IUserService _IUserService, ICommonService _commonService, IToastNotification toastNotification,
           ISkillSetService skillSetService) : base(_IUserService)
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
            _skillSetService = skillSetService;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("admin/manage/getAllSkill")]
        [Route("admin/{tenant_identifier}/manage/getAllSkill")]
        public IActionResult GetAllSkills()
        {

            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            List<SkillSetViewModel> data = _skillSetService.GetSkillSets(tenantid);
            ViewBag.status = "";
            ViewBag.message = "";
            return PartialView("~/Areas/Admin/Views/SkillSet/_AllSkills.cshtml", data);
        }
        [HttpPost]
        [Route("admin/manage/addNewSkill")]
        [Route("admin/{tenant_identifier}/manage/addNewSkill")]
        public IActionResult AddNewSkill(string title, int caselimit)
        {
            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            SkillSetViewModel skillSetViewModel = new SkillSetViewModel()
            {
                Title = title,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                TenantId = tenantid,
                CaseLimit = caselimit,
                CreatedBy = commonService.getLoggedInUserId()
            };
            if(title==string.Empty || title == null)
            {

                ViewBag.status = "error";
                ViewBag.message = "Title couldn't be empty";
                
            }
            else if(caselimit < 0 || caselimit > 1000)
            {
                ViewBag.status = "error";
                ViewBag.message = "Case Limit cannot be greater than 1000 or less than 0";
            }
            else
            {
                _skillSetService.CreateOrUpdateSkillSet(skillSetViewModel);
                ViewBag.status = "success";
                ViewBag.message = "Saved successfully";

            }

            List<SkillSetViewModel> data = _skillSetService.GetSkillSets(tenantid);
            return PartialView("~/Areas/Admin/Views/SkillSet/_AllSkills.cshtml", data);
        }
        [HttpPost]
        [Route("admin/manage/delSkill")]
        [Route("admin/{tenant_identifier}/manage/delSkill")]
        public IActionResult DeleteSkill(int skillId)
        {
            JObject result = _skillSetService.DeleteSkill(skillId);
            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            List<SkillSetViewModel> data = _skillSetService.GetSkillSets(tenantid);
            ViewBag.status = result["status"] ;
            ViewBag.message = result["message"];
            return PartialView("~/Areas/Admin/Views/SkillSet/_AllSkills.cshtml", data);
        }

        [HttpPost]
        [Route("admin/manage/swapSkill")]
        [Route("admin/{tenant_identifier}/manage/swapSkill")]
        public IActionResult SwapSkill(int exchangeFrom, int exchangeTo)
        {
            JObject result = _skillSetService.ExchangeSkillSet(exchangeFrom, exchangeTo);
            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
            List<SkillSetViewModel> data = _skillSetService.GetSkillSets(tenantid);
            ViewBag.status = result["status"];
            ViewBag.message = result["message"];
            return PartialView("~/Areas/Admin/Views/SkillSet/_AllSkills.cshtml", data);

        }

        [HttpPost]
        [Route("admin/manage/editSkill")]
        [Route("admin/{tenant_identifier}/manage/editSkill")]
        public IActionResult EditSkill(IFormCollection formData)
        {

            int tenantid = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());

            SkillSetViewModel skillSetViewModel = new SkillSetViewModel()
            {
                Id = Convert.ToInt16(formData["id"]),
                IsActive = Convert.ToBoolean(formData["isActive"]),
                CreatedAt = Convert.ToDateTime(formData["createdAt"]),
                CreatedBy = Convert.ToString(formData["createdBy"]),
                UpdatedAt = DateTime.Now,
                Title = Convert.ToString(formData["title"]),
                CaseLimit= Convert.ToInt16(formData["caseLimit"]),
                TenantId = tenantid
            };
            if (skillSetViewModel.Title == string.Empty || skillSetViewModel == null)
            {

                ViewBag.status = "error";
                ViewBag.message = "Title couldn't be empty";

            }
            else if (skillSetViewModel.CaseLimit < 0 || skillSetViewModel.CaseLimit > 1000)
            {
                ViewBag.status = "error";
                ViewBag.message = "Case Limit cannot be greater than 1000 or less than 0";
            }
            else
            {
                _skillSetService.CreateOrUpdateSkillSet(skillSetViewModel);
                ViewBag.status = "success";
                ViewBag.message = "Saved successfully";
            }

            List<SkillSetViewModel> data = _skillSetService.GetSkillSets(tenantid);
            return PartialView("~/Areas/Admin/Views/SkillSet/_AllSkills.cshtml", data);
        }
    }
}