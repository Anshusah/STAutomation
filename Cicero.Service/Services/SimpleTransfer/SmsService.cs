using AutoMapper;
using Cicero.Data;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Cicero.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Cicero.Data.Entities.SimpleTransfer;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Cicero.Service.Models.SimpleTransfer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Cicero.Service.Services.SimpleTransfer
{
    public interface ISmsService
    {
        bool SendOtp(string number, string countryCode); 
        void CreateSmsLog(SmsLog smsLog);
        void CreateCustomerRegistrationSms(SmsCodeCustomerRegistraion smsCodeCustomerRegistraion);
        void CreateCustomerRegistrationSmsLog(SmsCodeCustomerRegistraionLog smsCodeCustomerRegistraionLog);
        bool ValidateCustomerRegistrationOtp(string otp, string mobileNumber, string countryCode);
        bool IsDuplicateMobileNumber(string mobileNumber, int tenantId);
        bool IsSMSVerified(string mobileNumber, string countryCode);

    }

    public class SmsService : ISmsService
    {
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ILogger<ISmsService> Log;
        private readonly IMapper Mapper;
        private readonly ICommonService commonService;
        private readonly IActivityLogService activityLogService;
        private readonly Utils utils;
        private readonly TwilioAccountDetails _twilioAccountDetails;
        private readonly IConfiguration configuration;
        private readonly ICountryService countryService;
        private readonly bool isDev;
        private readonly bool isUat;

        public SmsService(SimpleTransferApplicationDbContext _db, ILogger<ISmsService> _Log, IMapper _mapper,
            ICommonService _commonService, IActivityLogService _activityLogService, Utils _utils, 
            IOptions<TwilioAccountDetails> twilioAccountDetails, ICountryService countryService,
            IConfiguration configuration)
        {
            db = _db;
            Log = _Log;
            Mapper = _mapper;
            commonService = _commonService;
            activityLogService = _activityLogService;
            utils = _utils;
            _twilioAccountDetails = twilioAccountDetails.Value ?? throw new ArgumentException(nameof(twilioAccountDetails));
            this.configuration = configuration;
            this.countryService = countryService;
            isDev= Convert.ToBoolean(configuration.GetSection("isDev").Value);
            isUat= Convert.ToBoolean(configuration.GetSection("isUat").Value);
        }
        public bool SendOtp(string number,string countryCode) 
        {
            string prefix = countryService.GetPhoneCodeByCountryCode(countryCode);
            //string prefix = "+44";
            var regex = @"^(07[\d]{9}|7[\d]{9})$";
            Match match = Regex.Match(number, regex, RegexOptions.IgnoreCase);
            if (countryCode=="GB"&&!match.Success)
            {
                throw new Exception("Invalid Phone Number");
            }
            string fromNumber = configuration.GetSection("FromPhoneNumber").Value;
            number=isDev? configuration.GetSection("TestPhoneNumber").Value: string.Concat(prefix, number.TrimStart('0'));
            number=isUat&&db.UatSetting.FirstOrDefault().Status==true?db.UatSetting.FirstOrDefault().PhoneNumber : string.Concat(prefix, number.TrimStart('0'));
            // Find your Account Sid and Auth Token at twilio.com/console
            string accountSid = configuration.GetSection("TwilioAccountDetails:AccountSid").Value;
            string authToken = configuration.GetSection("TwilioAccountDetails:AuthToken").Value;
            TwilioClient.Init(accountSid, authToken);
            string otp = GetRandomOtpNumber().ToString();
            try
            {
                var message = MessageResource.Create(
                to: new PhoneNumber(number),
                from: new PhoneNumber(fromNumber),
                body: "Dear Customer, your OTP(One Time Password) for verification is " + otp + ". Use this code to verify your Mobile Number with Simple Transfer."
                //mediaUrl: mediaUrl);
                );
                var smsLog = new SmsLog()
                {
                    Message = message.Body,
                    StatusMessage = message.Status.ToString(),
                    MobileNumber = number.ToString(),
                    ResponseMessage = message.Status.ToString(),
                    TenantId = commonService.GetTenantIdByIdentifier(""),
                    ErrorCode=message.ErrorCode.ToString()
                };
                CreateSmsLog(smsLog);                
                var smsCodeCustomerReg = new SmsCodeCustomerRegistraion()
                {
                    SmsSuccess = true,
                    MobileNumber = number.ToString(),
                    TenantId = commonService.GetTenantIdByIdentifier(""),
                    Status = true,
                    CustomerSuccess = false,
                    ExpiryDateTime = DateTime.Now.AddMinutes(5),
                    ExpiryMinute = 5,
                    SmsCode=Convert.ToInt32(otp)
                };
                CreateCustomerRegistrationSms(smsCodeCustomerReg);
                return true;
            }
            catch(Exception ex)
            {
                var smsLog = new SmsLog()
                {
                    Message = "",
                    StatusMessage = ex.Message,
                    MobileNumber = number.ToString(),
                    ResponseMessage = ex.Message,
                    TenantId = commonService.GetTenantIdByIdentifier("")
                };
                CreateSmsLog(smsLog);
                return false;
            }
        }
        private int GetRandomOtpNumber()
        {
            Random random = new Random();
            return random.Next(000000, 999999);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public void CreateSmsLog(SmsLog smsLog)
        {
            db.SmsLog.Add(smsLog);
            db.SaveChanges();
        }
        public void CreateCustomerRegistrationSms(SmsCodeCustomerRegistraion smsCodeCustomerRegistraion)
        {
            var mobileExists = db.SmsCodeCustomerRegistraion.Where(x => x.MobileNumber == smsCodeCustomerRegistraion.MobileNumber).FirstOrDefault();
            if(mobileExists!=null)
            {
                mobileExists.RetryCount = mobileExists.RetryCount+1;
                mobileExists.ExpiryDateTime = smsCodeCustomerRegistraion.ExpiryDateTime;
                mobileExists.ExpiryMinute = smsCodeCustomerRegistraion.ExpiryMinute;
                mobileExists.SmsSuccess = smsCodeCustomerRegistraion.SmsSuccess;
                mobileExists.Status = true;
                mobileExists.SmsCode = smsCodeCustomerRegistraion.SmsCode;
                db.SmsCodeCustomerRegistraion.Attach(mobileExists);
                SmsCodeCustomerRegistraionLog codeCustomerRegistraionLog = new SmsCodeCustomerRegistraionLog()
                {
                    MobileNumber = mobileExists.MobileNumber,
                    SmsCode = smsCodeCustomerRegistraion.SmsCode,
                    SmsSuccess = true,
                    TenantId = smsCodeCustomerRegistraion.TenantId
                };
                CreateCustomerRegistrationSmsLog(codeCustomerRegistraionLog);
            }
            else
            {
                db.SmsCodeCustomerRegistraion.Add(smsCodeCustomerRegistraion);
                SmsCodeCustomerRegistraionLog codeCustomerRegistraionLog = new SmsCodeCustomerRegistraionLog()
                {
                    MobileNumber = smsCodeCustomerRegistraion.MobileNumber,
                    SmsCode = smsCodeCustomerRegistraion.SmsCode,
                    SmsSuccess = true,
                    TenantId = smsCodeCustomerRegistraion.TenantId
                };
                CreateCustomerRegistrationSmsLog(codeCustomerRegistraionLog);
            }
            db.SaveChanges();
        }
        public void CreateCustomerRegistrationSmsLog(SmsCodeCustomerRegistraionLog smsCodeCustomerRegistraionLog)
        {
            db.SmsCodeCustomerRegistraionLog.Add(smsCodeCustomerRegistraionLog);
            db.SaveChanges();
        }
        public bool ValidateCustomerRegistrationOtp(string otp,string mobileNumber, string countryCode)
        {
            string prefix = countryService.GetPhoneCodeByCountryCode(countryCode);
            mobileNumber = isDev ? configuration.GetSection("TestPhoneNumber").Value : string.Concat(prefix, mobileNumber.TrimStart('0'));
            mobileNumber = isUat && db.UatSetting.FirstOrDefault().Status == true ? db.UatSetting.FirstOrDefault().PhoneNumber : string.Concat(prefix, mobileNumber.TrimStart('0'));
            var mobileExists = db.SmsCodeCustomerRegistraion.Where(x => x.MobileNumber == mobileNumber && x.SmsCode.ToString()==otp && x.ExpiryDateTime>=DateTime.Now).FirstOrDefault();
            if (mobileExists != null)
            {
                mobileExists.CustomerSuccess = true;
                db.Attach(mobileExists);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool IsDuplicateMobileNumber(string mobileNumber,int tenantId)
        {

             return (!db.SmsCodeCustomerRegistraion.Any(d => d.MobileNumber == mobileNumber && d.TenantId==tenantId));
      
        }

        public bool IsSMSVerified(string mobileNumber, string countryCode)
        {
            string prefix = countryService.GetPhoneCodeByCountryCode(countryCode);
            mobileNumber = isDev ? configuration.GetSection("TestPhoneNumber").Value : string.Concat(prefix, mobileNumber.TrimStart('0'));
            mobileNumber = isUat && db.UatSetting.FirstOrDefault().Status == true ? db.UatSetting.FirstOrDefault().PhoneNumber : string.Concat(prefix, mobileNumber.TrimStart('0'));
            int tenantId = commonService.GetTenantIdByIdentifier(utils.GetTenantFromSession());            
            return (db.SmsCodeCustomerRegistraion.Any(d => d.MobileNumber == mobileNumber && d.TenantId == tenantId 
            && d.CustomerSuccess && d.SmsSuccess&&d.Status));
        }
    }
}
