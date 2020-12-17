using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TenantConfigurationController : BaseController
    {
        private readonly ITenantConfig tenantConfig;
        private readonly IUserService userService;
        private readonly Utils _utils;
        public TenantConfigurationController(ITenantConfig _tenantConfig, IUserService _userService, Utils utils) : base(_userService)
        {
            tenantConfig = _tenantConfig;
            userService = _userService;
            _utils = utils;
         

        }
     
        // GET: /<controller>/
        [HttpGet]
        [Route("admin/tenantsconfiguration.html")]
        [Route("admin/{tenant_identifier}/tenantsconfiguration.html")]
        public IActionResult Index(string caseFormId)
        {
            ViewBag.Count = tenantConfig.GetTenantConfigCount(_utils.DecryptId(caseFormId));
            return View();
            
        }

        [HttpGet]
        [Route("admin/tenantdashboard.html")]
        [Route("admin/{tenant_identifier}/tenantdashboard.html")]
        public IActionResult Dashboard(string caseFormId,bool isActiveOnly)
        {
            
            ViewBag.Count = tenantConfig.GetTenantConfigCount(_utils.DecryptId(caseFormId));
            return View(tenantConfig.GetTenantForm(isActiveOnly));
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/latestcount.html")]
        public IActionResult LastestCount(string caseFormId)
        {
            return Json(tenantConfig.GetTenantConfigCount(_utils.DecryptId(caseFormId)));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/gettenantforms.html")]
        public IActionResult GetTenantForms(bool isActiveOnly)
        {
            return Json(tenantConfig.GetTenantForm(isActiveOnly));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/getsystemdatetime.html")]
        public IActionResult GetSystemDatetime()
        {
            return Json(Convert.ToString(DateTime.Now));
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/checkcaseform.html")]
        public IActionResult CheckCaseForm(string encrypedId) {

            return Json(tenantConfig.CheckCaseFrom(encrypedId));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/deletecreateddata.html")]
        public IActionResult DeleteCreatedData()
        {
            return Json(true);
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/getaddeddata.html")]
        public IActionResult GetAddedData(DateTime startDateTime)
        {
            return Json(tenantConfig.GetTenantAddedData(startDateTime));
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/deletedata.html")]
        public IActionResult DeleteData(string [] roles, string [] users, string [] workflow, string FormId, string delStep )
        {
            return Json(tenantConfig.RevertProcessAsync(roles, users, workflow, FormId,delStep));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/getcurrentdata.html")]
        public IActionResult GetCurrData(string keyId)
        {
            return Json(tenantConfig.GetAdminConfigByKeyId(keyId));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/savecurrentdata.html")]
        public IActionResult SaveCurrData(string keyId, string value)
        {
            return Json(tenantConfig.AddOrUpdateAdminConfig(keyId, value));
        }

        [HttpPost]
        [Route("admin/{tenant_identifier}/delcurrentdata.html")]
        public IActionResult DeleteCurrData(string keyId)
        {
            return Json(tenantConfig.RemoveAdminConfigByKeyId(keyId));
        }

    }
}
