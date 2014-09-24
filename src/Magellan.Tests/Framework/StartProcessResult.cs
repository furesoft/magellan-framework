using System.Diagnostics;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class StartProcessResultTests
    {
        [Test]
        public void ShouldStartProcess()
        {
            var result = new StartProcessResult(new ProcessStartInfo("calc.exe"), false);
            result.Execute(RequestBuilder.CreateRequest().BuildControllerContext());
            Assert.IsNotNull(result.StartedProcess);
            result.StartedProcess.Kill();
        }

        [Test]
        public void ShouldStartProcessWithArguments()
        {
            var result = new StartProcessResult(new ProcessStartInfo("calc.exe", "help"), false);
            result.Execute(RequestBuilder.CreateRequest().BuildControllerContext());
            Assert.IsNotNull(result.StartedProcess);
            Assert.AreEqual("help", result.StartedProcess.StartInfo.Arguments);
            result.StartedProcess.Kill();
        }

        [Test]
        public void ShouldWaitForExitIfAsked()
        {
            var result = new StartProcessResult(new ProcessStartInfo("xcopy"), true);
            result.Execute(RequestBuilder.CreateRequest().BuildControllerContext());
            Assert.IsNotNull(result.StartedProcess);
            Assert.IsTrue(result.StartedProcess.HasExited);
        }
    }
}
