using System.Collections;
using System.Linq;

namespace Magellan.Utilities
{
    internal static class EnumerableExtensions
    {
        public static bool HasSameItemsRegardlessOfSortOrder(this IEnumerable left, IEnumerable right)
        {
            var leftCollection = left.Cast<object>().ToList();
            var rightCollection = right.Cast<object>().ToList();
            
            return leftCollection.Except(rightCollection).Count() == 0;
        }
    }
}