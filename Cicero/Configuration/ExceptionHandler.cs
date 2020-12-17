using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cicero.Configuration
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string jsonData)
        {
            StatusCode = statusCode;
            JsonData = jsonData;
        }
        public HttpStatusCode? StatusCode { get; set; }
        public string JsonData { get; private set; }
    }

    public class BadRequest
    {
        public string Error { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
        public string Message { get; set; }
        public Dictionary<string, ICollection<string>> ModelState { get; set; }
        [JsonProperty("exceptionMessage")]
        public string ExceptionMessage { get; set; }
    }
}
