using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NecMoneyServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAFK;
using SimpleTransferAPI.Service;

namespace SimpleTransferAPI.Controllers
{
    [Route("api/safk")]
    [ApiController]
    public class SafkController : ControllerBase
    {
        private SAFK_WSDLSoapClient _safk_WSDLSoapClient = new SAFK_WSDLSoapClient(SAFK_WSDLSoapClient.EndpointConfiguration.SAFK_WSDLSoap);
        private readonly IConfiguration _config;
        private readonly IMapperService _mapperService;
        private readonly Login login;
        public SafkController(IConfiguration config, IMapperService mapperService)
        {
            login = new Login()
            {
                UserName = config.GetSection("ExternalHeaders:Safk:UserName").Value,
                Password = config.GetSection("ExternalHeaders:Safk:Password").Value,
                SecretKey = config.GetSection("ExternalHeaders:Safk:SecretKey").Value
            };
        }
        [HttpGet]

        [Route("getrate/{isoCurrencyCode}")]
        public IActionResult GetRate(string isoCurrencyCode)
        {
            GET_CURRENCY_RATE get_RatesResponse = _safk_WSDLSoapClient.Get_RatesAsync(login, isoCurrencyCode).Result.Body.Get_RatesResult;
            // _mapperService.MapToCiceroStandardResponse(get_RatesResponse);
            //var jo = new JObject();
            //jo.Add("Rate", get_RatesResponse.CustomerRate);
            return Ok(get_RatesResponse);
        }
        [HttpGet]
        [Route("getcommission")]
        public IActionResult GetCommissionValue(string isoCurrencyCode,double fcAmount)
        {
            GET_Commission gET_Commission = _safk_WSDLSoapClient.Get_commissionValueAsync(login, isoCurrencyCode,fcAmount).Result.Body.Get_commissionValueResult;
            return Ok(JsonConvert.SerializeObject(gET_Commission));
        }
        //[Route("sendmasterdata")]
        //public IActionResult SendMasterData(string isoCurrencyCode, double fcAmount)
        //{
        //    GET_Commission gET_Commission = _safk_WSDLSoapClient.SEND_MASTER_DATAAsync(login, isoCurrencyCode, fcAmount).Result.Body.Get_commissionValueResult;
        //    return Ok(JsonConvert.SerializeObject(gET_Commission));
        //}
    }
}