using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Areas.Admin.Controllers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Areas.Identity.Pages.Account;
//using Cicero.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.UI.Services;
using Cicero.Service.Models.Core;
using System.Reflection;
using Cicero.Service.Library.Toastr;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cicero.Areas.Admin
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly AppSetting _setting;
        private readonly IUserService UserService;
        private readonly IDashboardService _dashboardService;
        private readonly IActivityLogService activityLogService;
        private readonly ITenantConfig tenantConfig;
        private readonly Utils _utils;
        private readonly IFormBuilderService _formBuilderService;
        private readonly IEmailSender _emailSender;
        private readonly IRazorToStringRender _renderRazorToString;
        private readonly IToastNotification _toastNotification;

        public HomeController(IUserService _UserService, IArticleService articleService, AppSetting setting, 
            IEmailSender emailSender, IRazorToStringRender renderRazorToString, IFormBuilderService formBuilderService, 
            ITenantConfig _tenantConfig, Utils utils, IDashboardService dashboardService, IActivityLogService _activityLogService,
            IToastNotification toastNotification) : base(_UserService)
        {
            _articleService = articleService;
            _setting = setting;
            UserService = _UserService;
            _emailSender = emailSender;
            _renderRazorToString = renderRazorToString;
            _dashboardService = dashboardService;
            activityLogService = _activityLogService;
            tenantConfig = _tenantConfig;
            _utils = utils;
            _formBuilderService = formBuilderService;
            _toastNotification = toastNotification;
        }
        // GET: /<controller>/
        [Authorize]
        [Route("/admin.html")]
        [Route("/admin/{tenant_identifier}.html")]
        public async Task<IActionResult> IndexAsync(string tenant_identifier)
        {
            var checkRole = UserService.UserHasPolicy();
            if (checkRole == "frontend" && UserService.IsSuperAdmin().Result == false)
            {
                return Redirect("~/user/dashboard.html");
            }

            string view = "~/Themes/" + this.Theme.GetName(false) + "/Dashboard/Index.cshtml";
            //string identifierName = this.GetTenantIdentifier();
            ViewBag.Count = tenantConfig.GetTenantConfigCount(0);
            var dashboardData = _dashboardService.GetDashboard();
            dashboardData.LastFourActivities = await activityLogService.GetActivityLogAsync();
            return View(view, dashboardData);
        }

        [Authorize(Policy = "BackOffice")]
        [Route("/admin/{encryptedid}/adminConfig.html")]
        [Route("/admin/{tenant_identifier}/{encryptedid}/adminConfig.html")]
        public IActionResult AdminDashboardContinue(string encryptedid,bool isNew)
        {
            string view = "~/Themes/" + this.Theme.GetName(false) + "/Dashboard/Index1.cshtml";
          
            if(isNew)
            {
                ViewBag.isNew = "true";
            }
            else
            {
                ViewBag.isNew = "false";
            }
           
            int id = _utils.DecryptId(encryptedid);
            ViewBag.Count = tenantConfig.GetTenantConfigCount(id);
            try
            {
                CaseFormViewModel ccvm = new CaseFormViewModel
                {
                    Id = id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                if (id != 0)
                {
                    ccvm = _formBuilderService.GetBuilderFormById(id);
                }
                return View(view,ccvm);
            }
            catch (Exception ex)
            {
                
                return View(view);
            }
        }

        [Route("/admin/logout.html")]
        [Route("/admin/{tenant_identifier}/logout.html")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("tenant_identifier");
            await UserService.Logout();
           // _toastNotification.AddSuccessToastMessage("Logged out.");
            var checkRole = UserService.UserHasPolicy();
            if (checkRole == "frontend")
            {
                return Redirect("/");
            }
            return Redirect("/user/login.html");
            //return Redirect("~/Identity/Account/Login");
        }

        [Area("Admin")]
        [Route("/admin/activity-notification.html")]
        [Route("/admin/{tenant_identifier}/activity-notification.html")]
        public async Task<IActionResult> ActivityNotification()
        {
            var activityLog = await activityLogService.GetUnreadActivityLog("ALL");
            return View(activityLog);
        }

        [Route("/admin/activity-notification-read.html")]
        [Route("/admin/{tenant_identifier}/activity-notification-read.html")]
        public async Task<JsonResult> ActivityNotificationRead()
        {
            var result = await activityLogService.ActivityNotificationRead();

            if (result == true)
            {
                return Json(new { status = "success" });
            }

            return Json(new { status = "failure" });
        }

        //front office section
        public IActionResult Index()
        {
            //object[] args = { 2, 1, new object[] { 23, 34, 45 } };
            //this.Theme.DoAction("before_case_switch", args);
            //var theme = this.Theme.GetComponentsByType("Themes.Blue.Components.CaseAutomation");
            //var ex = theme;
            //var people1 = dataList.BuildQuery(d).ToList();
            //var people1 = d.BuildQuery().ToList();
            //var xx= JsonConvert.SerializeObject(definitions, jsonSerializerSettings);
            //Augment the definitions to show advanced scenarios not
            //handled by GetDefaultColumnDefinitionsForType(...)

            //Let's tweak the generated definition of FirstName to make it
            //a select element in jQuery QueryBuilder UI populated by
            //the possible values from our dataset
            /*
            var firstName = definitions.First(p => p.Field.ToLower() == "firstname");
            firstName.Values = people.Select(p => p.FirstName).Distinct().ToList();
            firstName.Input = "select";

            //Let's tweak birthday to use the jQuery-UI datepicker plugin instead of
            //just a text input.
            var birthday = definitions.First(p => p.Field.ToLower() == "birthday");
            birthday.Plugin = "datepicker";

            ViewBag.FilterDefinition = JsonConvert.SerializeObject(definitions, jsonSerializerSettings);
            ViewBag.Model = people;
            */

            //if (UserService.IsLogin() && UserService.UserHasPolicy().Result == false)
            //{
            //    //return Redirect("~/admin.html");
            //}
            string path = this.Theme.GetName(false);
            string QueryPath = Request.Query["theme"].ToString();
            if (!string.IsNullOrEmpty(QueryPath))
            {
                path = Request.Query["theme"].ToString();
                 
                 
                Type _type = Type.GetType("Themes." + path + ".Setup");
                var o = Activator.CreateInstance(_type, HttpContext);
                object p = Convert.ChangeType(o, Type.GetType("Themes." + path + ".Setup"));
                MethodInfo mi = _type.GetMethod("Init");
                mi.Invoke(p, null);

                _type.GetProperty("ControllerAction").SetValue(p, "Home");
                _type.GetProperty("ControllerName").SetValue(p, "Index");
                this.Theme = p as Theme;
                ViewData["theme"] = p;
                
            }
            if (path == "JazzCash")
            {
                return Redirect("~/admin.html");

            }
            return Redirect("~/admin/form" + _utils.GetTenantForUrl(false) + "/" + _utils.GetTenantForUrl(false) + "/" + _utils.EncryptId(0) + "/edit.html");
        }

        public IActionResult Article(string url)
        {
            ArticleViewModel avm = _articleService.GetArticleByIdentifier(System.IO.Path.GetFileNameWithoutExtension(url));
            if (avm.Id != 0)
            {
                return View();
            }
            return Redirect("404.html");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("~/home/send-email.html")]
        public async Task<JsonResult> SendEmailAsync(string name, string email, string phone, string message)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(message))
            {
                return Json(new
                {
                    status = "error",
                    message = "Please fill in all the information Correctly"
                });
            }

            string emailReceiver = _setting.Get("app_email");

            var fbvm = new FeedBackViewModel()
            {
                Name = name,
                Email = email,
                Phone = phone,
                Message = message
            };

            string body = await _renderRazorToString.RenderViewToStringAsync("/Views/Emails/FeedBackEmail.cshtml", fbvm);

            await _emailSender.SendEmailAsync(emailReceiver, name, body);

            //_status.Show("success", "Email Sent Successfully.", false);

            return Json(new
            {
                status = "success",
                message = "Email has been successfully sent"
            });
        }

        [Route("~/home/feed-back.html")]
        public IActionResult FeedBackEmail()
        {

            return View("~/Views/Emails/FeedBackEmail.cshtml");
        }


    }
}
