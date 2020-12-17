using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;
using AutoMapper;
using Cicero.Configuration;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Library.Toastr;
using Cicero.Service.Models;
using Cicero.Service.Models.Core;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Service.Models.SimpleTransfer.SecureTrading;
using Cicero.Service.Services;
using Cicero.Service.Services.SimpleTransfer;
using Core.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Cicero.Service.Enums;
using static Cicero.Service.SimpleTransferEnum;

namespace Cicero.Areas.Admin.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SecureTradingController : BaseController
    {
        private readonly IUserService _userService;
        private readonly Utils _utils;
        private readonly ILogger<SecureTradingController> _log;
        private readonly IStatus _status;
        private readonly ITenantService _tenantService;
        private readonly AppSetting _appSetting;
        private readonly ICommonService _ICommonService;
        private readonly ISecureTradingService _SecureTradingService;
        private readonly IQueueService _queueService;
        private readonly ICaseService _caseService;
        private readonly ISynchronizeService _synchronizeService;
        private readonly IAutomationService _automationService;
        private readonly IWorkflowService _workflowService;
        private readonly IMediaService mediaService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IToastNotification _toastNotification;
        private readonly ISecureTradingService secureTradingService;
        private readonly IConfiguration configuration; 
        private readonly List<KeyValuePair<string, string>> externalHeaders;
        private readonly IMapper mapper;
        private readonly ICustomerService customerService;
        private readonly IRateSupplierFeeConfigService rateSupplierFeeConfigService;
        private readonly string username;
        private readonly string secret;
        private readonly string siteReference;
        private readonly string basicAuth;
        private readonly string version;
        private readonly string url;

        public SecureTradingController(ISecureTradingService ISecureTradingService,
            ISynchronizeService synchronizeService,
            IAutomationService autoService,
            ICaseService caseService,
            IQueueService queueService,
            ICommonService ics,
            IUserService userService,
            AppSetting appSetting,
            Utils utils,
            ILogger<SecureTradingController> log,
            IStatus status,
            ITenantService tenantService,
            IWorkflowService workflowService,
            IMediaService mediaService,
            IHostingEnvironment hostingEnvironment,
            IToastNotification toastNotification,
            ISecureTradingService secureTradingService,
                        IConfiguration configuration,IMapper mapper,
                        ICustomerService customerService,
                        IRateSupplierFeeConfigService rateSupplierFeeConfigService) : base(userService)
        {
            _userService = userService;
            _utils = utils;
            _log = log;
            _status = status;
            _tenantService = tenantService;
            _appSetting = appSetting;
            _ICommonService = ics;
            _SecureTradingService = ISecureTradingService;
            _queueService = queueService;
            _caseService = caseService;
            _synchronizeService = synchronizeService;
            _automationService = autoService;
            _workflowService = workflowService;
            this.mediaService = mediaService;
            this.hostingEnvironment = hostingEnvironment;
            _toastNotification = toastNotification;
            this.secureTradingService = secureTradingService;
            this.configuration = configuration;
            externalHeaders = this.configuration.GetSection("SecureTrading:basicauth").GetChildren().ToDictionary(x => x.Key, x => x.Value).ToList();
            this.mapper = mapper;
            this.customerService = customerService;
            this.rateSupplierFeeConfigService = rateSupplierFeeConfigService;
            username = configuration.GetSection("SecureTrading:webserviceusername").Value;
            secret = configuration.GetSection("SecureTrading:webservicepassword").Value;
            siteReference = configuration.GetSection("SecureTrading:sitereference").Value;
            basicAuth = configuration.GetSection("SecureTrading:basicauth").Value;
            version = configuration.GetSection("SecureTrading:version").Value;
            url = configuration.GetSection("SecureTrading:url").Value;
        }

        [Route("admin/securetrading/pushpayment")]
        [HttpGet]
        public async Task<IActionResult> PushPaymentAsync(string userId,string amount)
        {
            object response;
            try
            {                
                CustomerCardDetail customerCardDetail = await customerService.GetCustomerCardDetails(userId);
                var fee = rateSupplierFeeConfigService.GetRateSupplierFeeBySendAmountAsync(amount, "BD", 1);
                var request = new SecureTradingPaymentRequest()
                {
                    alias=username,
                    version= version,
                    request=new PaymentRequest()
                    {
                        currencyiso3a="GBP",
                        requesttypedescriptions="AUTH",
                        sitereference=siteReference,
                        baseamount=((Convert.ToInt32(amount)+Convert.ToInt32(fee.Result.FeeAmount))*100).ToString(),
                        orderreference="st01",
                        accounttypedescription="ECOM",
                        pan= "4111111111111111",
                        expirydate= "12/2022",
                        securitycode= "123"
                    }
                };
                var res = await WebApiService.InstanceForExternal.PostJsonContent<object>(url, new HttpClient(), request,externalHeaders);
                var mapped = mapper.Map<SecureTradingPaymentResonse>(res);
                var mappedResponse = JsonConvert.DeserializeObject<SecureTradingPaymentDetail>(mapped.response[0].ToJson());
                mappedResponse.requestreference = mapped.requestreference;
                mappedResponse.version = mapped.version;
                mappedResponse.secrand = mapped.secrand;
                secureTradingService.SavePaymentResponseDetail(mappedResponse);
                response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = mappedResponse };
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }

        [Route("admin/securetrading/refund")]
        [HttpGet]
        public async Task<IActionResult> RefundPaymentAsync(string parenttransactionreference)
        {
            object response;
            try
            {
                if (parenttransactionreference != null)
                {
                    var request = new SecureTradingRefundRequest()
                    {
                        alias = username,
                        version = version,
                        request = new RefundRequest()
                        {
                            requesttypedescriptions = SecureTradingRequestType.REFUND.ToDescription() ,
                            sitereference = siteReference,
                            parenttransactionreference = parenttransactionreference,
                        }
                    };
                    var res = await WebApiService.InstanceForExternal.PostJsonContent<object>(url, new HttpClient(), request, externalHeaders);
                    var mapped = mapper.Map<SecureTradingPaymentResonse>(res);
                    var mappedResponse = JsonConvert.DeserializeObject<SecureTradingPaymentDetail>(mapped.response[0].ToJson());
                    mappedResponse.requestreference = mapped.requestreference;
                    mappedResponse.version = mapped.version;
                    mappedResponse.secrand = mapped.secrand;
                    secureTradingService.SavePaymentResponseDetail(mappedResponse);
                    secureTradingService.UpdatePaymentDetailToRefund(parenttransactionreference);
                    response = new { Success = true, StatusCode = 200, Message = "Get Successfully", DataList = "", Data = res };
                }
                else
                {
                    response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
                }
            }
            catch (Exception ex)
            {
                response = new { Success = false, StatusCode = 500, Message = "Unsuccessfull", DataList = "", Data = "" };
            }
            return Ok(response);
        }
    }
}