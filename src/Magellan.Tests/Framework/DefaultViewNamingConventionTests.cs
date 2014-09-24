using System.Linq;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class DefaultViewNamingConventionTests
    {
        [Test]
        public void AppendsViewToName()
        {
            var convention = new DefaultViewNamingConvention();
            var names = convention.GetAlternativeNames(RequestBuilder.CreateRequest().BuildControllerContext(), "EditCustomer").ToArray();
            Assert.AreEqual(2, names.Length);
            Assert.AreEqual("EditCustomer", names[0]);
            Assert.AreEqual("EditCustomerView", names[1]);
        }
    }
}