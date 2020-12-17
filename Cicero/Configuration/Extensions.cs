
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace Cicero.Configuration
{
    public static class Extensions
    {
        public static string CompleteUrl(this string url, string domain)
        {
            return string.Concat(domain, url);
        }


        #region WebApiService
        public static void AddExternalHeaders(this HttpClient client, List<KeyValuePair<string, string>> sectionKey)
        {
            sectionKey.ForEach(h => client.DefaultRequestHeaders.Add(h.Key,h.Value));
        }
        #endregion
        #region ParamToClass
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }


        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
        #endregion
    }
}