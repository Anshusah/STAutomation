using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [Route("st/api")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class InternalApiController : BaseController
    {
        private readonly ICommonService commonService;

        public InternalApiController(ICommonService commonService,
            IUserService _IUserService) : base(_IUserService)
        {
            this.commonService = commonService;
        }


        [HttpGet]
        [Route("getloggedinuserid")]

        public IActionResult GetUserId()
        {
            var userid= commonService.getLoggedInUserId();
            return Ok(userid);
        }

    }
}