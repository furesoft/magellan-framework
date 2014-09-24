using NUnit.Framework;

namespace Magellan.Tests.Helpers
{
    /// <summary>
    /// A simple base class for tests that need a WPF Window.
    /// </summary>
    public class UITestBase
    {
        protected TestWindow Window { get; private set; }
        
        protected void ExecuteTest()
        {
            Setup();
        }

        protected void ProcessEvents()
        {
            Window.ProcessEvents();
        }

        protected void ProcessEventsSlow()
        {
            Window.ProcessEvents(600);
        }

        protected virtual void AfterSetup()
        {
        }

        protected virtual void BeforeTearDown()
        {
        }

        [SetUp]
        public void Setup()
        {
            Window = new TestWindow() { Width = 300, Height = 300 };
            Window.Show();
            Window.Activate();
            Window.ProcessEvents();

            AfterSetup();
        }

        [TearDown]
        public void TearDown()
        {
            BeforeTearDown();

            Window.ProcessEvents();
            Window.Close();
        }
    }
}