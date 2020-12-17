﻿using Cicero.Service.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Cicero.Service.Library.Toastr
{
    class TempDataWrapper : ITempDataWrapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;

        public TempDataWrapper(ITempDataDictionaryFactory tempDataDictionaryFactory, IHttpContextAccessor httpContextAccessor)
        {
            _tempDataDictionaryFactory = tempDataDictionaryFactory;
            _httpContextAccessor = httpContextAccessor;
            _serializerSettings = GetSerializerSettings();
        }

        private JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
    }

        /// <summary>
        /// Gets or sets <see cref="ITempDataDictionary"/>/>.
        /// </summary>
        private ITempDataDictionary TempData => _tempDataDictionaryFactory?.GetTempData(_httpContextAccessor.HttpContext);

        public T Get<T>(string key) where T : class
        {
            if (TempData.ContainsKey(key))
            {
                return JsonConvert.DeserializeObject<T>(TempData[key] as string);
            }
            return default(T);
        }

        public T Peek<T>(string key) where T : class
        {
            if (TempData.ContainsKey(key))
            {
                return JsonConvert.DeserializeObject<T>(TempData.Peek(key) as string);
            }
            return default(T);
        }

        public void Add(string key, object value)
        {
            TempData[key] = value.ToJson();
        }

        public bool Remove(string key)
        {
            return TempData.ContainsKey(key) && TempData.Remove(key);
        }
    }
}