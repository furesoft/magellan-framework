using System;
using Magellan.Exceptions;
using Magellan.Routing;
using Magellan.Tests.Helpers;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class RoutingBehavior
    {
        protected RouteResolver Resolver { get; set; }
        protected TestRouteCatalog Routes { get; set; }

        [SetUp]
        public void SetUp()
        {
            Routes = new TestRouteCatalog();   
            Resolver = new RouteResolver(Routes);
        }

        [Test]
        public void ShouldMatchSingleStatic()
        {
            Routes.Register("home");
            Resolver.MatchPathToRoute("home").AssertRoute();
        }

        [Test]
        public void ShouldMatchSingleVariable()
        {
            Routes.Register("{controller}");
            Resolver.MatchPathToRoute("blog").AssertRoute(controller => "blog");
        }

        [Test]
        public void ShouldMatchStaticVariable()
        {
            Routes.Register("blog/{title}");
            Resolver.MatchPathToRoute("blog/wpf").AssertRoute(title => "wpf");
        }

        [Test]
        public void ShouldMatchStaticVariableVariable()
        {
            Routes.Register("wiki/{title}/{rev}");
            Resolver.MatchPathToRoute("wiki/wpf/1").AssertRoute(title => "wpf", rev => "1");
        }

        [Test]
        public void ShouldMatchDefaultedParameters()
        {
            Routes.Register("{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional } );
            Resolver.MatchPathToRoute("").AssertRoute(controller => "Home", action => "Index", id => null);
            Resolver.MatchPathToRoute("foo").AssertRoute(controller => "foo", action => "Index", id => null);
            Resolver.MatchPathToRoute("foo/bar").AssertRoute(controller => "foo", action => "bar", id => null);
            Resolver.MatchPathToRoute("foo/bar/1").AssertRoute(controller => "foo", action => "bar", id => "1");
        }

        [Test]
        public void ShouldMatchUsingConstraints()
        {
            Routes.Register("blog/{id}", new { }, new { id = "^[0-9]+$" });
            Routes.Register("blog/{title}");
            Resolver.MatchPathToRoute("blog/1").AssertRoute(id => "1");
            Resolver.MatchPathToRoute("blog/12").AssertRoute(id => "12");
            Resolver.MatchPathToRoute("blog/foobar").AssertRoute(title => "foobar");
        }

        [Test]
        public void ShouldMatchCatchAll()
        {
            Routes.Register("blog/{id}/{*foo}");
            Resolver.MatchPathToRoute("blog/1").AssertRoute(id => "1");
            Resolver.MatchPathToRoute("blog/42/a/b/c").AssertRoute(id => "42", foo => "a/b/c");
        }

        [Test]
        public void ShouldCreatePathFromRoute()
        {
            Routes.Register("{controller}/{action}/{id}", new { id = "" });
            Resolver.MatchRouteToPath(new { controller = "Foo", action = "Bar" }).AssertPath("Foo/Bar");
            Resolver.MatchRouteToPath(new { controller = "Foo", action = "Bar", id = "1" }).AssertPath("Foo/Bar/1");
        }

        [Test]
        public void ShouldMatchFirstRouteWhenAmbiguous()
        {
            Routes.Register("patients/search", new { controller = "Search", action = "Search" });
            Routes.Register("patients/search/results", new { controller = "Search", action = "Results" });

            // In this example, since action isn't specified, we could match either of the defaulted routes 
            // above. In this case ASP.NET MVC routing selects the first route, so we should do the same.
            Resolver.MatchRouteToPath(new { controller = "Search" }).AssertPath("patients/search");
        }

        [Test]
        public void ShouldMatchCorrectRouteWhenUnambiguous()
        {
            Routes.Register("patients/search", new { controller = "Search", action = "Search" });
            Routes.Register("patients/search/results", new { controller = "Search", action = "Results" });

            // In this case we've specified the action, so it should be completely unambiguous
            Resolver.MatchRouteToPath(new { controller = "Search", action = "Search" }).AssertPath("patients/search");
            Resolver.MatchRouteToPath(new { controller = "Search", action = "Results" }).AssertPath("patients/search/results");
        }

        [Test]
        public void ShouldFailToMatchWhenNonParameterizedDefaultsAreNotMatched()
        {
            Routes.Register("patients/search", new { controller = "Search", action = "Search" });
            Routes.Register("patients/search/results", new { controller = "Search", action = "Results" });

            // This action won't meet either, and since the {action} value isn't a URL parameter, the route match should fail
            var match = Resolver.MatchRouteToPath(new { controller = "Search", action = "Test" });
            Assert.IsFalse(match.Success);
            Assert.AreEqual(
                "- Route with specification 'patients/search' did not match: The route was a close match, but the value of the 'action' parameter was expected to be 'Search', but 'Test' was provided instead." + Environment.NewLine +
                "- Route with specification 'patients/search/results' did not match: The route was a close match, but the value of the 'action' parameter was expected to be 'Results', but 'Test' was provided instead.",
                match.FailReason);
        }

        [Test]
        public void UnmatchedRoutesShouldProduceNiceErrorMessage()
        {
            Routes.Register("blog/{id}", new { }, new { id = "^[0-9]+$" });
            Routes.Register("wiki/{id}", new { }, new { id = "^[0-9]+$" });
            var match = Resolver.MatchPathToRoute("wiki/bambam");
            Assert.IsFalse(match.Success);
            Assert.AreEqual(
                "- Route with specification 'blog/{id}' did not match: Expected segment 'blog'; got 'wiki'." + Environment.NewLine +
                "- Route with specification 'wiki/{id}' did not match: Segment 'bambam' did not match the constraint on parameter 'id'.", 
                match.FailReason);
        }

        [Test]
        public void RoutePathFormatsAreVeryForgiving()
        {
            // In ASP.NET route formats cannot start with slashes and have a few other limitations. In the Magellan
            // implementation it would be harder to enforce this, and as it really doesn't buy anything, there's no 
            // need to - let's be forgiving of format inconsistencies.
            Routes.Register((string)null);
            Routes.Register("");
            Routes.Register("/");
            Routes.Register("abc");
            Routes.Register("abc/");
            Routes.Register("/abc");
            Routes.Register("/abc/");
            Routes.Register("/abc/def");
            Routes.Register("/abc////def");
            Routes.Register("/abc/\\\\\\def");
        }

        [Test]
        public void ValidationShouldPickUpCommonRouteErrorsAtRegistration()
        {
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{id}/{id}"));        // Same parameter name used
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{ID}/{*id}"));
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{*Foo}/{*foo}"));
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{*foo}/{*bar}"));    // Multiple catch-all's
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{*path}/hello"));    // Catch-all not at end of path
            Assert.Throws<InvalidRouteException>(() => Routes.Register("{sddsds/foosdd}"));       // Nonsense path
        }

        [Test]
        public void ShouldFailOnLongerRoutes()
        {
            Routes.Register("blog/{id}", new { }, new { id = "^[0-9]+$" });
            var match = Resolver.MatchPathToRoute("blog/1/2/3/4/5/6");
            Assert.IsFalse(match.Success);
            Assert.AreEqual(
                "- Route with specification 'blog/{id}' did not match: Route was initially matched, but the request contains additional unexpected segments.",
                match.FailReason);
        }
    }
}
