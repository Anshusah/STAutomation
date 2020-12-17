using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NecMoneyServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleTransferAPI.Model;
using SimpleTransferAPI.Service;
using Utilities;
using static Cicero.Data.Enumerations;
using static Cicero.Service.SimpleTransferEnums;

namespace SimpleTransferAPI.Controllers
{
    [Route("api/necmoney")]
    [ApiController]
    public class NecMoneyController : Controller
    {
        private readonly IConfiguration _config;
        private readonly string _userId;
        private readonly string _password;
        private readonly string _companyId;
        private ImportTTSoapClient _importTTSoapClient = new ImportTTSoapClient(ImportTTSoapClient.EndpointConfiguration.ImportTTSoap);
        private readonly IBankBranchServcie _bankBranchService;
        private readonly IExchangeRateServices exchangeRateServices;
        private readonly SimpleTransferApplicationDbContext _db;
        public NecMoneyController(IConfiguration config, SimpleTransferApplicationDbContext db, IBankBranchServcie bankBranchServcie, IExchangeRateServices exchangeRateServices)
        {
            _config = config;
            var values = _config.GetSection("ExternalHeaders:NecMoney").GetChildren().ToDictionary(x => x.Key, x => x.Value).ToList();
            var systemId = values.Where(x => x.Key == "SystemId").Select(x => x.Value).FirstOrDefault();
            _userId = values.Where(x => x.Key == "UserId").Select(x => x.Value).FirstOrDefault();
            _password = values.Where(x => x.Key == "Password").Select(x => x.Value).FirstOrDefault();
            _companyId = values.Where(x => x.Key == "CompanyId").Select(x => x.Value).FirstOrDefault();
            _db = db;
            _bankBranchService = bankBranchServcie;
            this.exchangeRateServices = exchangeRateServices;
        }

