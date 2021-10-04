using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ILoveBaku.Application.Common.Extension
{
    public static class Extension
    {
        public static bool IsNull<T>(this T value)
        {
            return value is null;
        }

        public static bool IsEmpty(this string value)
        {
            return value == string.Empty;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrZore(this int? value)
        {
            return value.IsNull() || value == 0;
        }

        public static bool IsZore(this int value)
        {
            return value == 0;
        }

        public static decimal PercentOf(this decimal number, decimal percentage)
        {
            return number * percentage / 100;
        }

        public static decimal PercentReductionOf(this decimal number, decimal percentage) => number - (number * percentage / 100);

        public static decimal Round(this decimal number, int decimals)
        {
            return (number % 1 == 0) ? Math.Round(number) : Math.Round(number, decimals);
        }

        public static string ToCapitalize(this string value)
        {
            return $"{char.ToUpper(value[0])}{value.Substring(1)}";
        }

        public static string ToCapitalize(this string value, CultureInfo cultureInfo)
        {
            return $"{char.ToUpper(value[0], cultureInfo)}{value.Substring(1)}";
        }

        public static string ToParameterizingRoute(this string value)
        {
            string trim = value.Trim();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < trim.Length; i++)
            {
                if (trim[i] != ' ') stringBuilder.Append(char.ToLower(trim[i], new CultureInfo("en-US")));
                else if (i != trim.Length - 1 && trim[i + 1] != ' ') stringBuilder.Append('-');
            }
            return stringBuilder.Replace("#", "").Replace("ə", "e").ToString();
        }

        public static string ToParameterizingRouteWithRegex(this string value)
        {
            return new Regex(@"\s\s+").Replace(value.Trim().ToLower(), "-"); //Regex.Replace(value, " {2,}", " ")
        }

        public static string ToQueryString<TRequestParameter>(this TRequestParameter parameter) where TRequestParameter : new()
        {
            if (parameter.IsNull())
                return string.Empty;

            var parameters = typeof(TRequestParameter).GetProperties()
                                                         .Where(p => !p.GetValue(parameter).IsNull())
                                                            .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(parameter)?.ToString())}");

            return $"?{string.Join('&', parameters)}";
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> keyValuePairs, Dictionary<TKey, TValue> elements)
        {
            foreach (var item in elements) keyValuePairs.Add(item.Key, item.Value);
        }

        public static void ForEach<T>(this IEnumerable<T> ienumerable, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (ienumerable == null) return;

            foreach (T ienumerableItem in ienumerable) action(ienumerableItem);
        }
    }
}
