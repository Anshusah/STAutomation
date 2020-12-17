using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Cicero.Service.Helpers
{
    public class General
    {
        //public static string Getsession()
        //{
        //    if (HttpContext.Current.Session.SessionID != null)
        //        return HttpContext.Current.Session.SessionID.ToString();
        //    return string.Empty;
        //}
        //public static string GetUserName()
        //{
        //    if (HttpContext.Current.Session["UserName"] != null)
        //        return HttpContext.Current.Session["UserName"].ToString();

        //    return string.Empty;
        //}
        //public static string Encrypt(string str)
        //{
        //    byte[] encbuff = Encoding.UTF8.GetBytes(str);
        //    return HttpServerUtility.UrlTokenEncode(encbuff);
        //}     


        public static bool IsNameValid(string firstName, string InviteCode)
        {
            firstName = firstName.Replace(" ", "").ToLower();
            firstName = firstName.Length > 10 ? firstName.Substring(0, 10) : firstName;

            if (InviteCode.ToLower().Contains(firstName))
                return true;
            else
                return false;
        }

        //public static string Decrypt(string str)
        //{
        //    string encstring = "";
        //    try
        //    {
        //        if (str != null)
        //        {
        //            byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);
        //            encstring = Encoding.UTF8.GetString(decbuff);
        //        }
        //        else
        //        {
        //            encstring = str;
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return encstring;

        //}

        public static string SetFormatDate(string dateString)
        {
            if (dateString == null) return null;
            if (string.IsNullOrEmpty(dateString)) return string.Empty;

            string[] datearr;

            if (dateString.Contains("/"))
                datearr = dateString.Split('/');
            else
                datearr = dateString.Split('-');

            CultureInfo CI = new CultureInfo("ms-MY");

            DateTime fromDateValue;
            string testDateString = "13" + "/" + datearr[0] + "/" + datearr[2];
            if (DateTime.TryParse(testDateString, CI, DateTimeStyles.None, out fromDateValue))
            {
                // return dateString;
                return datearr[1] + "/" + datearr[0] + "/" + datearr[2];
            }
            else
            {
                //do for in-valid date
                return datearr[1] + "/" + datearr[0] + "/" + datearr[2];
            }

        }

        public static ValidationFailureError GetValidationError(Enum errorCodeDesc)
        {
            ValidationFailureError vError = new ValidationFailureError();
            try
            {
                vError.ErrorDescription = errorCodeDesc.ToString();
                vError.Message = GetEnumDescriptionAttribute(errorCodeDesc);
            }
            catch
            {
                return null;
            }
            return vError;
        }

        public static string GetEnumDescriptionAttribute(Enum enumVal)
        {
            try
            {
                FieldInfo fi = enumVal.GetType().GetField(enumVal.ToString());

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return enumVal.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

       
        public static Boolean ValidateEmailAddress(string email)
        {
            try
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        public static Boolean ValidatePassword(string pwd)
        {
            //length >= 7,  Password containts at least three of the four available character types:
            //lowercase letters, uppercase letters, numbers, and symbols. 
            bool isError = false;
            if (pwd.Length < 8)
            {
                isError = true;
            }

            int totalCount = 0;
            int upperCount = 0;
            int lowerCount = 0;
            int numberCount = 0;
            int symbolCount = 0;

            foreach (Char C in pwd)
            {
                if (Char.IsUpper(C))
                {
                    upperCount = 1;
                }
                if (Char.IsLower(C))
                {
                    lowerCount = 1;
                }
                if (Char.IsNumber(C))
                {
                    numberCount = 1;
                }
                if (char.IsSymbol(C))
                {
                    symbolCount = 1;
                }
            }
            totalCount = upperCount + lowerCount + numberCount + symbolCount;

            if (isError || (totalCount < 3))
            {
                return false;
            }
            return true;
        }

        

        public static bool HasLettersOnly(string inputStr)
        {
            bool result = false;
            try
            {
                Regex objAlphaPattern = new Regex(@"^[a-zA-Z ]+$");
                result = objAlphaPattern.IsMatch(inputStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool HasNumbersAndSpecialCharacters(string inputStr)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(inputStr))
                {
                    Regex objAlphaPattern = new Regex(@"^[a-zA-Z ]+$");
                    result = !objAlphaPattern.IsMatch(inputStr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool IsPasswordValid(string password)
        {

            if (string.IsNullOrEmpty(password) || password.Length < 8 || password.Length > 25)
            {
                return false;
            }

            int totalCount = 0;
            int uCount = 0;
            int lCount = 0;
            int numCount = 0;
            int splCount = 0;

            Regex ucPattern = new Regex(@"[A-Z]");

            Regex lcPattern = new Regex(@"[a-z]");

            Regex numPattern = new Regex(@"[0-9]");

            Regex splcharacter = new Regex(@"[^A-Za-z0-9]");

            if (ucPattern.Match(password).Success)
            {
                uCount = 1;
            }
            if (lcPattern.Match(password).Success)
            {
                lCount = 1;
            }
            if (numPattern.Match(password).Success)
            {
                numCount = 1;
            }
            if (splcharacter.Match(password).Success)
            {
                splCount = 1;
            }
            totalCount = uCount + lCount + numCount + splCount;

            if ((totalCount >= 3) && (password.Length >= 8))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool IsFullNameValid(string fullName)
        {

            if (string.IsNullOrEmpty(fullName) || fullName.Length > 125)
            {
                return false;
            }

            int valid = 0;
            int invalid = 0;
            int uCount = 0;
            int lCount = 0;
            int numCount = 0;
            int splCount = 0;

            Regex ucPattern = new Regex(@"[A-Z]");

            Regex lcPattern = new Regex(@"[a-z]");

            Regex numPattern = new Regex(@"[0-9]");

            Regex splcharacter = new Regex(@"[^A-Za-z0-9]");

            if (ucPattern.Match(fullName).Success)
            {
                uCount = 1;
            }
            if (lcPattern.Match(fullName).Success)
            {
                lCount = 1;
            }
            if (numPattern.Match(fullName).Success)
            {
                numCount = 1;
            }
            if (splcharacter.Match(fullName).Success)
            {
                splCount = 1;
            }
            valid = uCount + lCount;
            invalid = numCount + splCount;

            if (valid >= 1 && invalid < 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}
