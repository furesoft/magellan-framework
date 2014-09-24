using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ControllerTests
    {
        #region SUT

        /// <summary>
        /// Most of the default return methods are protected. This class allows us to create a fake 
        /// controller which exposes them as public.
        /// </summary>
        public class DelegatingController : Controller
        {
            public DelegatingController()
            {
            }

            public DelegatingController(string controllerName, string actionName)
            {
                ControllerContext = new ControllerContext(this, RequestBuilder.CreateRequest(controllerName, actionName, null).BuildRequest(), Magellan.Framework.ViewEngines.CreateDefaults());
            }

            public TResult Call<TResult>(string methodName, params object[] args)
            {
                var method = GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.Name == methodName)
                    .Where(x => x.GetParameters().Length == args.Length)
                    .Where(x => x.GetParameters().Where((p,i) => args[i] == null || p.ParameterType.IsAssignableFrom(args[i].GetType())).Count() == x.GetParameters().Length)
                    .FirstOrDefault();

                if (method == null)
                {
                    Assert.Fail("Method {0} not found", methodName);
                }

                try
                {
                    return (TResult) method.Invoke(this, args);
                }
                catch (TargetInvocationException tex)
                {
                    Trace.WriteLine(tex.InnerException);
                    throw tex.InnerException;
                }
            }
        }

        #endregion

        #region Cancel
        
        [Test]
        public void CancelReturnsDoNothingResult()
        {
            var controller = new DelegatingController();
            var result = controller.Call<ActionResult>("Cancel");
            Assert.IsInstanceOf<DoNothingResult>(result);
        }

        #endregion

        #region Page

        [Test]
        public void PageShouldNotRequireRequestInformation()
        {
            var controller = new DelegatingController();
            controller.Call<ActionResult>("Page");
        }

        [Test]
        public void PageReturnsPageResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<PageResult>("Page");
            Assert.AreEqual(null, result.ViewName);
        }

        [Test]
        public void PageWithNameReturnsPageResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<PageResult>("Page", "Foo");
            Assert.AreEqual("Foo", result.ViewName);
        }

        #endregion

        #region Redirect 
        
        [Test]
        public void RedirectWithActionReturnsRedirectResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<RedirectResult>("Redirect", "NewAction");
            Assert.AreEqual("MyController", result.NewRequest.GetOrDefault<string>("controller"));
            Assert.AreEqual("NewAction", result.NewRequest.GetOrDefault<string>("action"));
        }

        [Test]
        public void RedirectWithActionAndParametersReturnsRedirectResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<RedirectResult>("Redirect", "NewAction", new object());
            Assert.AreEqual("MyController", result.NewRequest.GetOrDefault<string>("controller"));
            Assert.AreEqual("NewAction", result.NewRequest.GetOrDefault<string>("action"));
        }

        [Test]
        public void RedirectWithControllerActionAndParametersReturnsRedirectResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<RedirectResult>("Redirect", "NewController", "NewAction", new object());
            Assert.AreEqual("NewController", result.NewRequest.GetOrDefault<string>("controller"));
            Assert.AreEqual("NewAction", result.NewRequest.GetOrDefault<string>("action"));
        }

        #endregion

        #region Window

        [Test]
        public void WindowShouldNotRequireRequestInformation()
        {
            var controller = new DelegatingController();
            controller.Call<ActionResult>("Window");
        }

        [Test]
        public void WindowReturnsWindowResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<WindowResult>("Window");
            Assert.AreEqual(null, result.ViewName);
        }

        [Test]
        public void WindowWithNameReturnsWindowResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<WindowResult>("Window", "Foo");
            Assert.AreEqual("Foo", result.ViewName);
        }

        #endregion

        #region Dialog

        [Test]
        public void DialogShouldNotRequireRequestInformation()
        {
            var controller = new DelegatingController();
            controller.Call<ActionResult>("Dialog");
        }

        [Test]
        public void DialogReturnsDialogResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<DialogResult>("Dialog");
            Assert.AreEqual(null, result.ViewName);
        }

        [Test]
        public void DialogWithNameReturnsDialogResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<DialogResult>("Dialog", "Foo");
            Assert.AreEqual("Foo", result.ViewName);
        }

        #endregion

        #region Start Process

        [Test]
        public void StartProcessShouldReturnStartProcessResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<StartProcessResult>("StartProcess", new ProcessStartInfo("calc.exe"), true);
            Assert.AreEqual("calc.exe", result.StartInfo.FileName);
            Assert.IsTrue(result.WaitForExit);
        }

        [Test]
        public void StartProcessByNameShouldReturnStartProcessResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<StartProcessResult>("StartProcess", "calc.exe");
            Assert.AreEqual("calc.exe", result.StartInfo.FileName);
            Assert.IsFalse(result.WaitForExit);
        }

        [Test]
        public void StartProcessByNameWithArgumentsShouldReturnStartProcessResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<StartProcessResult>("StartProcess", "calc.exe", "help");
            Assert.AreEqual("calc.exe", result.StartInfo.FileName);
            Assert.AreEqual("help", result.StartInfo.Arguments);
            Assert.IsFalse(result.WaitForExit);
        }

        [Test]
        public void StartProcessByNameWaitShouldReturnStartProcessResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<StartProcessResult>("StartProcess", "calc.exe", true);
            Assert.AreEqual("calc.exe", result.StartInfo.FileName);
            Assert.IsTrue(result.WaitForExit);
        }

        [Test]
        public void StartProcessByNameWaitWithArgumentsShouldReturnStartProcessResult()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<StartProcessResult>("StartProcess", "calc.exe", "help", true);
            Assert.AreEqual("calc.exe", result.StartInfo.FileName);
            Assert.AreEqual("help", result.StartInfo.Arguments);
            Assert.IsTrue(result.WaitForExit);
        }

        [Test]
        public void StartProcessDoesNotWaitForExitByDefault()
        {
            var controller = new DelegatingController("MyController", "DoStuff");
            var result = controller.Call<StartProcessResult>("StartProcess", new ProcessStartInfo("calc.exe"));
            Assert.AreEqual("calc.exe", result.StartInfo.FileName);
            Assert.IsFalse(result.WaitForExit);
        }

        #endregion
    }
}
