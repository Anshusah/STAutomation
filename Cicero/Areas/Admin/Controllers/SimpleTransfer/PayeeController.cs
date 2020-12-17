using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    public class PayeeController : BaseController
    {
        private readonly IUserService IUserService;
        private readonly ICommonService commonService;
        private readonly INotificationService notificationService;
        private readonly IPaymentRequestService paymentRequestService;

        public PayeeController(IUserService _UserService, Utils _Utils, ILogger<PayeeController> _Log, IStatus _status,
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

        [Route("st/payment/index")]
        [Route("st/payment/index.html")]
        public IActionResult Index()
        {
            var dashboardViewModel = new Cicero.Service.Models.JazzCash.DashboardViewModel();
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            //dashboardViewModel.PaymentDetails = GetPaymentDetailsData();
            dashboardViewModel.FullName = IUserService.GetUserFullName().Result;
            dashboardViewModel.CustomerId = IUserService.getLoggedInUserId();
            dashboardViewModel.NotificationCount = notificationService.GetAllNotificationCount(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            dashboardViewModel.RemittanceDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            dashboardViewModel.PaymentDetails = paymentRequestService.GetPaymentRequestDetailsByUserId().Result;
            dashboardViewModel.RemittanceDetails = paymentRequestService.GetRemittanceDetailsByUserId().Result;
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/index.cshtml", dashboardViewModel);
        }

        [Route("payee/transactions")]
        [Route("payee/transactions.html")]
        public IActionResult Transactions()
        {
            var dashboardViewModel = new Cicero.Service.Models.JazzCash.DashboardViewModel();
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            //dashboardViewModel.PaymentDetails = GetPaymentDetailsData();
            dashboardViewModel.FullName = IUserService.GetUserFullName().Result;
            dashboardViewModel.CustomerId = IUserService.getLoggedInUserId();
            dashboardViewModel.NotificationCount = notificationService.GetAllNotificationCount(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            return View("/Themes/SimpleTransfer/Payee/Transactions.cshtml", dashboardViewModel);
        }
        [Route("payee/transactions")]
        [Route("payee/transactions.html")]
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

        [Route("payment/notifications")]
        [Route("payment/notifications.html")]
        public IActionResult Notifications()
        {
            List<NotificationViewModel> notifications = new List<NotificationViewModel>();
            notifications = notificationService.GetAllNotification(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            return View("/Themes/" + this.Theme.GetName(false) + "/Payee/Notifications.cshtml", notifications);
        }

    }
}

