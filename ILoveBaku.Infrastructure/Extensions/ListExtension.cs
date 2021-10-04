using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILoveBaku.Infrastructure.Extensions
{
    public static class ListExtension
    {
        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable()
                .Select(s => s.Trim())
                .ToList();
        }

        public static void ForEachItems<T>(this IEnumerable<T> ienumerable, Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (ienumerable == null) return;

            foreach (T ienumerableItem in ienumerable) action(ienumerableItem);
        }
    }
}
