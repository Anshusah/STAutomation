using AutoMapper;
using Cicero.Data;
using Cicero.Data.Entities;
using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Cicero.Service.Enums;
using Cicero.Service.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Cicero.Service.Models.PaymentRequest;
using Cicero.Data.Entities.SimpleTransfer;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
using static Cicero.Service.SimpleTransferEnum;
using Cicero.Service.Models.JazzCash;
using static Cicero.Service.Extensions.Extensions;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface IPaymentRequestService
    {
        Task<List<PaymentDetails>> GetPaymentRequestDetailsByUserId();
        Task<List<PaymentDetails>> GetRemittanceDetailsByUserId();
        Task<List<PaymentDetails>> GetPaymentDetailsByUserId();
        List<SelectListItem> GetAllPayerList();
        Task<PaymentRequestViewModel> CreateOrUpdate(PaymentRequestViewModel model, string callbackUrl);
        Task<bool> SendEmail(string email, int id, string callbackUrl);
        List<SelectListItem> GetPayerList(string payeeId);
        List<PaymentRequestViewModel> GetPayemntRequestList();
        DTResponseModel GetPaymentListByFilter(DTPostModel model);
        Task<PaymentRequestViewModel> GetPaymentRequestData(int id);
        Task<PaymentRequestDetails> GetPaymentRequestDatas(int id);
        Task<PaymentRequestDetails> GetRemittanceDatas(int id);
        Task<bool> SetReminder(int id, string reminder);
    }
    public class PaymentRequestService : IPaymentRequestService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<IPaymentRequestService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITemplateService templateService;
        private readonly IEmailSender emailSender;
        private readonly IRazorToStringRender razorViewToStringRenderer;
        private readonly IUserService userService;
        private readonly ICustomerService customerService;
        private readonly ICountryService countryService;

        public PaymentRequestService(SimpleTransferApplicationDbContext _db, ILogger<IPaymentRequestService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, UserManager<ApplicationUser> _UserManager, ITemplateService templateService, IEmailSender emailSender, IRazorToStringRender razorViewToStringRenderer, IUserService userService, ICustomerService customerService, ICountryService countryService)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            userManager = _UserManager;
            this.templateService = templateService;
            this.emailSender = emailSender;
            this.razorViewToStringRenderer = razorViewToStringRenderer;
            this.userService = userService;
            this.customerService = customerService;
            this.countryService = countryService;
        }
        public DTResponseModel GetPaymentListByFilter(DTPostModel model)
        {
            string searchBy = string.Empty;
            int skip = 0;
            int take = 10;
            string sortBy = "amount";
            bool sortDir = false;
            int totalResultsCount = 0;
            int filteredResultsCount = 0;
            int draw = 0;

            if (model != null)
            {
                searchBy = (model.search != null) ? model.search.value : null;
                take = model.length;
                skip = model.start;
                draw = model.draw;

                if (model.order != null)
                {
                    sortBy = model.columns[model.order[0].column].data;
                    sortDir = model.order[0].dir.ToLower() == "desc";
                }
            }
            string payeeId = Convert.ToString(commonService.getLoggedInUserId());
            var user = commonService.GetUserById(payeeId);
            var countryCode = countryService.GetCountryCodeByUserId(payeeId).Result;
            if (payeeId != null && payeeId != "")
            {
                countryCode = countryService.GetCountryCodeByUserId(payeeId).Result;
            }

            var paymentList = (from c in db.STPaymentRequest.Where(x => x.PayeeId == payeeId || x.PayerEmail == user.Result.Email).OrderBy(x => x.CreatedDate)
                               select new
                               {
                                   id = c.Id,
                                   description = db.STPaymentRequestDetails.Where(x => x.STPaymentRequestId == c.Id.ToString()).Select(x => x.Description).FirstOrDefault(),//c.PayerId != "" ? "Requested to you" : "Requested by you",
                                   amount = c.RequestAmount.ToString(),
                                   date = Utils.GetDefaultDateFormatToDetail(c.CreatedDate),
                                   status = c.Status.ToString(),
                                   payerId = c.PayerId,
                                   action = c.PayeeId == payeeId ? "" : (c.PayerEmail == user.Result.Email && c.Status == (int)PaymentRequestStatus.PaymentRequestPending && countryCode == "GB") ? "<a href='/admin/form/jazzcash/jazzcash/" + utils.EncryptId(0) + "/edit.html?id=" + utils.EncryptId(c.Id) + "' class='btn btn-sm btn-outline-primary'>Pay Now</a>" : ""
                               });

            if (!String.IsNullOrEmpty(searchBy))
            {
                var searchByLower = searchBy.ToLower();
                paymentList = paymentList.Where(o => o.amount.ToLower().Contains(searchByLower) || o.status.ToLower().Contains(searchByLower));

            }

            totalResultsCount = paymentList.Count();
            //paymentList = paymentList.OrderBy(sortBy, sortDir).Skip(skip).Take(take);
            var list = paymentList.ToList();
            filteredResultsCount = list.Count();

            return new DTResponseModel
            {
                draw = draw,
                recordsTotal = filteredResultsCount,
                recordsFiltered = totalResultsCount,
                data = list
            };

        }
        public async Task<PaymentRequestViewModel> CreateOrUpdate(PaymentRequestViewModel model, string callbackUrl)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                var paymentRequest = new STPaymentRequest();
                model.PayeeId = commonService.getLoggedInUserId();
                var payerId = db.Users.Where(x => x.Email.ToLower().Trim() == model.PayerEmail.ToLower().Trim()).FirstOrDefault();
                model.PayerId = payerId == null ? "" : payerId.Id;
                model.Status = (int)STPaymentRequestStatus.Pending;
                paymentRequest = Mapper.Map<PaymentRequestViewModel, STPaymentRequest>(model);
                paymentRequest.CreatedBy = commonService.getLoggedInUserId();
                paymentRequest.UpdatedBy = commonService.getLoggedInUserId();
                paymentRequest.CreatedDate = DateTime.Now;
                paymentRequest.UpdatedDate = DateTime.Now;

                var paymentRequestDetails = new STPaymentRequestDetails();
                paymentRequestDetails = Mapper.Map<STPaymentRequestDetailsViewModel, STPaymentRequestDetails>(model.STPaymentRequestDetails);
                paymentRequestDetails.CreatedBy = commonService.getLoggedInUserId();
                paymentRequestDetails.UpdatedBy = commonService.getLoggedInUserId();
                paymentRequestDetails.CreatedDate = DateTime.Now;
                paymentRequestDetails.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    paymentRequest.CreatedBy = paymentRequest.UpdatedBy;
                    paymentRequest.CreatedDate = paymentRequest.UpdatedDate;
                    db.STPaymentRequest.Add(paymentRequest);
                    await db.SaveChangesAsync();

                    paymentRequestDetails.STPaymentRequestId = paymentRequest.Id.ToString();
                    paymentRequestDetails.CreatedBy = paymentRequest.UpdatedBy;
                    paymentRequestDetails.CreatedDate = paymentRequest.UpdatedDate;
                    db.STPaymentRequestDetails.Add(paymentRequestDetails);
                    await db.SaveChangesAsync();

                    var payeeName = await userService.GetUserFullName();
                    callbackUrl = callbackUrl + paymentRequest.Id;
                    string userId = commonService.getLoggedInUserId();
                    var payeeData = await customerService.GetCustomerById(userId);
                    var result = await SendEmailPayee(payeeData.Email, payeeData.FirstName, model.PayerName, model.STPaymentRequestDetails.DueDate.ToString("yyyy/MM/dd"), model.RequestId, model.STPaymentRequestDetails.Description, model.RequestAmount);
                    result = await SendEmail(model.PayerEmail, payeeName, model.PayerName, model.STPaymentRequestDetails.DueDate.ToString("yyyy/MM/dd"), model.RequestId, model.STPaymentRequestDetails.Description, model.RequestAmount, callbackUrl);
                    //    await SendEmail(model.PayerEmail, model.Id, "");
                    return Mapper.Map<PaymentRequestViewModel>(paymentRequest);
                }
                else
                {
                    db.STPaymentRequest.Attach(paymentRequest).State = EntityState.Modified;
                    db.STPaymentRequestDetails.Attach(paymentRequestDetails).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<SelectListItem> GetPayerList(string payeeId)
        {
            var payerList = db.Users.Select(x => new SelectListItem() { Text = x.UserName, Value = x.UserId }).ToList();
            return payerList;
        }

        public List<SelectListItem> GetAllPayerList()
        {
            string payeeId = commonService.getLoggedInUserId();
            var payerList = db.STPaymentRequest.Where(x => x.PayeeId == payeeId).Select(x => new SelectListItem() { Text = x.PayerName, Value = x.Id.ToString() }).GroupBy(x => x.Text).Select(x => x.First()).ToList();
            return payerList;
        }

        public List<PaymentRequestViewModel> GetPayemntRequestList()
        {
            string payerId = Convert.ToString(commonService.getLoggedInUserId());
            List<STPaymentRequest> paymentList = db.STPaymentRequest.Where(x => x.PayerId == payerId || x.PayeeId == payerId).ToList();
            return Mapper.Map<List<PaymentRequestViewModel>>(paymentList);
        }

        public async Task<bool> SendEmail(string email, string name, string payerName, string dueBy, string requestId, string description, decimal amount, string callbackUrl)
        {
            try
            {
                string settings = templateService.GetEmailGeneralSetting();
                JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                int key = (int)EmailSettingFor.PaymentRequest;
                int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                var user = await userManager.FindByEmailAsync(email);
                string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, "", true);
                var messageNew = new TemplateEmailViewModel { };
                content = content.Replace("[amount]", amount.ToString());
                content = content.Replace("[name]", name);
                content = content.Replace("[payername]", payerName);
                content = content.Replace("[dueby]", dueBy);
                content = content.Replace("[requestid]", requestId);
                content = content.Replace("[description]", description);
                content = content.Replace("[paynow]", callbackUrl);
                content = content.Replace("[moreinfo]", callbackUrl);
                messageNew.Content = content;
                string body = razorViewToStringRenderer.RenderViewToStringAsync("Helper/EmailTemplate-ST.cshtml", messageNew).GetAwaiter().GetResult();
                await emailSender.SendEmailAsync(email, templateViewModel.Subject, body);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendEmailPayee(string email, string name, string payerName, string dueBy, string requestId, string description, decimal amount)
        {
            try
            {
                string settings = templateService.GetEmailGeneralSetting();
                JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                int key = (int)EmailSettingFor.PaymentRequestSuccess;
                int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                var user = await userManager.FindByEmailAsync(email);
                string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, "", "", true);
                var messageNew = new TemplateEmailViewModel { };
                content = content.Replace("[amount]", amount.ToString());
                content = content.Replace("[name]", name);
                content = content.Replace("[payername]", payerName);
                content = content.Replace("[dueby]", dueBy);
                content = content.Replace("[requestid]", requestId);
                content = content.Replace("[description]", description);
                messageNew.Content = content;
                string body = razorViewToStringRenderer.RenderViewToStringAsync("Helper/emailTemplate-RequestSuccess.cshtml", messageNew).GetAwaiter().GetResult();
                await emailSender.SendEmailAsync(email, templateViewModel.Subject, body);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendEmail(string email, int id, string callbackUrl)
        {
            try
            {
                string settings = templateService.GetEmailGeneralSetting();
                JObject mailObject = (JObject)JsonConvert.DeserializeObject(settings);
                int key = (int)EmailSettingFor.PaymentRequest;
                int templateId = Convert.ToInt16(mailObject[key.ToString()]);
                TemplateViewModel templateViewModel = templateService.GetTemplateById(templateId);
                var user = await userManager.FindByEmailAsync(email);
                string content = templateService.CreateEmailTemplate(templateViewModel.Content, 0, 0, 0, callbackUrl, "", true);
                var messageNew = new TemplateEmailViewModel { };
                messageNew.Content = content;
                string body = razorViewToStringRenderer.RenderViewToStringAsync("Helper/TemplateEmail.cshtml", messageNew).GetAwaiter().GetResult();
                await emailSender.SendEmailAsync(email, templateViewModel.Subject, body);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<PaymentRequestViewModel> GetPaymentRequestData(int id)
        {
            try
            {
                var data = await db.STPaymentRequest.Where(x => x.Id == id).FirstOrDefaultAsync();
                var paymentRequestData = Mapper.Map<PaymentRequestViewModel>(data);
                paymentRequestData.PayeeName = db.Customer.Where(x => x.UserId == paymentRequestData.PayeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                var name = new List<string>();
                if (data.PayerName != null)
                {
                    name = new List<string>(data.PayerName.Split(" "));
                    paymentRequestData.PayerFirstName = data.PayerName;
                }
                if (name.Count > 1)
                {
                    paymentRequestData.PayerFirstName = string.Join(" ", name.Take(name.Count - 1));
                    paymentRequestData.PayerLastName = name.LastOrDefault();
                }

                paymentRequestData.STPaymentRequestDetails = new STPaymentRequestDetailsViewModel();
                var paymentRequestDetailData = await db.STPaymentRequestDetails.Where(x => x.STPaymentRequestId == id.ToString()).FirstOrDefaultAsync();
                paymentRequestData.STPaymentRequestDetails = Mapper.Map<STPaymentRequestDetailsViewModel>(paymentRequestDetailData);
                return paymentRequestData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PaymentRequestDetails> GetPaymentRequestDatas(int id)
        {
            try
            {
                var data = await db.STPaymentRequest.Where(x => x.Id == id).FirstOrDefaultAsync();
                var paymentRequestData = Mapper.Map<PaymentRequestViewModel>(data);
                paymentRequestData.PayeeName = db.Customer.Where(x => x.UserId == paymentRequestData.PayeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                var name = new List<string>();
                if (data.PayerName != null)
                {
                    name = new List<string>(data.PayerName.Split(" "));
                    paymentRequestData.PayerFirstName = data.PayerName;
                }
                if (name.Count > 1)
                {
                    paymentRequestData.PayerFirstName = string.Join(" ", name.Take(name.Count - 1));
                    paymentRequestData.PayerLastName = name.LastOrDefault();
                }

                paymentRequestData.PaymentRequestStatus = EnumModel<PaymentRequestStatus>.GetDescription(paymentRequestData.Status);

                paymentRequestData.STPaymentRequestDetails = new STPaymentRequestDetailsViewModel();
                var paymentRequestDetailData = await db.STPaymentRequestDetails.Where(x => x.STPaymentRequestId == id.ToString()).FirstOrDefaultAsync();
                paymentRequestData.STPaymentRequestDetails = Mapper.Map<STPaymentRequestDetailsViewModel>(paymentRequestDetailData);

                var paymentRequestDetails = new PaymentRequestDetails();
                paymentRequestDetails.PaymentRequestViewModel = new PaymentRequestViewModel();
                paymentRequestDetails.PaymentRequestViewModel = paymentRequestData;

                var transactionEvents = await GetTransactionEventsByReferenceNo(id);
                paymentRequestDetails.TransactionEvents = new List<TransactionEvents>();
                paymentRequestDetails.TransactionEvents = transactionEvents;

                return paymentRequestDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PaymentRequestDetails> GetRemittanceDatas(int id)
        {
            try
            {
                var data = await db.Transaction.Where(x => x.TransactionId == id).FirstOrDefaultAsync();
                var paymentRequestData = new PaymentRequestViewModel();
                paymentRequestData.RequestAmount = data.SendAmount;
                paymentRequestData.CreatedDate = Utils.GetDefaultDateFormatToDetail(data.CreatedDate);
                paymentRequestData.PaymentRequestStatus = Enum.GetName(typeof(SimpleTransferTransactionStatus), data.Status);
                paymentRequestData.PayeeName = db.Customer.Where(x => x.UserId == data.BeneficiaryId.ToString()).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();

                var user = await customerService.GetCustomerById(data.UserId.ToString());
                paymentRequestData.PayerFirstName = user.FirstName;
                paymentRequestData.PayerLastName = user.LastName;
                paymentRequestData.PayerMobileNumber = user.MobileNumber;

                paymentRequestData.STPaymentRequestDetails = new STPaymentRequestDetailsViewModel();
                paymentRequestData.STPaymentRequestDetails.DueDate = data.CreatedDate.AddDays(30);

                var paymentRequestDetails = new PaymentRequestDetails();
                paymentRequestDetails.PaymentRequestViewModel = new PaymentRequestViewModel();
                paymentRequestDetails.PaymentRequestViewModel = paymentRequestData;

                var transactionEvents = await GetTransactionTimeStampByReferenceNo(data.TransactionRefNo);
                paymentRequestDetails.TransactionEvents = new List<TransactionEvents>();
                paymentRequestDetails.TransactionEvents = transactionEvents;

                return paymentRequestDetails;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<TransactionEvents>> GetTransactionTimeStampByReferenceNo(string referenceNo)
        {
            var failureList = new List<string> { "2", "5", "8", "10", "11", "13", "15", "16", "17", "18" };
            var datas = await db.TransactionHistory.Where(x => x.TransactionRefNo == referenceNo).Select(x => new TransactionEvents
            {
                TransactionDate = x.TransactionDate,
                Time = x.CreatedDate.ToString("dd/MM/yyyy hh:mm"),
                Status = x.Status,
                Description = EnumModel<SimpleTransferTransactionManagementStatus>.GetDescription(x.Status),
                ClassName = (failureList.Contains(x.Status.ToString())) ? "failure" : "success"
            }).GroupBy(x => x.Status).Select(g => g.First()).OrderBy(x => x.TransactionDate).ToListAsync();

            return datas;
        }

        public async Task<bool> SetReminder(int id, string reminder)
        {
            try
            {
                var data = db.STPaymentRequestDetails.Where(x => x.STPaymentRequestId == id.ToString()).FirstOrDefault();
                if (data.Reminder == null || data.Reminder == "")
                {
                    data.Reminder = reminder;
                }
                else
                {
                    var reminderList = new List<string>(data.Reminder.Split(","));
                    var index = reminderList.IndexOf(reminder);
                    if (index < 0)
                    {
                        reminderList.Add(reminder);
                    }
                    else
                    {
                        reminderList.Remove(reminder);
                    }
                    data.Reminder = string.Join(',', reminderList);
                }

                db.STPaymentRequestDetails.Attach(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<TransactionEvents>> GetTransactionEventsByReferenceNo(int id)
        {
            var datas = await db.STPaymentRequest.Where(x => x.Id == id).Select(x => new TransactionEvents
            {
                TransactionDate = x.CreatedDate,
                Time = x.CreatedDate.ToString("dd/MM/yyyy hh:mm"),
                Status = x.Status,
                Description = EnumModel<SimpleTransferTransactionManagementStatus>.GetDescription(x.Status),
                ClassName = "success"
            }).GroupBy(x => x.Status).Select(g => g.First()).OrderBy(x => x.TransactionDate).ToListAsync();

            return datas;
        }

        public async Task<List<PaymentDetails>> GetPaymentDetailsByUserId()
        {
            try
            {
                string payeeId = Convert.ToString(commonService.getLoggedInUserId());
                var user = commonService.GetUserById(payeeId);

                var data = await db.STPaymentRequest.Where(x => x.PayeeId == payeeId || x.PayerEmail == user.Result.Email).OrderByDescending(x => x.CreatedDate).Select(x => new PaymentDetails
                {
                    Id = x.Id,
                    Amount = x.RequestAmount,
                    Date = Utils.GetDefaultDateFormatToDetail(x.CreatedDate),
                    Currency = "GBP",
                    Status = EnumModel<PaymentRequestStatus>.GetDescription(x.Status),
                    ClassName = EnumModel<PaymentStatusClassName>.GetDescription(x.Status),
                    Description = db.STPaymentRequestDetails.Where(y => y.STPaymentRequestId == x.Id.ToString()).Select(y => y.Description).FirstOrDefault(),
                    Type = x.PayeeId == payeeId ? 1 : 2
                }).Take(7).ToListAsync();


                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<PaymentDetails>> GetPaymentRequestDetailsByUserId()
        {
            try
            {
                string payeeId = Convert.ToString(commonService.getLoggedInUserId());
                var user = commonService.GetUserById(payeeId).Result;

                var data = await db.STPaymentRequest.Where(x => x.PayeeId == payeeId || x.PayerEmail == user.Email).OrderByDescending(x => x.CreatedDate).Select(x => new PaymentDetails
                {
                    Id = x.Id,
                    Amount = x.RequestAmount,
                    Date = Utils.GetDefaultDateFormatToDetail(x.CreatedDate),
                    Currency = "GBP",
                    Status = EnumModel<PaymentRequestStatus>.GetDescription(x.Status),
                    ClassName = EnumModel<PaymentStatusClassName>.GetDescription(x.Status),
                    Description = db.STPaymentRequestDetails.Where(y => y.STPaymentRequestId == x.Id.ToString()).Select(y => y.Description).FirstOrDefault(),
                    Type = x.PayeeId == payeeId ? 1 : 2
                }).Take(7).ToListAsync();


                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<PaymentDetails>> GetRemittanceDetailsByUserId()
        {
            try
            {
                string payeeId = Convert.ToString(commonService.getLoggedInUserId());
                var user = commonService.GetUserById(payeeId).Result;

                var data = await db.Transaction.Where(x => x.UserId.ToString() == user.Id && x.TrasactionType==1).OrderByDescending(x => x.CreatedDate).Select(x => new PaymentDetails
                {
                    Id = x.TransactionId,
                    Amount = x.SendAmount,
                    Date = Utils.GetDefaultDateFormatToDetail(x.CreatedDate),
                    Currency = db.CountryList.Where(y=>y.Id==x.SendCountryId).FirstOrDefault().CurrencyCode,
                    Status = EnumModel<SimpleTransferTransactionManagementStatus>.GetDescription(x.Status),
                    ClassName = EnumModel<PaymentStatusClassName>.GetDescription(x.Status),
                    Description = x.Remark,
                    Type = 1
                }).Take(7).ToListAsync();


                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
