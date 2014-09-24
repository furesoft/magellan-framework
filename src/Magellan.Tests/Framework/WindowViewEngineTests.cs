using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class WindowViewEngineTests : ViewEngineTestBase
    {
        public WindowViewEngineTests()
            : base(() => new WindowViewEngine(new DefaultViewActivator()))
        {
        }

        [Test]
        public void FiltersByTypesDerivedFromWindow()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Page>("MyProject.Views.Home.AWindow");
            Project.DefineView<Window>("MyProject.Views.Home.ADialog");
            Project.DefineView<Page>("MyProject.Views.Home.A");

            var result = (WindowViewEngineResult)FindViewForController("MyProject.Controllers.HomeController", "A", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
            Assert.AreEqual("MyProject.Views.Home.ADialog", result.ViewType.FullName);
        }

        [Test]
        public void IgnoresFindViewWhenViewTypeIsNotWindowOrDialog()
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
            Project.DefineView<Window>("MyProject.Views.Home.Home");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithWindow()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeWindow");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithDialog()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeDialog");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithView()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeView");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithViewWindow()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeViewWindow");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithViewDialog()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeViewDialog");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home", new { ViewType = "Window" });
            Assert.IsTrue(result.Success);
        }
    }
}