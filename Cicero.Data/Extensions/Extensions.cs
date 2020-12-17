using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Cicero.Data.Extensions
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

        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var names = Enum.GetNames(type);
            return (from name in names let field = type.GetField(name) let fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true) select fds.Any() ? ((DescriptionAttribute)fds[0]).Description : name).ToList();
        }

    }
    public class GreaterThanAttribute : ValidationAttribute
    {

        public GreaterThanAttribute(string otherProperty)
            : base("{0} must be greater than {1}")
        {
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; set; }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) < 1)
                {
                    object obj = validationContext.ObjectInstance;
                    var thing = obj.GetType().GetProperty(OtherProperty);
                    var displayName = (DisplayAttribute)Attribute.GetCustomAttribute(thing, typeof(DisplayAttribute));

                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName, displayName.GetName()));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }
    public class LessThanAttribute : ValidationAttribute
    {

        public LessThanAttribute(string otherProperty)
            : base("{0} must be less than {1}")
        {
            OtherProperty = otherProperty;
        }

        public string OtherProperty { get; set; }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (firstComparable.CompareTo(secondComparable) > 1)
                {
                    object obj = validationContext.ObjectInstance;
                    var thing = obj.GetType().GetProperty(OtherProperty);
                    var displayName = (DisplayAttribute)Attribute.GetCustomAttribute(thing, typeof(DisplayAttribute));

                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName, displayName.GetName()));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }

}

