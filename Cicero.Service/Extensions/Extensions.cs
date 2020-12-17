using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Cicero.Service.Models.General;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Cicero.Service.Extensions
{
    public static class Extensions
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
        public static long ToUnixTime(this DateTime dateTime)
        {
            return (dateTime - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static string SubstringFixed(this string str, int? maxLength = 50)
        {
            StringBuilder sb = new StringBuilder(str);

            if (sb.Length > maxLength)
            {
                return sb.ToString(0, maxLength.Value) + "..";
            }
            else
            {
                return sb.ToString();
            }
        }

        public static string GetInnerTextFromXmlNode(this XmlNode node)
        {
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }

        public static dynamic CastStringToDataType(this string str, Type castTo)
        {
            try
            {
                return Convert.ChangeType(str, castTo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }

        public static string ToDescription(this Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                if (attributes.Length > 0)
                    return attributes[0].Description;
                return value.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static class EnumModel<T>
        {
            public static IEnumerable<EnumViewModel> List()
            {
                var names = GetDescriptions(typeof(T));
                var values = Enum.GetValues(typeof(T)).Cast<int>();

                return names.Zip(values, (name, value) => new EnumViewModel() { Text = name, Id = value }).ToList();
            }

            public static string GetDescription(dynamic value)
            {
                var name = Enum.GetName(typeof(T), value);
                var names = GetDescriptions(typeof(T));
                var values = Enum.GetValues(typeof(T)).Cast<int>();
                var enumList = names.Zip(values, (n, v) => new EnumViewModel() { Text = n, Id = v }).ToList();

                return enumList.Where(x => x.Id == value).Select(x => x.Text).FirstOrDefault();
            }
        }
        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var names = Enum.GetNames(type);
            return (from name in names let field = type.GetField(name) let fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true) select fds.Any() ? ((DescriptionAttribute)fds[0]).Description : name).ToList();
        }

        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }      

    }
}
