using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer2
{
    internal static class Extensions
    {
        internal static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null) return true;

            var collection = source as ICollection<T>;
            if (collection != null) return collection.Count == 0;

            var readOnlyCollection = source as IReadOnlyCollection<T>;
            if (readOnlyCollection != null) return readOnlyCollection.Count > 0;

            return source.GetEnumerator().MoveNext();
        }
    }
}