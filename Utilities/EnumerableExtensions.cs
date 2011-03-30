using System;
using System.Collections.Generic;

namespace Utilities {
    public static class EnumerableExtensions {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> items, Action<T> action) {
            foreach(var item in items) {
                action(item);
            }
            return items;
        }
    }
}