        [HttpGet]
        [Route("gettodayrate")]
        public IActionResult GetTodayRate()
        {
            try
            {
                var test = _importTTSoapClient.GetTodayRateAsync(_userId, _password, _companyId).Result;
                var data = test.Body.GetTodayRateResult;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data.Result);
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                var jObject = JsonConvert.DeserializeObject<JObject>(jsonText);
                var jsondata = jObject["DealingRates"];
                var necMoneyRateList = jsondata["DealingRate"].ToObject<List<NecMoneyDealingRate>>();

                var datas = (from nm in necMoneyRateList
                             select new RateViewModel
                             {
                                 SourceCountry = "UK",
                                 DestinationCountry = nm.Country,
                                 Currency = nm.Currency,
                                 Rate = Convert.ToDecimal(nm.Rate),
                                 Bank = nm.Bank
                             }).ToList();

                foreach (var item in datas)
                {
                    var index = item.Bank.IndexOf("(");
                    var length = item.Bank.Length;
                    var type = item.Bank.Remove(0, index);
                    item.Bank = item.Bank.Remove(index, length - index).Trim();
                    item.ModeOfPayment = (type.Contains("ACCOUNT")) ? 1 : 2;
                }
                //var necExchangeRates = new List<decimal>();
                //foreach (var item in necMoneyRateList)
                //{
                //    necExchangeRates.Add(Convert.ToDecimal(item.Rate));
                //}
                //var exchangeRate = necExchangeRates.OrderBy(x => x).FirstOrDefault();
                //var jo = new JObject();
                //jo.Add("Rate", exchangeRate);
                if (datas.Count > 0)
                    return Ok(datas);
                else
                    return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }
        [HttpGet]
        [Route("settodayrate")]
        public IActionResult SetTodayRate()
        {
            try
            {
                var necMoneyResult = GetTodayRate() as OkObjectResult;
                var necMoneyRateList = necMoneyResult.Value as List<RateViewModel>;

                var necExchangeRate = exchangeRateServices.ExchangeRates(necMoneyRateList, SchedulerList.NecMoney.ToString());
                var necExchangeRateHistory = exchangeRateServices.ExchangeRatesHistory(necMoneyRateList, SchedulerList.NecMoney.ToString());

                var result = exchangeRateServices.CreateOrUpdate(necExchangeRate).Result;
                result = exchangeRateServices.CreateOrUpdate(necExchangeRateHistory).Result;
                if (result)
                    return Ok(result);
                else
                    return StatusCode((int)System.Net.HttpStatusCode.NoContent, "No Data Found");
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }
        [HttpGet]
        [Route("getbank")]
        public IActionResult GetBanks()
        {
            try
            {
                var countryList = _db.CountryList.Where(x => x.IsActive == true).ToList();
                foreach (var item in countryList)
                {
                    var getBank = _importTTSoapClient.GetBanksAsync(_userId, _password, _companyId, item.Code).Result;
                    var data = getBank.Body.GetBanksResult;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(data.Result);
                    string jsonText = JsonConvert.SerializeXmlNode(doc);
                    var jObject = JsonConvert.DeserializeObject<JObject>(jsonText);
                    var jsondata = jObject["Banks"];
                    if (jsondata.Any())
                    {
                        var necBankList = jsondata["Bank"].ToObject<List<NecBankViewModel>>();
                        var necBanks = (from nm in necBankList
                                        select new NecBankViewModel
                                        {
                                            Code = nm.Code.ToUpper(),
                                            Name = nm.Name.ToUpper(),
                                            CountryIsoCode = nm.CountryIsoCode.ToUpper()
                                        }).ToList();
                        _bankBranchService.CreateOrUpdateBank(item.Code, RateSupplierEnum.NecMoney, null, necBanks);
                    }
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }
        [HttpGet]
        [Route("getbankbranch")]
        public async Task<IActionResult> GetBankBranchesAsync()
        {
            try
            {
                var banks = _db.SupplierBank.Where(x => x.Status == true && x.SupplierId==(int)RateSupplierEnum.NecMoney).OrderBy(x=>x.BankCode).ToList();
                foreach (var item in banks)
                {
                    var getBank = _importTTSoapClient.GetBankBranchesAsync(_userId, _password, _companyId,item.CountryCode.ToUpper(),item.BankCode.ToUpper()).Result;
                    var data = getBank.Body.GetBankBranchesResult;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(data.Result);
                    string jsonText = JsonConvert.SerializeXmlNode(doc);
                    var jObject = JsonConvert.DeserializeObject<JObject>(jsonText);
                    var jsondata = jObject["BankBranches"];
                    if (jsondata.Any())
                    {
                        var necBankBranchList = new List<NecBankBranchViewModel>();
                        try
                        {
                            var branch = jsondata["BankBranch"].ToObject<NecBankBranchViewModel>();
                            necBankBranchList.Add(branch);
                            _bankBranchService.CreateOrUpdateBankBranch(item.CountryCode.ToUpper(), RateSupplierEnum.NecMoney, null, necBankBranchList);

                        }
                        catch
                        {
                            var necBankBranches = jsondata["BankBranch"].ToObject<List<NecBankBranchViewModel>>();
                            if (necBankBranches != null)
                            {
                                necBankBranchList = (from nm in necBankBranches
                                                     select new NecBankBranchViewModel
                                                       {
                                                           Code = (nm.Code == null) ? "" : nm.Code.ToUpper(),
                                                           Name = (nm.Name == null) ? "" : nm.Name.ToUpper(),
                                                           CountryIsoCode = (nm.CountryIsoCode == null) ? "" : nm.CountryIsoCode.ToUpper(),
                                                           BankCode = (nm.BankCode == null) ? "" : nm.BankCode.ToUpper(),
                                                           City = (nm.City == null) ? "" : nm.City.ToUpper()
                                                       }).ToList();
                                _bankBranchService.CreateOrUpdateBankBranch(item.CountryCode.ToUpper(), RateSupplierEnum.NecMoney, null, necBankBranchList);
                            }
                        }
                    }
                }
                return Ok();

            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription);
            }
            finally
            {
                //log request and response
            }
        }


    }
    
}
