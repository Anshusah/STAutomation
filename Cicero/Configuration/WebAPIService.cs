using Cicero.Service.Helpers;
using Cicero.Service.Models.SimpleTransfer.SecureTrading;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Configuration
{
    public class WebApiService
    {
        private WebApiService()
        {

        }
        private WebApiService(string baseUri)
        {
            BaseUri = baseUri;
        }

        private static WebApiService _instance;
        private static WebApiService _instanceExternal;

        public static WebApiService Instance => _instance ?? (_instance = new WebApiService(ConfigurationManager.AppSettings["WebApiUri"]));

        public static WebApiService InstanceForExternal => _instanceExternal ?? (_instanceExternal = new WebApiService());

        public string BaseUri { get; private set; }

        public async Task<T> AuthenticateAsync<T>(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                //client.AddHeaders();

                var result = await client.PostAsync(BuildActionUri("token"), new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("userName", userName),
                    new KeyValuePair<string, string>("password", password)
                }));

                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var res = json.Replace("\\\"", "\"").Replace("\"[", "[").Replace("]\"", "]").Replace("\"{", "{").Replace("}\"", "}");
                    return JsonConvert.DeserializeObject<T>(res);
                }

                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<T> GetAsync<T>(string action, bool isExternal = false, string sectionKey = "")
        {
//#pragma warning disable CS0618 // Type or member is obsolete
//            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
//#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                //if (isExternal && sectionKey != "")
                //    client.AddExternalHeaders(sectionKey);
                //else
                //    client.AddHeaders();

                var result = await client.GetAsync(BuildActionUri(action));

                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }

        public T Get<T>(string action, bool isExternal = false, string sectionKey = "")
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                                            (se, cert, chain, sslerror) =>
                                            {
                                                return true;
                                            };

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                //if (isExternal && sectionKey != "")
                //    client.AddExternalHeaders(sectionKey);
                //else
                //    client.AddHeaders();

                var result = client.GetAsync(BuildActionUri(action)).Result;

                string json = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
        public void Get(string action)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                //client.AddHeaders();
                var result = client.GetAsync(BuildActionUri(action)).Result;
                var json = result.Content.ReadAsStringAsync().Result;
                throw new ApiException(result.StatusCode, json);
            }
        }
        public async Task GetAsync(string action)
        {
            using (var client = new HttpClient())
            {
                //client.AddHeaders();

                var result = await client.GetAsync(BuildActionUri(action));
                if (result.IsSuccessStatusCode)
                {
                    return;
                }
                var json = await result.Content.ReadAsStringAsync();
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<T> PostAsync<T>(string action, object data)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                //client.AddHeaders();

                var result = await client.PostAsJsonAsync(BuildActionUri(action), data);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
        public async Task<T> PostAsync<T>(string action, bool isExternal = false, string sectionKey = "", object data = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                //if (isExternal && sectionKey != "")
                //    client.AddExternalHeaders(sectionKey);
                //else
                //    client.AddHeaders();
                var result = await client.PostAsJsonAsync(BuildActionUri(action), data);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
        public T Post<T>(string action, bool isExternal = false, string sectionKey = "", object data = null)
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            #pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                //if (isExternal && sectionKey != "")
                //    client.AddExternalHeaders(sectionKey);
                //else
                //    client.AddHeaders();

                var result = client.PostAsJsonAsync(BuildActionUri(action), data).Result;
                string json = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
        public T Post<T>(string action, object data)
        {
            using (var client = new HttpClient())
            {
                //client.AddHeaders();
                client.Timeout = TimeSpan.FromMinutes(6);
                var result = client.PostAsJsonAsync(BuildActionUri(action), data).Result;
                string json = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
        public async Task PostAsync(string action, object data = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                //client.AddHeaders();

                var result = await client.PostAsJsonAsync(BuildActionUri(action), data);
                if (result.IsSuccessStatusCode)
                {
                    return;
                }
                string json = await result.Content.ReadAsStringAsync();
                throw new ApiException(result.StatusCode, json);
            }
        }
        public async Task<T> PostAsync<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null, object data = null)
        {
        #pragma warning disable CS0618 // Type or member is obsolete
                    //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        #pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                }

                var result = await client.PostAsJsonAsync(BuildActionUri(action), data);
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }
        private string BuildActionUri(string action)
        {
            return BaseUri + action;
        }
        public async Task<T> PostJsonContent<T>(string uri, HttpClient httpClient, dynamic request, List<KeyValuePair<string, string>> sectionKey = null)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = JsonContent.Create(request)
            };
            httpClient.AddExternalHeaders(sectionKey);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("text/html"));
            postRequest.Content.Headers.Add("Content-Length", JsonConvert.SerializeObject(request).Length.ToString());
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.Timeout = TimeSpan.FromMinutes(30);
            var postResponse = await httpClient.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            string json = await postResponse.Content.ReadAsStringAsync();
            httpClient.Dispose();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

}
