using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILoveBaku.MVC.Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty<T>(this List<T> array)
        {
            bool IsNull = true;
            if (array == null)
                return IsNull;

            foreach (var item in array)
            {
                if (item == null)
                    return IsNull;
            }

            IsNull = false;
            return IsNull;
        }
    }
}
