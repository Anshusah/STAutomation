using AutoMapper.Configuration;
using Cicero.Data.Entities;
using Cicero.Data.Extensions;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.JazzCash;
using Cicero.Service.Models.PaymentRequest;
using Cicero.Service.Services;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cicero.Service.Enums;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using static Cicero.Data.Enumerations;

namespace Cicero.Areas.Admin.Controllers.SimpleTransfer
{
    [ApiExplorerSettings(IgnoreApi = true)]

    public class PaymentRequestController : BaseController
    {
        private readonly IUserService IUserService;
        private readonly IRoleService roleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Utils Utils;
        private readonly ILogger<PaymentRequestController> Log;
        private readonly IStatus status;
        private readonly IEmailSender _EmailSender;
        private readonly ICommonService commonService;
        private readonly ICaseService caseService;
        private readonly IQueueService queueService;
        private readonly ITemplateService templateService;
        private readonly IFormBuilderService formbuilderService;
        private readonly IFormService _formService;
        private readonly IRazorToStringRender razorViewToStringRenderer;
        private readonly IToastNotification _toastNotification;
        private readonly IMediaService mediaService;
        private readonly AppSetting appSetting = null;
        private readonly ISmsService smsService;
        private readonly IOnfidoService onfidoService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IPayerService payerService;
        private readonly IRecipientService recipientService;
        private readonly INotificationService notificationService;
        private readonly IPaymentRequestService paymentRequestService;
        private readonly IConfiguration config;
        private readonly ICustomerService customerService;
        private readonly ITransactionLimitConfigService transactionLimitConfigService;

        public PaymentRequestController(IUserService _UserService, Utils _Utils, ILogger<PaymentRequestController> _Log, IStatus _status,
          ICaseService _caseService, IRazorToStringRender _razorViewToStringRenderer, ITemplateService _templateService,
          IRoleService _roleService, UserManager<ApplicationUser> _userManager, IEmailSender emailSender,
          ICommonService _commonService, IQueueService _queueService, IFormBuilderService _formbuilderService,
          IFormService formService, IToastNotification toastNotification, IMediaService mediaService, AppSetting _appSetting, ISmsService smsService,
          IOnfidoService onfidoService, IHostingEnvironment HostingEnvironment, IPayerService payerService, IRecipientService recipientService,
          INotificationService notificationService, IPaymentRequestService paymentRequestService, IConfiguration config, ICustomerService customerService, ITransactionLimitConfigService transactionLimitConfigService) : base(_UserService)
        {
            roleService = _roleService;
            IUserService = _UserService;
            Utils = _Utils;
            Log = _Log;
            status = _status;
            userManager = _userManager;
            _EmailSender = emailSender;
            commonService = _commonService;
            caseService = _caseService;
            templateService = _templateService;
            queueService = _queueService;
            formbuilderService = _formbuilderService;
            _formService = formService;
            razorViewToStringRenderer = _razorViewToStringRenderer;
            _toastNotification = toastNotification;
            this.mediaService = mediaService;
            appSetting = _appSetting;
            this.smsService = smsService;
            this.onfidoService = onfidoService;
            hostingEnvironment = HostingEnvironment;
            this.payerService = payerService;
            this.recipientService = recipientService;
            this.notificationService = notificationService;
            this.paymentRequestService = paymentRequestService;
            this.config = config;
            this.customerService = customerService;
            this.transactionLimitConfigService = transactionLimitConfigService;
        }

        [Route("payment/request/onlinepayment")]
        [Route("payment/request/onlinepayment.html")]
        public IActionResult OnlinePayment(int id)
        {
            var data = paymentRequestService.GetPaymentRequestData(id).Result;
            if (data == null || data.Status != (int)PaymentRequestStatus.PaymentRequestPending)
            {
                return Redirect("~/user/dashboard.html");
            }
            //if (data == null)
            //{
            //    data = new PaymentRequestViewModel();
            //    data.Id = 1;
            //    data.PayeeName = "John Doe";
            //    data.RequestAmount = 2300;
            //    data.Status = 1;
            //    data.RequestId = "asd7878";
            //    data.PayerEmail = "johndoe@sample.com";
            //}
            // HttpContext.Session.SetString("id", data.Id.ToString());
            return View("/Themes/" + this.Theme.GetName(false) + "/Payer/OnlinePayment.cshtml", data);
        }

        [Route("payment/requestpayment")]
        [Route("payment/requestpayment.html")]
        public IActionResult RequestPayment()
        {

            PaymentRequestViewModel paymentRequest = HttpContext.Session.GetComplexData<PaymentRequestViewModel>("paymentRequest");
            string folderpath = hostingEnvironment.ContentRootPath;
            var fullFolderPath = folderpath + "\\wwwroot\\uploads\\temp\\";

            if (paymentRequest != null)
            {
                string filePath = fullFolderPath + paymentRequest.STPaymentRequestDetails.Invoice;
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var formFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    paymentRequest.STPaymentRequestDetails.InvoiceFile = formFile;
                }
            }

