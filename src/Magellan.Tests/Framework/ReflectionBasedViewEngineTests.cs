using System;
using System.Collections.Generic;
using System.Windows;
using Magellan.Framework;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ReflectionBasedViewEngineTests : ViewEngineTestBase
    {
        public ReflectionBasedViewEngineTests() : base(() => new DummyViewEngine())
        {
        }

        #region SUT

        public class DummyViewEngine : ReflectionBasedViewEngine, IViewNamingConvention
        {
            public DummyViewEngine()
            {
                NamingConvention = this;
            }

            public IEnumerable<string> GetAlternativeNames(ControllerContext controllerContext, string baseName)
            {
                yield return baseName;
                yield return baseName + "Window";
                yield return baseName + "Dialog";
            }

            protected override IEnumerable<Type> FilterCandidateTypes(ControllerContext controllerContext, ViewResultOptions viewParameters, string viewName, IEnumerable<Type> candidates)
            {
                return candidates;
            }

            protected override ViewEngineResult CreateViewResult(ControllerContext controllerContext, ViewResultOptions viewParameters, Type type)
            {
                return new ViewEngineResult(true, null);
            }
        }

        #endregion

        [Test]
        public void FindViewSameName()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.Home");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home");
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithWindow()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeWindow");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home");
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewSuffixedWithDialog()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.HomeDialog");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Home");
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void FindViewWalksHierarchy()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.A");
            Project.DefineView<Window>("MyProject.Views.B");
            Project.DefineView<Window>("MyProject.C");
            Project.DefineView<Window>("D");

            Assert.IsTrue(FindViewForController("MyProject.Controllers.HomeController", "A").Success);
            Assert.IsTrue(FindViewForController("MyProject.Controllers.HomeController", "B").Success);
            Assert.IsTrue(FindViewForController("MyProject.Controllers.HomeController", "C").Success);
            Assert.IsTrue(FindViewForController("MyProject.Controllers.HomeController", "D").Success);
        }

        [Test]
        public void FindViewTriesToFindClosestViewToController()
        {
            Project.DefineController("MyProject.Controllers.HomeController");
            Project.DefineView<Window>("MyProject.Views.Home.A");

            var result = FindViewForController("MyProject.Controllers.HomeController", "Megatron");
            Assert.IsFalse(result.Success);

            var searchLocations = new Queue<string>(result.SearchLocations);
            Assert.AreEqual("MyProject.Controllers.Views.Home.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Home.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Home.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Megatron.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Megatron.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Megatron.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Home.Megatron.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Home.Megatron.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Home.Megatron.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Views.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Home.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Home.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Home.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Controllers.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Home.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Home.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Home.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Megatron.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Megatron.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Megatron.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Home.Megatron.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Home.Megatron.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Home.Megatron.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Views.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Home.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Home.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Home.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MyProject.MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual("Megatron", searchLocations.Dequeue());
            Assert.AreEqual("MegatronWindow", searchLocations.Dequeue());
            Assert.AreEqual("MegatronDialog", searchLocations.Dequeue());
            Assert.AreEqual(0, searchLocations.Count);


        }

    }
}
