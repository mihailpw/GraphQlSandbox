using System.Collections.Generic;

namespace GQL.WebApp.Typed.GraphQl.Extensions.Core
{
    internal static class EnumerableExtensions
    {
        public static T[] ToArray<T>(this IEnumerable<T> enumerable, int length)
        {
            var array = new T[length];
            var i = 0;
            foreach (var value in enumerable)
            {
                array[i] = value;
                i++;
            }

            return array;
        }
    }
}