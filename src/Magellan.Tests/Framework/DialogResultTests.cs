using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class DialogResultTests
    {
        [Test]
        public void ShouldAllowNullDialogName()
        {
            new DialogResult(null, null, new ViewEngineCollection());
        }

        [Test]
        public void ShouldSetViewTypeToDialog()
        {
            var result = new DialogResult("MyWindow", null, new ViewEngineCollection());
            Assert.AreEqual("Dialog", result.Options["ViewType"]);
        }

        [Test]
        public void ShouldSetModel()
        {
            var result = new DialogResult("MyWindow", "Hello", new ViewEngineCollection());
            Assert.AreEqual("Hello", result.Options["Model"]);
        }
    }
}