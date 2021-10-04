using System;
using System.Collections.Generic;
using System.Text;

namespace ILoveBaku.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }
    }
}
