using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cicero.Data;
using Cicero.Service.Extensions;
using Cicero.Service.Helpers;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Cicero.Service.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "AuthToken";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
            {
                context.Result = new JsonResult(new ValidationFailureError()
                {
                    ErrorCode = (int)Enumerations.EnumValidationFailureError.AUTHTOKEN_REQUIRED,
                    ErrorDescription = Enumerations.EnumValidationFailureError.AUTHTOKEN_REQUIRED.ToDescription()
                });
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var tokenValidate = context.HttpContext.RequestServices.GetRequiredService<ISimpleTransferService>();
            try
            {
                if (tokenValidate.ValidateToken(potentialApiKey))
                {
                    await next();
                }
                else
                {
                    context.Result = new JsonResult(new ValidationFailureError()
                    {
                        ErrorCode = (int)Enumerations.EnumValidationFailureError.INVALID_USERTOKEN,
                        ErrorDescription = Enumerations.EnumValidationFailureError.INVALID_USERTOKEN.ToDescription()
                    }); 
                    return;  
                }
            }
            catch(Exception ex)
            {
                context.Result = new JsonResult(new ValidationFailureError() { 
                    ErrorCode= (int)Enumerations.EnumValidationFailureError.INVALID_USERTOKEN,
                    ErrorDescription= Enumerations.EnumValidationFailureError.INVALID_USERTOKEN.ToDescription()
                });
                return;
            }
        }
    }
}