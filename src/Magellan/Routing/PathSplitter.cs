using System.Linq;

namespace Magellan.Routing
{
    /// <summary>
    /// Extension methods for splitting paths.
    /// </summary>
    internal static class PathSplitter
    {
        /// <summary>
        /// Splits the URL path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string[] SplitUrlPath(this string path)
        {
            if (string.IsNullOrEmpty(path)) return new string[0];
            return path 
                .Split('/', '\\')
                .Where(x => x.Length > 0)
                .ToArray();
        }
    }
}
