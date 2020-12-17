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

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SwimLineController : BaseController
    {
        private readonly IUserService userService;
        private readonly ISwimLineService swimLineService;
        public SwimLineController(IUserService _userService, ISwimLineService _swimLineService) : base(_userService)
        {
            userService = _userService;
            swimLineService = _swimLineService;
        }

        [HttpGet]
        [Route("admin/swimline.html")]
        [Route("admin/{tenant_identifier}/swimline.html")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/getlatestforms.html")]
        public IActionResult GetLatestForms(bool isActiveOnly)
        {
            return Json(swimLineService.GetCaseFromList(isActiveOnly));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/getcasesbyformid.html")]
        public IActionResult GetCasesByFormId(string encryptedId,string searchText)
        {
            return Json(swimLineService.GetCasesByCaseFormId(encryptedId,searchText));
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/swimline/updateposition.html")]
        public IActionResult updatePosition(int newPosition, int oldPosition, int sourceId, int targetId, int claimId,string formId)
        {
            bool success = swimLineService.UpdatePosition(newPosition+1, oldPosition+1, sourceId, targetId, claimId, formId);
            return Json(success);
        }
        [HttpPost]
        [Route("admin/{tenant_identifier}/swimline/checkpermission.html")]
        public IActionResult CheckPermission(string claimId)
        {
            bool hasPermission = swimLineService.CheckPermission(claimId);
            return Json(hasPermission);
        }
        
       
    }
}