using System;
using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Helpers
{
    public static class RouteMatchExtensions
    {
        public static void AssertRoute(this RouteMatch match, params Func<string, string>[] parameters)
        {
            Assert.IsTrue(match.Success, match.FailReason);
            foreach (var parameter in parameters)
            {
                var key = parameter.Method.GetParameters()[0].Name;
                var value = parameter(null);

                Assert.IsTrue(match.Values.ContainsKey(key), "The matched route does not contain a value for '{0}'", key);
                Assert.AreEqual(value, match.Values[key], "Error when matching parameter {0}", key);
            }
        }
    
        public static void AssertPath(this PathMatch match, string path)
        {
            Assert.IsTrue(match.Success, match.FailReason);
            Assert.AreEqual(path, match.Path);
        }
    }
}
