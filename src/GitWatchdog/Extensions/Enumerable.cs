using System.Collections.Generic;

namespace GitWatchdog.Extensions
{
    public static class Enumerable
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source)
        {
            return source ?? new T[0];
        }
    }
}
