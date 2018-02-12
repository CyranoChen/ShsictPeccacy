using System;
using System.Collections.Generic;
using System.Linq;

namespace Shsict.Peccancy.Service.Extension
{
    public static class CollectionExtensions
    {
        // Load All Records
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var skip = pageIndex * pageSize;

            if (skip > 0)
                source = source.Skip(skip);

            source = source.Take(pageSize);

            return source;
        }

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            return source.Where(instance => seenKeys.Add(keySelector(instance)));
        }

        public static IEnumerable<TKey> DistinctOrderBy<T, TKey>(this IEnumerable<T> instances,
            Func<T, TKey> keySelector)
        {
            return instances.DistinctBy(keySelector).OrderBy(keySelector).Select(keySelector);
        }
    }
}