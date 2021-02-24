using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace PMVOnline.Api.Extensions
{
    public static class StringExtensions
    {
        public static T DeserializeObject<T>(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch
            {
                return default(T);
            }
        }

        public static string SerializeObject<T>(this T obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool IsEmailValid(this string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;
            if ((Regex.Match(address, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" + @"((([0-9a-zA-Z][-0-9a-zA-Z]*[0-9a-zA-Z]*\.)+[a-z0-9A-Z][\-a-z0-9A-Z]{0,22}[a-z0-9A-Z]))$").Success && (!string.IsNullOrWhiteSpace(address))))
                return true;
            return false;
        }

        public static string RemoveLeadingZero(this string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return phone;
            }
            var match = Regex.Match(phone, @"^0([0-9]{6,13})$");
            if (match.Success == false)
                return phone;
            return match.Groups[1].Value;
        }

        public static bool IsPhoneValid(this string ph)
        {
            if (ph == null || string.IsNullOrWhiteSpace(ph))
                return false;
            return Regex.Match(ph, @"^[0-9]{6,13}$").Success;
        }

        public static bool IsNumber(this string ph)
        {
            if (ph == null || string.IsNullOrWhiteSpace(ph))
                return false;
            return Regex.Match(ph, @"^[0-9]+$").Success;
        }

        public static DateTime ToNormalDateTime(this string time, string pattern = null, int expandTime = 0)
        {
            if (string.IsNullOrEmpty(time))
            {
                return DateTime.Now;
            }
            try
            {
                return DateTime.ParseExact(time, pattern ?? "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.Now.AddMinutes(expandTime);
            }
        }
    }
}
