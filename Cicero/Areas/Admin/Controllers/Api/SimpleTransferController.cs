using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Configuration;
using Cicero.Data;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace Cicero.Areas.Admin.Controllers.Api
{
    [ApiKeyAuth]
    [Route("api/simpletransfer")]
    [ApiController]
    public class SimpleTransferController : ControllerBase
    {
        private readonly IMapperService mapperService;
        private readonly IExchangeRateServices exchangeRateServices;
        private readonly ISimpleTransferService secureTransferService;

        public SimpleTransferController(IMapperService mapperService, IExchangeRateServices exchangeRateServices, ISimpleTransferService secureTransferService)
        {
            this.mapperService = mapperService;
            this.exchangeRateServices = exchangeRateServices;
            this.secureTransferService = secureTransferService;
        }
        
        [HttpPost]
        [Route("setSafkhanRate")]
        public IActionResult SetNecRate(SetSafkhanRateRequest request)
        {
            try 
            {
                SetSafkhanRateResponse response = new SetSafkhanRateResponse();
                if (secureTransferService.ValidateUser(request))
                {
                    if (!exchangeRateServices.CheckCountryCode(request.FromCountryCode, request.ToCountryCode))
                    {
                        response.isSuccess = false;
                        response.ErrorCode = (int)Enumerations.EnumValidationFailureError.INVALID_COUNTRYCODE;
                        response.ErrorDescription = Enumerations.EnumValidationFailureError.INVALID_COUNTRYCODE.ToDescription();
                        return Ok(response);
                    }
                    if (!exchangeRateServices.CheckCurrencyCode(request.FromCurrencyCode, request.ToCurrencyCode))
                    {
                        response.isSuccess = false;
                        response.ErrorCode = (int)Enumerations.EnumValidationFailureError.INVALID_CURRENCYCODE;
                        response.ErrorDescription = Enumerations.EnumValidationFailureError.INVALID_CURRENCYCODE.ToDescription();
                        return Ok(response);
                    }
                    if (!exchangeRateServices.CheckPaymentMode(Convert.ToInt32(request.PaymentMode)))
                    {
                        response.isSuccess = false;
                        response.ErrorCode = (int)Enumerations.EnumValidationFailureError.INVALID_PAYMENTMETHOD;
                        response.ErrorDescription = Enumerations.EnumValidationFailureError.INVALID_PAYMENTMETHOD.ToDescription();
                        return Ok(response);
                    }
                    if (exchangeRateServices.CreateOrUpdateExchangeRate(request).Result && exchangeRateServices.CreateOrUpdateExchangeRateHistory(request).Result)
                    {
                        response.isSuccess = true;
                        response.ExchangeRateValue = request.ExchangeRateValue;
                        response.FromCountryCode = request.FromCountryCode;
                        response.ToCountryCode = request.ToCountryCode;
                        response.UpdatedOn = DateTime.Now.ToString();
                        return Ok(response);
                    }
                    else
                    {
                        ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR);

                        response.isSuccess = false;
                        response.ErrorCode = error.ErrorCode;
                        response.ErrorDescription = error.ErrorDescription;
                        return Ok(response);
                    }
                }
                else
                {
                    ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INVALID_CREDENTIAL);
                    response.isSuccess = false;
                    response.ErrorCode = error.ErrorCode;
                    response.ErrorDescription = error.ErrorDescription;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                SetSafkhanRateResponse response = new SetSafkhanRateResponse();
                response.ErrorCode = (int)System.Net.HttpStatusCode.Forbidden;
                response.ErrorDescription = ex.Message;
                return Ok(response);
            }
            finally
            {
                //log request and response
            }        
        }
    }
}