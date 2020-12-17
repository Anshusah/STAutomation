using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cicero.Service.Configuration;
using Cicero.Service.Models;
using Cicero.Service.Models.SimpleTransfer.Onfido;
using Cicero.Service.Services;
using Cicero.Service.Services.API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cicero.Areas.Admin.Controllers.Api
{
   
    [Route("api/onfido")]
    [ApiController]
    public class OnfidoApiController : ControllerBase
    {
        
        private readonly IConfiguration _config;
        private readonly IMapperService _mapperService;
        private readonly string apiUrl;
        private readonly List<KeyValuePair<string, string>> externalHeaders;

        public OnfidoApiController(IConfiguration config, IMapperService mapperService)
        {
            _config = config;
            _mapperService = mapperService;
            apiUrl = _config.GetSection("Onfido").Value;
            externalHeaders= _config.GetSection("ExternalHeaders:Onfido").GetChildren().ToDictionary(x => x.Key, x => x.Value).ToList();
        }
        // GET api/values
        [HttpGet]
        [Route("applicants")]
        public async Task<IActionResult> GetAllApplicants()
        {
            string url = "/v3/applicants";
            var response = await WebApiService.InstanceForExternal.GetAsync<GetApplicantsResponseViewModel>(url.CompleteUrl(apiUrl), true, externalHeaders);
            var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(mapped);
        }       


        [HttpGet]
        [Route("applicants/{id}")]
        public async Task<IActionResult> GetApplicantById(string id)
        {
            string url = "/v3/applicants/" + id;
            var response = await WebApiService.InstanceForExternal.GetAsync<Applicant>(url.CompleteUrl(apiUrl), true, externalHeaders);
            var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(mapped);
        }

        [HttpPost]
        [Route("applicants")]
        public async Task<IActionResult> CreateApplicant(Applicant applicant)
        {
            string url = "/v3/applicants";
            var response = await WebApiService.InstanceForExternal.PostAsync<Applicant>(url.CompleteUrl(apiUrl), true, externalHeaders, applicant);
            var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(mapped);
        }

        [HttpPut]
        [Route("applicants/{id}")]
        public async Task<IActionResult> UpdateApplicant(string id, Applicant applicant)
        {
            string url = "/v3/applicants/" + id;
            var response = await WebApiService.InstanceForExternal.PutAsync<Applicant>(url.CompleteUrl(apiUrl), true, externalHeaders,applicant);
            var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(mapped);
        }

        [HttpDelete]
        [Route("applicants/{id}")]
        public async Task<IActionResult> DeleteApplicant(string id)
        {
            string url = "/v3/applicants/" + id;
            var response = await WebApiService.InstanceForExternal.DeleteAsync(url.CompleteUrl(apiUrl), true, externalHeaders);
            var standardResponse = new CiceroStandardResponse();
            standardResponse.Message = response.IsSuccessStatusCode ? "Success" : response.ReasonPhrase;
            standardResponse.Success = response.IsSuccessStatusCode ? true : false;
            return Ok(standardResponse);
        }

        //TODO: implement
        [HttpPost]
        [Route("applicants/{id}/restore")]
        public async Task<IActionResult> RestoreApplicant(string id)
        {
            string url = "/v3/applicants/" + id + "/restore";
            var response = await WebApiService.InstanceForExternal.PostAsync<Applicant>(url.CompleteUrl(apiUrl), true, externalHeaders);
            var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(mapped);
        }
        [HttpGet]

        [Route("documents/{id}")]
        public async Task<IActionResult> GetDocumentById(string id)
        {
            string url = "/v3/documents/" + id;
            var data = await WebApiService.InstanceForExternal.GetAsync<Document>(url.CompleteUrl(apiUrl), true, externalHeaders);
            return Ok(data);
        }
        [HttpGet]

        [Route("documents/getByApplicantId/{applicantId}")]
        public async Task<IActionResult> GetAllDocumentsByApplicantId(string applicantId)
        {
            string url = "/v3/documents?applicant_id=" + applicantId;
            return Ok(await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders));
        }
        [HttpGet]

        [Route("documents/download/{documentId}")]
        public async Task<IActionResult> DownloadDocuments(string documentId)
        {
            string url = "/v3/documents/" + documentId + "/download";
            var imageByteArray = await WebApiService.InstanceForExternal.GetImageAsync<byte[]>(url.CompleteUrl(apiUrl), true, externalHeaders);
           // var file = File(imageByteArray, "image/jpeg");

            return Ok(imageByteArray);
        }
        [HttpGet]

        [Route("live_photos/{id}")]
        public async Task<IActionResult> GetPhotoById(string id)
        {
            string url = "/v3/live_photos/" + id;
            return Ok(await WebApiService.InstanceForExternal.GetAsync<LivePhoto>(url.CompleteUrl(apiUrl), true, externalHeaders));
        }
        [HttpGet]

        [Route("live_photos/getByApplicantId/{applicantId}")]
        public async Task<IActionResult> GetAllPhotosByApplicantId(string applicantId)
        {
            string url = "/v3/live_photos?applicant_id=" + applicantId;
            return Ok(await WebApiService.InstanceForExternal.GetAsync<object>(url.CompleteUrl(apiUrl), true, externalHeaders));
        }
        [HttpGet]

        [Route("live_photos/download/{photoId}")]
        public async Task<IActionResult> DownloadPhotos(string photoId)
        {
            string url = "/v3/live_photos/" + photoId + "/download";
            var imageByteArray = await WebApiService.InstanceForExternal.GetImageAsync<byte[]>(url.CompleteUrl(apiUrl), true, externalHeaders);
           // var file = File(imageByteArray, "image/jpeg");

            return Ok(imageByteArray);
        }

        [HttpPost]
        [Route("generate_sdktoken")]
        public async Task<IActionResult> GenerateSdk(SDKToken datas)
        {
            string url = "/v3/sdk_token";
            var response = await WebApiService.InstanceForExternal.PostAsync<SDKTokenResponse>(url.CompleteUrl(apiUrl), true, externalHeaders, datas);
            return Ok(response);
        }

        [HttpPost]
        [Route("createcheck")]
        public async Task<IActionResult> CreateCheck(CreateCheck datas)
        {
            string url = "/v3/checks";
            var response = await WebApiService.InstanceForExternal.PostAsync<Check>(url.CompleteUrl(apiUrl), true, externalHeaders, datas);
           // var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("checks")]
        public async Task<IActionResult> GetAllChecks()
        {
            string url = "/v3/checks";
            var response = await WebApiService.InstanceForExternal.GetAsync<Check>(url.CompleteUrl(apiUrl), true, externalHeaders);
           // var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(response);
        }


        [HttpGet]
        [Route("checks/{id}")]
        public async Task<IActionResult> GetCheckById(string id)
        {
            string url = "/v3/checks/" + id;
            var response = await WebApiService.InstanceForExternal.GetAsync<Check>(url.CompleteUrl(apiUrl), true, externalHeaders);
           // var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(response);
        }

        [HttpGet]
        [Route("reports/{id}")]
        public async Task<IActionResult> GetReportById(string id)
        {
            string url = "/v3/reports/" + id;
            var response = await WebApiService.InstanceForExternal.GetAsync<Report>(url.CompleteUrl(apiUrl), true, externalHeaders);
            // var mapped = _mapperService.MapToCiceroStandardResponse(response);
            return Ok(response);
        }
    }
}
