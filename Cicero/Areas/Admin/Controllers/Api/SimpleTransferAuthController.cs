using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cicero.Data;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Microsoft.AspNetCore.Mvc;


namespace Cicero.Areas.Admin.Controllers.Api
{
    [Route("api/simpletransfer")]
    [ApiExplorerSettings(IgnoreApi = true)]

    [ApiController]
    public class SimpleTransferAuthController : ControllerBase
    {
        private readonly IMapperService mapperService;
        private readonly IExchangeRateServices exchangeRateServices;
        private readonly ISimpleTransferService secureTransferService;

        public SimpleTransferAuthController(IMapperService mapperService, IExchangeRateServices exchangeRateServices, ISimpleTransferService secureTransferService)
        {
            this.mapperService = mapperService;
            this.exchangeRateServices = exchangeRateServices;
            this.secureTransferService = secureTransferService;
        }
        [HttpPost]
        [Route("auth")]
        public IActionResult STAuth(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try 
            {
                response = secureTransferService.GetUserToken(request);
                if (response != null)
                    return Ok(response);
                else {
                    response.ErrorCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    response.ErrorDescription = Enumerations.EnumValidationFailureError.INVALID_USERTOKEN.ToDescription();
                    return Ok(response); 
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = (int)System.Net.HttpStatusCode.InternalServerError;
                response.ErrorDescription = Enumerations.EnumValidationFailureError.INTERNAL_SERVER_ERROR.ToDescription();
                return Ok(response);
            }
        }

        [Route("validAuth")]
        public IActionResult STValidAuth(string token)
        {
            object response;
            bool result = false;
            try
            {
                result = secureTransferService.ValidateToken(token);
                response = new { Success = true, StatusCode = 200, Message = "Valid Successful", DataList = "", Data = ""};
                return Ok(response);
            }
            catch (Exception ex)
            {
                ValidationFailureError error = General.GetValidationError(Enumerations.EnumValidationFailureError.INVALID_USERTOKEN);
                response = new { Success = false, StatusCode = StatusCode((int)System.Net.HttpStatusCode.InternalServerError, error.ErrorDescription), Message = error.ErrorDescription, DataList = "", Data = "" };
                return Ok(response);
            }
        }

    }
}