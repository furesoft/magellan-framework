using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class PageViewEngineTests : ViewEngineTestBase
    {
        public PageViewEngineTests()
            : base(() => new PageViewEngine(new DefaultViewActivator()))
        {
        }

        [Test]
        public void FiltersByTypesDerivedFromPage()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.AWindow");
            Project.DefineView<Page>("MyProject.Views.Home.APage");
            Project.DefineView<Window>("MyProject.Views.Home.A");

            var result = (PageViewEngineResult)FindViewForController("MyProject.Controllers.HomeController", "A", new { ViewType = "Page" });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("MyProject.Views.Home.APage", result.ViewType.FullName);
        }

        [Test]
        public void IgnoresFindViewWhenViewTypeIsNotPage()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Page>("MyProject.Views.Home.AWindow");
            Project.DefineView<Window>("MyProject.Views.Home.ADialog");
            Project.DefineView<Page>("MyProject.Views.Home.A");

            var result = FindViewForController("MyProject.Controllers.HomeController", "A");
            Assert.IsFalse(result.Success);
            Assert.AreEqual(0, result.SearchLocations.Count());
        }

        [Test]
        public void FindViewSameName()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Page>("MyProject.Views.Home.Home");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Page" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithPage()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Page>("MyProject.Views.Home.HomePage");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Page" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithView()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Page>("MyProject.Views.Home.HomeView");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Page" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithViewPage()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Page>("MyProject.Views.Home.HomeViewPage");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Page" });
            Assert.IsTrue(result.Success);
        }
    }
}