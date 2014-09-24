using System;
using System.Threading;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Testability;
using Magellan.Tests.Helpers;
using Magellan.Tests.Helpers.TypeGeneration;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class AsyncActionInvokerTests
    {
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
        public void ShouldExecuteOnBackgroundThread()
        {
            var testThreadId = Thread.CurrentThread.ManagedThreadId;
            var waitHandle = new AutoResetEvent(false);

            Controller.Method("ShowCustomer").Returns((int a) =>
                {
                    Assert.AreNotEqual(testThreadId, Thread.CurrentThread.ManagedThreadId);
                    waitHandle.Set();
                    return new DoNothingResult();
                }).MustBeCalled();

            Controller.Instance.ActionInvoker = new AsyncActionInvoker();
            Controller.Instance.Execute("ShowCustomer", new { a = 3 });

            waitHandle.WaitOne();
        }

        [Test]
        public void ShouldDispatchExceptionsToUIThread()
        {
            var testThreadId = Thread.CurrentThread.ManagedThreadId;
            var waitHandle = new AutoResetEvent(false);

            Controller.Method("ShowCustomer").Returns((int a) =>
            {
                Assert.AreNotEqual(testThreadId, Thread.CurrentThread.ManagedThreadId);
                throw new DivideByZeroException();
                return new DoNothingResult();
            }).MustBeCalled();

            var dispatcher = new ManualPumpDispatcher();

            var request = RequestBuilder.CreateRequest("ExampleController", "ShowCustomer", new {a = 3});
            request.Dispatcher = dispatcher;
            
            Controller.Instance.ActionInvoker = new AsyncActionInvoker();
            Controller.Instance.Execute(request.BuildControllerContext(Controller.Instance));

            Thread.Sleep(400);

            var ex = Assert.Throws<AsyncControllerExecutionException>(dispatcher.Pump);
            Assert.IsInstanceOf<DivideByZeroException>(ex.InnerException);
        }

        [Test]
        public void ShouldSetNameOfWorkerThread()
        {
            var waitHandle = new AutoResetEvent(false);

            Controller.Method("ShowCustomer").Returns((int a) =>
            {
                Assert.IsTrue(Thread.CurrentThread.Name.StartsWith("Navigation request: "));
                waitHandle.Set();
                return new DoNothingResult();
            }).MustBeCalled();

            Controller.Instance.ActionInvoker = new AsyncActionInvoker();
            Controller.Instance.Execute("ShowCustomer", new { a = 3 });

            waitHandle.WaitOne();
        }
    }
}
