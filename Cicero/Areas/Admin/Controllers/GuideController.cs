using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Service.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GuideController : BaseController
    {
        private readonly IUserService _userService;
        
        public GuideController(IUserService userService) : base(userService)
        {
            _userService = userService;
        }
        // GET: /<controller>/
        [Route("admin/userguide.html")]
        public IActionResult Index()
        {
           string vp = "/Areas/Admin/Views/Guide/Index.cshtml";
            return View(vp);
        }
    }
}
