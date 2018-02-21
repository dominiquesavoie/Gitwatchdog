using System.Collections.Generic;

namespace GitWatchdog.Presentation.Extensions
{
    public static class Enumerable
    {
        public static IEnumerable<T> Safe<T>(this IEnumerable<T> source)
        {
            return source ?? new T[0];
        }
    }
}