            if (paymentRequest == null)
            {
                paymentRequest = new PaymentRequestViewModel();
                var loggedinuser = commonService.getLoggedInUserId();
                paymentRequest.PayeeCountry = customerService.GetCustomerCountryCode(loggedinuser).Result;
                paymentRequest.STPaymentRequestDetails = new STPaymentRequestDetailsViewModel();
                paymentRequest.STPaymentRequestDetails.DueDate = DateTime.Now;
            }
            return View("/Themes/SimpleTransfer/PaymentRequest/RequestPayment.cshtml", paymentRequest);
        }
        //[HttpPost]
        //[Route("payment/requestpayment")]
        //[Route("payment/requestpayment.html")]
        //public async Task<IActionResult> RequestPayment(PaymentRequestViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //model = await paymentRequestService.CreateOrUpdate(model);
        //            //_toastNotification.AddSuccessToastMessage("Payment Requested.");
        //            return RedirectToAction("~/payment/requestpaymentsummary.html");
        //        }
        //        Utils.addModelError(ModelState);
        //        return Redirect("~/user/dashboard.html");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.LogError("Payment Request - " + ex.ToString());
        //        _toastNotification.AddErrorToastMessage(ex.ToString());
        //        return Redirect("~/user/dashboard.html");
        //    }
        //}

        [Route("payment/requestpaymentsummary")]
        [Route("payment/requestpaymentsummary.html")]
        public IActionResult RequestPaymentSummary()
        {
            PaymentRequestViewModel paymentRequestViewModel = HttpContext.Session.GetComplexData<PaymentRequestViewModel>("paymentRequest");

            return View("/Themes/SimpleTransfer/PaymentRequest/RequestPaymentSummary.cshtml", paymentRequestViewModel);
        }
        [HttpPost]
        [Route("payment/requestpaymentsummary")]
        [Route("payment/requestpaymentsummary.html")]
        public async Task<IActionResult> RequestPaymentSummary(PaymentRequestViewModel model)
        {

            try
            {
                ModelState.Clear();
                model = HttpContext.Session.GetComplexData<PaymentRequestViewModel>("paymentRequest");
                model.STPaymentRequestDetails.InvoiceFile = new FormFile(null, 0, 0, "", "");
                TryValidateModel(model);
                if (ModelState.IsValid)
                {
                    string folderpath = hostingEnvironment.ContentRootPath;
                    var fullTempFolderPath = folderpath + "\\wwwroot\\uploads\\temp\\";
                    string tempFilePath = fullTempFolderPath + model.STPaymentRequestDetails.Invoice;
                    var fullFolderPath = folderpath + "\\wwwroot\\uploads\\";
                    string filePath = fullFolderPath + model.STPaymentRequestDetails.Invoice;

                    // To move a file or folder to a new location:
                    System.IO.File.Move(tempFilePath, filePath);

                    var callbackUrl = config.GetSection("BaseApiUrl").Value + "payment/request/onlinepayment?id=";
                    model = await paymentRequestService.CreateOrUpdate(model, callbackUrl);
                    _toastNotification.AddSuccessToastMessage("Payment Requested.");
                    HttpContext.Session.Remove("paymentRequest");
                    TempData["paymentRequestId"] = model.Id;
                    // HttpContext.Session.SetInt32("paymentRequestId", model.Id);
                    return RedirectToAction("RequestSuccess");
                }
                return View("/Themes/SimpleTransfer/PaymentRequest/RequestPaymentSummary.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.LogError("Payment Request - " + ex.ToString());
                _toastNotification.AddErrorToastMessage(ex.ToString());
                return View("/Themes/SimpleTransfer/PaymentRequest/RequestPaymentSummary.cshtml", model);
            }
        }
        [Route("payment/requestsuccess")]
        [Route("payment/requestsuccess.html")]
        public IActionResult RequestSuccess()
        {
            var id = Convert.ToInt32(TempData["paymentRequestId"]);//HttpContext.Session.GetInt32("paymentRequestId");
            TempData["paymentRequestId"] = id;

            PaymentRequestViewModel data = paymentRequestService.GetPaymentRequestData(Convert.ToInt32(id)).Result;
            return View("/Themes/SimpleTransfer/PaymentRequest/RequestSuccess.cshtml", data);
        }

        [Route("payment/transactions")]
        [Route("payment/transactions.html")]
        public IActionResult Transactions()
        {
            var dashboardViewModel = new Cicero.Service.Models.JazzCash.DashboardViewModel();
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            //dashboardViewModel.PaymentDetails = GetPaymentDetailsData();
            dashboardViewModel.FullName = IUserService.GetUserFullName().Result;
            dashboardViewModel.CustomerId = IUserService.getLoggedInUserId();
            dashboardViewModel.NotificationCount = notificationService.GetAllNotificationCount(commonService.getLoggedInUserId(), commonService.GetRoleIdByUserId(commonService.getLoggedInUserId()));
            dashboardViewModel.PaymentDetails = new List<Service.Models.JazzCash.PaymentDetails>();
            return View("/Themes/SimpleTransfer/PaymentRequest/Transactions.cshtml", dashboardViewModel);
        }
        [Route("payment/transactions")]
        [Route("payment/transactions.html")]
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

        [Route("payment/request")]
        [Route("payment/request.html")]
        public IActionResult PaymentRequest(string receiverCountry)
        {
            PaymentRequestViewModel paymentRequest = new PaymentRequestViewModel();
            paymentRequest.PayeeCountry = receiverCountry;
            paymentRequest.STPaymentRequestDetails = new STPaymentRequestDetailsViewModel();
            paymentRequest.STPaymentRequestDetails.DueDate = DateTime.Now;
            return PartialView("/Themes/SimpleTransfer/PaymentRequest/_RequestPayment.cshtml", paymentRequest);
        }

        [HttpPost]
        [Route("payment/request")]
        [Route("payment/request.html")]
        public IActionResult PaymentRequest(PaymentRequestViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userId = commonService.getLoggedInUserId();
                    if (userId != null && userId != "")
                    {
                        var user = commonService.GetUserById(userId).Result;
                        if (user != null && user.Email == data.PayerEmail)
                        {
                            return Ok("failure");
                        }
                    }

                    var formFile = data.STPaymentRequestDetails.InvoiceFile;
                    if (formFile != null && formFile.Length > 0)
                    {
                        var extension = System.IO.Path.GetExtension(formFile.FileName);
                        string filename = System.Guid.NewGuid() + "." + extension;
                        string folderpath = hostingEnvironment.ContentRootPath;
                        var fullFolderPath = folderpath + "\\wwwroot\\uploads\\temp\\";
                        string filePath = fullFolderPath + filename;

                        bool exists = System.IO.Directory.Exists(fullFolderPath);

                        if (!exists)
                            System.IO.Directory.CreateDirectory(fullFolderPath);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            formFile.CopyToAsync(stream).Wait();
                            data.STPaymentRequestDetails.OldInvoice = formFile.FileName;
                            data.STPaymentRequestDetails.InvoiceFile = null;
                        }
                        data.STPaymentRequestDetails.Invoice = filename;
                    }
                    data.RequestId = RandomString(10);
                    string loggedinUser = commonService.getLoggedInUserId();
                    HttpContext.Session.SetComplexData("paymentRequest", data);
                    if (loggedinUser == null || loggedinUser == "")
                    {
                        return Ok("redirect");
                    }

                    return Ok("success");
                }

