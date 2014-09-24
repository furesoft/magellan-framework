using Magellan.Framework;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests
{
    [TestFixture]
    public class NavigatorExtensionsTests
    {
        #region SUT

        public class PatientController : Controller
        {
            public ActionResult Search(string text)
            {
                return Back();
            }
        }

        #endregion

        [Test]
        public void CanNavigateUsingControllerLambda()
        {
            var navigator = new Mock<INavigator>();
            navigator.Setup(x => x.ProcessRequest(It.IsAny<NavigationRequest>()))
                .Callback(delegate(NavigationRequest routeValues)
                {
                    Assert.AreEqual(3, routeValues.RouteData.Count);
                    Assert.AreEqual("Hello", routeValues.RouteData["text"]);
                    Assert.AreEqual("Patient", routeValues.RouteData["controller"]);
                    Assert.AreEqual("Search", routeValues.RouteData["action"]);
                });

            navigator.Object.Navigate<PatientController>(x => x.Search("Hello"));

            navigator.Verify(x => x.ProcessRequest(It.IsAny<NavigationRequest>()));
        }
    }
}
