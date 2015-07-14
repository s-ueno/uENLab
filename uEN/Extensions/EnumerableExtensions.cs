using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uEN.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 中央値を取得します。
        /// </summary>
        public static double Median<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            return source.Select(selector).Median();
        }
        /// <summary>
        /// 中央値を取得します。
        /// </summary>
        public static decimal Median<T>(this IEnumerable<T> source, Func<T, decimal> selector)
        {
            return source.Select(selector).Median();
        }
        /// <summary>
        /// 中央値を取得します。
        /// </summary>
        public static decimal Median(this IEnumerable<decimal> source)
        {
            return (decimal)Median(source.Select(x => (double)x));
        }
        /// <summary>
        /// 中央値を取得します。
        /// </summary>
        public static double Median(this IEnumerable<double> source)
        {
            if (source.Count() == 0)
            {
                throw new InvalidOperationException("Cannot compute median for an empty set.");
            }

            var sortedList = from number in source
                             orderby number
                             select number;

            int itemIndex = (int)sortedList.Count() / 2;

            if (sortedList.Count() % 2 == 0)
            {
                // Even number of items. 
                return (sortedList.ElementAt(itemIndex) + sortedList.ElementAt(itemIndex - 1)) / 2;
            }
            else
            {
                // Odd number of items. 
                return sortedList.ElementAt(itemIndex);
            }
        }

    }
}