                return Ok("failed");
            }
            catch (Exception ex)
            {
                return Ok(StatusCodes.Status500InternalServerError);
            }

        }

        private string RandomString(int size, bool lowerCase = false)
        {
            Random _random = new Random();
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        //Transactions details

        [Route("payment/transactiondetails")]
        [Route("payment/transactiondetails.html")]
        public IActionResult TransactionDetails()
        {
            return View("/Themes/SimpleTransfer/PaymentRequest/TransactionDetails.cshtml");
        }

        [Route("payment/reminder")]
        [Route("payment/reminder.html")]
        public IActionResult PaymentReminder(int id, string reminder)
        {
            try
            {
                var result = paymentRequestService.SetReminder(id, reminder).Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("payment/request/payerData")]
        [Route("payment/request/payerData.html")]
        public IActionResult PayerData(int id)
        {
            try
            {
                var data = paymentRequestService.GetPaymentRequestData(id).Result;
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(StatusCodes.Status500InternalServerError);
            }

        }

        [Route("payment/request/data")]
        [Route("payment/request/data.html")]
        public IActionResult PaymentRequestData(int id, int type = 2)
        {
            try
            {
                var data = new PaymentRequestDetails();
                if (type == (int)TransactionType.PaymentRequest)
                {

                    data = paymentRequestService.GetPaymentRequestDatas(id).Result;
                }
                else
                {
                    data = paymentRequestService.GetRemittanceDatas(id).Result;
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }

        }

        [Area("Admin")]
        [HttpGet(Name = "CheckAmountLimit")]
        public bool CheckAmountLimit(decimal RequestAmount, string PayeeCountry)
        {
            try
            {
                var limitDatas = transactionLimitConfigService.GetTransactionLimitConfigByCountryCodeAsync(PayeeCountry).Result;
                if (limitDatas != null)
                {
                    if (RequestAmount > limitDatas.LimitAmountPerTxn)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
