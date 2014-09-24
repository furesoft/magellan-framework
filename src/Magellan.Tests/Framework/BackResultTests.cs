using Magellan.Framework;
using Magellan.Tests.Helpers;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class BackResultTests
    {
        [Test]
        public void BackResultWithRemoveShouldCallNavigationService()
        {
            var request = RequestBuilder.CreateRequest();

            var backResult = new BackResult(true);
            backResult.Execute(request.BuildControllerContext());
            request.Navigator.Verify(x => x.GoBack(true));
        }

        [Test]
        public void BackResultWithoutRemoveShouldCallNavigationService()
        {
            var request = RequestBuilder.CreateRequest();

            var backResult = new BackResult(false);
            backResult.Execute(request.BuildControllerContext());
            request.Navigator.Verify(x => x.GoBack(false));
        }
    }
}
