using System;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using Magellan.Tests.Helpers.TypeGeneration;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class DefaultActionInvokerTests
    {
        #region Action and Result Filters

        public class SuppressExceptionsAttribute : Attribute, IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext context)
            {
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
                Assert.IsNotNull(context.ModelBinders);
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
                context.ExceptionHandled = true;
            }
        }

        public class SkipAttribute : Attribute, IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext context)
            {
                context.OverrideResult = new DoNothingResult();
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
            }
        }

        public class DoNothingResultAttribute : Attribute, IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext context)
            {
                context.Cancel = true;
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
                Assert.IsNotNull(context.Result);
            }
        }

        public class SuppressResultAttribute : Attribute, IResultFilter
        {
            public void OnResultExecuting(ResultExecutingContext context)
            {
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
            }

            public void OnResultExecuted(ResultExecutedContext context)
            {
                Assert.IsNotNull(context.ControllerContext);
                Assert.IsNotNull(context.Request);
                Assert.IsNotNull(context.Request.RouteValues);
                Assert.IsNotNull(context.Result);
                context.ExceptionHandled = true;
            }
        }

        public class ExplodeResult : ActionResult
        {
            protected override void ExecuteInternal(ControllerContext controllerContext)
            {
                throw new DivideByZeroException("Boom!");
            }
        }

        #endregion

        protected TypeBuilder<Controller> Controller { get; set; }

        [SetUp]
        public void SetUp()
        {
            Controller = new TypeBuilder<Controller>("ExampleController");
        }

        [TearDown]
        public void TearDown()
        {
            Controller.VerifyAll();
        }

        [Test]
        public void ShouldSelectCorrectAction()
        {
            Controller.Method("ShowCustomer").Returns(() => new DoNothingResult()).MustBeCalled();
            Controller.Instance.Execute("ShowCustomer");
        }

        [Test]
        public void ShouldSelectCorrectActionByParameters()
        {
            Controller.Method("ShowCustomer").Returns((int a) => new DoNothingResult()).MustNotBeCalled();
            Controller.Method("ShowCustomer").Returns((int a, int b) => new DoNothingResult()).MustBeCalled();
            Controller.Method("ShowCustomer").Returns((int a, int b, int c) => new DoNothingResult()).MustNotBeCalled();
            Controller.Instance.Execute("ShowCustomer", new { a = 1, b = 2});
        }

        [Test]
        public void ActionNamesAreNotCaseSensitive()
        {
            Controller.Method("ShowCustomer").Returns(() => new DoNothingResult()).MustBeCalled();
            Controller.Instance.Execute("ShowCustomer");
            Controller.Instance.Execute("ShowCustOMER");
            Controller.Instance.Execute("SHOWCUSTOMER");
        }

        [Test]
        public void ActionsMayNotBePrivate()
        {
            Controller.Method("ShowCustomer1").Private().Returns(() => new DoNothingResult()).MustNotBeCalled();
            Assert.Throws<ActionNotFoundException>(() => Controller.Instance.Execute("ShowCustomer1"));
        }

        [Test]
        public void ActionsMayNotBeProtected()
        {
            Controller.Method("ShowCustomer").Protected().Returns(() => new DoNothingResult()).MustNotBeCalled();
            Assert.Throws<ActionNotFoundException>(() => Controller.Instance.Execute("ShowCustomer"));
        }

        [Test]
        public void ActionsMayNotBeVoid()
        {
            Controller.Method("ShowCustomer").Returns(() => {}).MustNotBeCalled();
            Assert.Throws<ActionNotFoundException>(() => Controller.Instance.Execute("ShowCustomer"));
        }

        [Test]
        public void ActionsMustReturnActionResultOrDerived()
        {
            Controller.Method("ShowCustomer").Returns(() => "Hello").MustNotBeCalled();
            Assert.Throws<ActionNotFoundException>(() => Controller.Instance.Execute("ShowCustomer"));
        }

        [Test]
        public void ExceptionShouldNotBeWrapped()
        {
            Controller.Method("ShowCustomer").MustBeCalled().Returns(
                new Func<ActionResult>(() =>
                    {
                        throw new DivideByZeroException(); 
                    }));

            Assert.Throws<DivideByZeroException>(() => Controller.Instance.Execute("ShowCustomer"));
        }

        [Test]
        public void ExceptionsCanBeSuppressedByFilters()
        {
            Controller.Method("ShowCustomer")
                .Attribute<SuppressExceptionsAttribute>()
                .Returns(new Func<ActionResult>(() => { throw new NullReferenceException(); }))
                .MustBeCalled();

            Controller.Instance.Execute("ShowCustomer");
        }

        [Test]
        public void ActionShouldNotBeCalledWhenActionFilterOverridesResult()
        {
            Controller.Method("ShowCustomer")
                .Attribute<SkipAttribute>()
                .Returns(new Func<ActionResult>(() => { throw new NullReferenceException(); }))
                .MustNotBeCalled();

            Controller.Instance.Execute("ShowCustomer");
        }

        [Test]
        public void ResultsCanThrow()
        {
            Controller.Method("ShowCustomer")
                .Returns(() => new ExplodeResult())
                .MustBeCalled();

            Assert.Throws<DivideByZeroException>(() => Controller.Instance.Execute("ShowCustomer"));
        }

        [Test]
        public void ResultsCanBeAvoidedByCancellation()
        {
            Controller.Method("ShowCustomer")
                .Attribute<DoNothingResultAttribute>()
                .Returns(() => new ExplodeResult())
                .MustBeCalled();

            Controller.Instance.Execute("ShowCustomer");
        }

        [Test]
        public void ResultsCanBeAvoidedBySuppression()
        {
            Controller.Method("ShowCustomer")
                .Attribute<SuppressResultAttribute>()
                .Returns(() => new ExplodeResult())
                .MustBeCalled();

            Controller.Instance.Execute("ShowCustomer");
        }
    }
}
