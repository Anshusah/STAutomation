using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    public class PayerController : BaseController
    {
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly INotificationService notificationService;
        private readonly IPaymentRequestService paymentRequestService;

        public PayerController(IUserService _UserService, Utils _Utils, ILogger<PayeeController> _Log, IStatus _status,
          ICaseService _caseService, IRazorToStringRender _razorViewToStringRenderer, ITemplateService _templateService,
          IRoleService _roleService, UserManager<ApplicationUser> _userManager, IEmailSender emailSender,
          ICommonService _commonService, IQueueService _queueService, IFormBuilderService _formbuilderService,
          IFormService formService, IToastNotification toastNotification, IMediaService mediaService, AppSetting _appSetting, ISmsService smsService,
          IOnfidoService onfidoService, IHostingEnvironment HostingEnvironment, IPayeeService PayeeService, IRecipientService recipientService, INotificationService notificationService, IPaymentRequestService paymentRequestService) : base(_UserService)
        {
            IUserService = _UserService;
            commonService = _commonService;
            this.notificationService = notificationService;
            this.paymentRequestService = paymentRequestService;
        }

        [Route("st/payer/index")]
        [Route("st/payer/index.html")]
        public IActionResult Index()
        {
            var dashboardViewModel = new Cicero.Service.Models.JazzCash.DashboardViewModel();
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            //dashboardViewModel.PaymentDetails = GetPaymentDetailsData();
            dashboardViewModel.FullName = IUserService.GetUserFullName().Result;
            dashboardViewModel.CustomerId = IUserService.getLoggedInUserId();
            dashboardViewModel.NotificationCount = notificationService.GetAllNotificationCount(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/index.cshtml", dashboardViewModel);
        }

        [Route("payer/transactions")]
        [Route("payer/transactions.html")]
        public IActionResult Transactions()
        {
            return View("/Themes/SimpleTransfer/PaymentRequest/Transactions.cshtml");
        }
        [Route("payer/transactions")]
        [Route("payer/transactions.html")]
        [HttpPost]
        public JsonResult Transactions(DTPostModel model)
        {
            var payment = paymentRequestService.GetPaymentListByFilter(model);
            return Json(new
            {
                draw = payment.draw,
                recordsTotal = payment.recordsTotal,
                recordsFiltered = payment.recordsFiltered,
                data = payment.data
            });
        }

        [Route("payer/notifications")]
        [Route("payer/notifications.html")]
        public IActionResult Notifications()
        {
            List<NotificationViewModel> notifications = new List<NotificationViewModel>();
            notifications = notificationService.GetAllNotification(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/Notifications.cshtml", notifications);
        }

        [Route("st/payer/login")]
        [Route("st/payer/login.html")]
        public IActionResult Login(int id)
        {
            HttpContext.Session.SetInt32("payerPayment", id);
            return Redirect("~/st/user/login.html");
        }

        [Route("st/payer/register/")]
        [Route("st/payer/register.html")]
        public IActionResult Register(int id)
        {
            HttpContext.Session.SetInt32("payerPayment", id);
            return Redirect("~/st/user/register.html");
        }
    }

}

