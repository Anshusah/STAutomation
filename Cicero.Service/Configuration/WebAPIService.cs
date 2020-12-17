using Cicero.Service.Helpers;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Service.Configuration
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
        public async Task<T> GetAsync<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null)
        {
            //#pragma warning disable CS0618 // Type or member is obsolete
            //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //#pragma warning restore CS0618 // Type or member is obsolete

            try
            {
                using (var client = new HttpClient())
                {
                    if (sectionKey != null)
                    {
                        if (isExternal && sectionKey.Count > 0)
                        {
                            client.AddExternalHeaders(sectionKey);
                        }
                    }
                    var result = await client.GetAsync(BuildActionUri(action));

                    string json = await result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                throw new ApiException(HttpStatusCode.InternalServerError, "Internal Server Error.");
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
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
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
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
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
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
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
        private string BuildActionUri(string action)
        {
            return BaseUri + action;
        }
        public async Task<byte[]> GetImageAsync<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null)
        {
            //#pragma warning disable CS0618 // Type or member is obsolete
            //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                }
                var result = await client.GetAsync(BuildActionUri(action));

                var type = result.Content.Headers.ContentType.MediaType;
                if (type == "image/jpeg" || type == "image/png")
                {
                    Byte[] byteArray = await result.Content.ReadAsByteArrayAsync();
                    return byteArray;
                }

                throw new ApiException(result.StatusCode, "");
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

        public async Task<string> PostAsyncString<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null, object data = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                }

                var result = await client.PostAsJsonAsync(BuildActionUri(action), data);
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return json;
                }
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<T> PostAsyncTransfast<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null, object data = null)
        {


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                }
                var myContent = JsonConvert.SerializeObject(data);

                var httpContent = new StringContent(myContent, Encoding.UTF8, "application/json");

                var result = await client.PostAsync(BuildActionUri(action), httpContent);
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<T> PutAsync<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null, object data = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                }

                var result = await client.PutAsJsonAsync(BuildActionUri(action), data);
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<T> PutAsyncTransfast<T>(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null, object data = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                }

                var myContent = JsonConvert.SerializeObject(data);

                var httpContent = new StringContent(myContent, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(BuildActionUri(action), httpContent);
                string json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                throw new ApiException(result.StatusCode, json);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string action, bool isExternal = false, List<KeyValuePair<string, string>> sectionKey = null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            //     ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
#pragma warning restore CS0618 // Type or member is obsolete


            using (var client = new HttpClient())
            {
                if (isExternal && sectionKey.Count > 0)
                {
                    client.AddExternalHeaders(sectionKey);
                }

                var result = await client.DeleteAsync(BuildActionUri(action));
                string json = await result.Content.ReadAsStringAsync();
                return result;
                throw new ApiException(result.StatusCode, json);
            }
        }
    }

}
