using Magellan.Framework;
using Magellan.Routing;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class RedirectResultTests
    {
        [Test]
        public void RedirectResultShouldUseSuppliedNavigator()
        {
            var request = RequestBuilder.CreateRequest();
            var newRequest = new RouteValueDictionary(new {controller = "Foo", action = "Bar", abc = "123"});
            var result = new RedirectResult(newRequest);
            result.Execute(request.BuildControllerContext());
            request.Navigator.Verify(x => x.ProcessRequest(It.IsAny<NavigationRequest>()));
        }
    }
}
