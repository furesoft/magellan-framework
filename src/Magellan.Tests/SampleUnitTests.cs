using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace Magellan.Tests
{
    [TestClass]
    public class SampleUnitTests
    {
        [TestMethod]
        public void ExampleTest()
        {
            var window = new Window() {Width = 100, Height = 100};
            window.Show();
            window.Activate();
            window.Dispatcher.InvokeShutdown();
            window.Close();
        }
    }
}
