using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cicero.Service.Helpers;
using Cicero.Service.Services.SimpleTransfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Cicero.Data.Entities.SimpleTransfer;
using Cicero.Service.Models.Core;
using Cicero.Service.Services;
using System.Text.RegularExpressions;
using static Cicero.Data.Enumerations;
using Microsoft.AspNetCore.Http;
using Cicero.Data.Extensions;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer;
using Cicero.Data.Entities.JazzCash;
using Cicero.Data;
using Cicero.Service.Services.JazzCash;
using Cicero.Service.Services.API;
using System.Text;
using LexisNexis;
using Cicero.Service.Configuration;

namespace SimpleTransferAPI.Controllers
{

    [Route("api/lexisnexis")]
    [ApiController]
    public class LexisNexisApi : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly string _username;
        private readonly string _password;
        private readonly string _reportUrl;
        private readonly List<KeyValuePair<string, string>> externalHeaders;
        private readonly SimpleTransferApplicationDbContext db;
        private readonly IMapper mapper;
        private iduPortClient iduPortClient = new iduPortClient(iduPortClient.EndpointConfiguration.iduPort);

        public LexisNexisApi(IConfiguration config, SimpleTransferApplicationDbContext db, IMapper mapper)
        {
            this.config = config;
            this.db = db;
            this.mapper = mapper;
            var values = config.GetSection("ExternalHeaders:LexisNexis").GetChildren().ToDictionary(x => x.Key, x => x.Value).ToList();
            _username = values.Where(x => x.Key == "Username").Select(x => x.Value).FirstOrDefault();
            _password = values.Where(x => x.Key == "Password").Select(x => x.Value).FirstOrDefault();
            _reportUrl = values.Where(x => x.Key == "ReportUrl").Select(x => x.Value).FirstOrDefault();
            externalHeaders = new List<KeyValuePair<string, string>>();
            externalHeaders.Add(new KeyValuePair<string, string>("username", _username));
            externalHeaders.Add(new KeyValuePair<string, string>("password", _password));
        }

        [HttpPost]
        [Route("iduprocess")]
        public IActionResult IDUProcess(Request datas)
        {
            try
            {
                // var requestParams = new Request();
                var requestParams = new Request();
                requestParams.Login = new LoginDetails();
                requestParams.IDU = new IDUDetails();
                requestParams.Person = new PersonDetails();
                requestParams.Services = new ServiceDetails();

                requestParams = mapper.Map<Request>(datas);

                requestParams.Login = new LoginDetails();
                requestParams.Login.username = _username;
                requestParams.Login.password = _password;

                if(requestParams.Person.docfront == null)
                {
                    requestParams.Person.docfront = new byte[0];
                }

                if (requestParams.Person.docback == null)
                {
                    requestParams.Person.docback = new byte[0];
                }


                Result result = iduPortClient.IDUProcess(requestParams);
                return Ok(result);
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

        [Route("retrievereport")]
        public async Task<IActionResult> RetrieveReport(string url)
        {
            try
            {
                var report = await WebApiService.InstanceForExternal.PostAsyncString<string>(url, true, externalHeaders);
                return Ok(report);
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

    }
}
