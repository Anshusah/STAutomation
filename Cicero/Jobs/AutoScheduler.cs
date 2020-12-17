using AutoMapper;
using Cicero.Areas.Admin.Controllers.Api;
using Cicero.Data;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Helpers;
using Cicero.Service.Models;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Cicero.Service.SimpleTransferEnums;

namespace Cicero.Service.Configuration
{
    public class AutoScheduler
    {
        private List<Timer> timers = new List<Timer>();
        private readonly IUserService userService;
        private readonly IMapperService mapperService;
        private readonly IExchangeRateServices exchangeRateServices;
        private readonly IBankBranchServcie bankBranchServcie;
        private readonly ICityService cityService;
        private readonly ITransactionMgmtService transactionMgmtService;
        private readonly ICountryService countryService;
        private readonly ICustomerService customerService;
        private readonly SimpleTransferApplicationDbContext db;
        private readonly ISourceOfFundSetupService sourceOfFundSetupService;
        private readonly IPaymentPurposeSetupService paymentPurposeSetupService;
        private readonly IMapper mapper;
        private readonly IWorkflowService workflowService;
        private readonly Utils utils;
        private readonly IFormService formService;
        private readonly ICaseService caseService;
        private readonly ILexisNexisService lexisNexisService;

        public AutoScheduler(IUserService userService,IMapperService mapperService, SimpleTransferApplicationDbContext _db, IExchangeRateServices exchangeRateServices, IBankBranchServcie bankBranchServcie, 
            ICityService cityService, ITransactionMgmtService transactionMgmtService, ICountryService countryService, ICustomerService customerService, 
            ISourceOfFundSetupService sourceOfFundSetupService,IPaymentPurposeSetupService paymentPurposeSetupService, IMapper mapper, IWorkflowService workflowService, Utils utils, IFormService formService, ICaseService caseService, ILexisNexisService lexisNexisService)
        {
            this.userService = userService;
            this.mapperService = mapperService;
            this.db = _db;
            this.exchangeRateServices = exchangeRateServices;
            this.bankBranchServcie = bankBranchServcie;
            this.cityService = cityService;
            this.transactionMgmtService = transactionMgmtService;
            this.countryService = countryService;
            this.customerService = customerService;
            this.sourceOfFundSetupService = sourceOfFundSetupService;
            this.paymentPurposeSetupService = paymentPurposeSetupService;
            this.mapper = mapper;
            this.workflowService = workflowService;
            this.utils = utils;
            this.formService = formService;
            this.caseService = caseService;
            this.lexisNexisService = lexisNexisService;
        }
        public void GetAutoSchedulerRunTime()
        {
            var transfastSchedule = this.db.AutoSchedulerSetting.Where(x => x.Name == SchedulerList.Transfast);
            //   var necMoneySchedule = this.db.AutoSchedulerSetting.Where(x => x.Name == SchedulerList.NecMoney);

        }

        [HttpGet]
        [Route("transaction/status")]
        public async void TransactionStatus(IConfiguration configuration)
        {
            try
            {
                var transfast = new TransfastApiController(userService, configuration, mapperService, bankBranchServcie, db, exchangeRateServices, cityService, transactionMgmtService, countryService, customerService,sourceOfFundSetupService,paymentPurposeSetupService, mapper, workflowService, utils, formService, caseService, lexisNexisService);
                var result = await transfast.TransactionStatus() as OkObjectResult;
            }
            catch (Exception ex)
            {
            }
        }

        [Route("transaction/checkbttrxnew")]
        public async void CheckBTTrxNew(IConfiguration configuration)
        {
            try
            {
                var transfast = new TransfastApiController(userService, configuration, mapperService, bankBranchServcie, db, exchangeRateServices, cityService, transactionMgmtService, countryService, customerService, sourceOfFundSetupService, paymentPurposeSetupService, mapper, workflowService, utils, formService, caseService,lexisNexisService);
                var result = await transfast.CheckBankTransferNewTransaction() as OkObjectResult;
            }
            catch (Exception ex)
            {
            }
        }

