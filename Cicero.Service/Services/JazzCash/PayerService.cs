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
using Cicero.Service.Models.JazzCash;
using Cicero.Data.Entities.JazzCash;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static Cicero.Service.Enums;
using Cicero.Service.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using static Cicero.Data.Enumerations;

namespace Cicero.Service.Services.JazzCash
{
    public interface IPayerService
    {
        Task<PayerViewModel> CreateOrUpdate(PayerViewModel model);
        Task<PaymentRequest> Create(PaymentRequest data, string callbackUrl);
        Task<bool> SendEmail(string email, string name, decimal amount, string callbackUrl);
        Task<JazzCashPaymentRequestViewModel> GetPaymentRequestData(int id);
        Task<JazzCashPaymentRequestViewModel> GetPaymentRequestData(string paymentReferenceNumber);
    }

    public class PayerService : IPayerService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<PayeeService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITemplateService templateService;
        private readonly IEmailSender emailSender;
        private readonly IRazorToStringRender razorViewToStringRenderer;

        public PayerService(SimpleTransferApplicationDbContext _db, ILogger<PayeeService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, UserManager<ApplicationUser> _UserManager, ITemplateService templateService, IEmailSender emailSender, IRazorToStringRender razorViewToStringRenderer)
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
        }

        public async Task<PaymentRequest> Create(PaymentRequest data, string callbackUrl)
        {
            try
            {
                // data.CreatedBy = model.CreatedBy;
                //  data.UpdatedBy = commonService.getLoggedInUserId();
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                data.PaymentReferenceNumber = RandomString(10);
                data.Status = (int)SimpleTransferTransactionStatus.New;

                db.PaymentRequest.Add(data);
                await db.SaveChangesAsync();

                callbackUrl = callbackUrl + data.Id;
                var result = await SendEmail(data.Email, data.PayeeName, data.RequestAmount, callbackUrl);
                if (result) return data;

                db.PaymentRequest.Remove(data);
                return null;
            }
            catch (Exception ex)
            {
                return null;
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

        public async Task<PayerViewModel> CreateOrUpdate(PayerViewModel model)
        {
            try
            {
                var tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());
                var payer = new Payer();
                payer = Mapper.Map<Payer>(model);
                payer.CreatedBy = model.CreatedBy;
                payer.UpdatedBy = commonService.getLoggedInUserId();
                payer.CreatedDate = Convert.ToDateTime(model.CreatedDate);
                payer.UpdatedDate = DateTime.Now;

                if (model.Id == 0)
                {
                    payer.CreatedBy = payer.UpdatedBy;
                    payer.CreatedDate = payer.UpdatedDate;
                    db.Payer.Add(payer);
                    await db.SaveChangesAsync();
                    return Mapper.Map<PayerViewModel>(payer);
                }
                else
                {
                    db.Payer.Attach(payer).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<JazzCashPaymentRequestViewModel> GetPaymentRequestData(int id)
        {
            try
            {
                var data = await db.PaymentRequest.Where(x => x.Id == id).FirstOrDefaultAsync();
                return Mapper.Map<JazzCashPaymentRequestViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<JazzCashPaymentRequestViewModel> GetPaymentRequestData(string paymentReferenceNumber)
        {
            try
            {
                var data = await db.PaymentRequest.Where(x => x.PaymentReferenceNumber == paymentReferenceNumber).FirstOrDefaultAsync();
                return Mapper.Map<JazzCashPaymentRequestViewModel>(data);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> SendEmail(string email, string name, decimal amount, string callbackUrl)
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
    }
}