        public void GetTodayRate(IConfiguration configuration)
        {
            try
            {
                //var necMoney = new NecMoneyController(configuration,db,bankBranchServcie,exchangeRateServices);
                //var necMoneyResult = necMoney.GetTodayRate() as OkObjectResult;
                //var necMoneyRateList = necMoneyResult.Value as List<RateViewModel>;
                ////var necExchangeRates = new List<decimal>();
                ////foreach (var item in necMoneyRateList)
                ////{
                ////    necExchangeRates.Add(Convert.ToDecimal(item.Rate));
                ////}

                var transfast = new TransfastApiController(userService, configuration, mapperService, bankBranchServcie, db, exchangeRateServices, cityService, transactionMgmtService, countryService, customerService, sourceOfFundSetupService,paymentPurposeSetupService, mapper, workflowService, utils, formService, caseService,lexisNexisService);
                var transfastResult = transfast.GetTodayRates("GBP", "BD").Result as OkObjectResult;
                var transfastRateList = transfastResult.Value as List<RateViewModel>;
                //var transfastExchangeRates = new List<decimal>();
                //foreach (var item in transfastRateList)
                //{
                //    transfastExchangeRates.Add(Convert.ToDecimal(item.StartRate));
                //}

                //#region necMoneyExchangeRate
                //var necExchangeRate = ExchangeRates(necMoneyRateList, SchedulerList.NecMoney.ToString());

                //var necExchangeRateHistory = ExchangeRatesHistory(necMoneyRateList, SchedulerList.NecMoney.ToString());

                //var result = exchangeRateServices.CreateOrUpdate(necExchangeRate).Result;
                //result = exchangeRateServices.CreateOrUpdate(necExchangeRateHistory).Result;
                //#endregion

                #region transfastExchangeRate
                var transfastExchangeRate = ExchangeRates(transfastRateList, SchedulerList.Transfast.ToString());

                var transfastExchangeRateHistory = ExchangeRatesHistory(transfastRateList, SchedulerList.Transfast.ToString());

                var result = exchangeRateServices.CreateOrUpdate(transfastExchangeRate).Result;
                result = exchangeRateServices.CreateOrUpdate(transfastExchangeRateHistory).Result;
                #endregion
            }
            catch (Exception ex)
            {

            }
        }

        private List<ExchangeRates> ExchangeRates(List<RateViewModel> data, string source)
        {
            var exchangeRate = new List<ExchangeRates>();
            foreach (var item in data)
            {
                exchangeRate.Add(new ExchangeRates
                {
                    DateTime = DateTime.Now,
                    Bank = item.Bank,
                    ModeOfPayment = item.ModeOfPayment,
                    ExchangeRate = item.Rate,
                    FromCountryCode = item.SourceCountry,
                    ToCountryCode = item.DestinationCountry,
                    FromCurrencyCode = "BGP",
                    ToCurrencyCode = item.Currency,
                    Source = source,
                    UpdatedBy = "System",
                    UpdatedOn = DateTime.Now
                });
            }
            return exchangeRate;
        }

        private List<ExchangeRatesHistory> ExchangeRatesHistory(List<RateViewModel> data, string source)
        {
            var exchangeRateHistory = new List<ExchangeRatesHistory>();
            foreach (var item in data)
            {
                exchangeRateHistory.Add(new ExchangeRatesHistory
                {
                    DateTime = DateTime.Now,
                    Bank = item.Bank,
                    ModeOfPayment = item.ModeOfPayment,
                    ExchangeRate = item.Rate,
                    FromCountryCode = item.SourceCountry,
                    ToCountryCode = item.DestinationCountry,
                    FromCurrencyCode = "BGP",
                    ToCurrencyCode = item.Currency,
                    Source = source,
                    UpdatedBy = "System",
                    UpdatedOn = DateTime.Now
                });
            }
            return exchangeRateHistory;
        }

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }
            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }
            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            timers.Add(timer);
        }

        public void IntervalInDays(int hour, int min, double interval, Action task)
        {
            interval = interval * 24;
            ScheduleTask(hour, min, interval, task);
        }


        public void IntervalInSeconds(int hour, int sec, double interval, Action task)
        {
            interval = interval / 3600;
            ScheduleTask(hour, sec, interval, task);
        }
    }
}
