using System.Windows.Controls;
using Magellan.Behaviors;
using Magellan.Routing;
using Magellan.Tests.Helpers;
using Magellan.Views;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Behaviors
{
    [TestFixture]
    public class NavigateBehaviorTests : UITestBase
    {
        public NavigateBehaviorTests()
        {
            // Ensure the assembly containing the behaviors is loaded into the AppDomain, 
            // so that the XAML tags can be resolved
            new NavigateControllerAction();
        }

        protected Mock<INavigator> Navigator = new Mock<INavigator>();

        private void ExpectNavigationRequest(string controllerName, string actionName, object parameters)
        {
            Navigator.Setup(x => x.ProcessRequest(It.IsAny<NavigationRequest>())).Callback(
                (NavigationRequest request) =>
                {
                    Assert.AreEqual(request.RouteData["action"], actionName);
                    Assert.AreEqual(request.RouteData["controller"], controllerName);
                    var expectedParameters = new RouteValueDictionary(parameters);
                    foreach (var expectedParameter in expectedParameters)
                    {
                        var expected = expectedParameter.Value;
                        var paramName = expectedParameter.Key;
                        var actual = request.RouteData[expectedParameter.Key];
                        Assert.AreEqual(expected, actual, string.Format("Parameter {0}", paramName));
                    }
                });
        }

        [Test]
        public void ParametersCanUseDataContextBindings()
        {
            Window.LoadContent(
              @"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
	                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:magellan='http://xamlforge.com/magellan'
                    xmlns:i='clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity'
                    Name='MyButton'
                    Content='Hello'
                    >
                    <i:Interaction.Triggers>
                        <i:EventTrigger EventName='Click'>
                            <magellan:NavigateControllerAction x:Name='NavBehavior' Controller='MyController' Action='MyAction'>
                                <magellan:NavigateControllerAction.Parameters>
                                    <magellan:Parameter ParameterName='param' Value='{Binding}' />
                                    <magellan:Parameter ParameterName='paramA' Value='{Binding Path=A}' />
                                    <magellan:Parameter ParameterName='paramB' Value='{Binding Path=B}' />
                                </magellan:NavigateControllerAction.Parameters>
                            </magellan:NavigateControllerAction>
                        </i:EventTrigger>
                    </i:Interaction.Triggers>
                </Button>");

            var dataContext = new { A = 3, B = 8 };
            ExpectNavigationRequest("MyController", "MyAction", new { param = dataContext, paramA = dataContext.A, paramB = dataContext.B });
            Window.DataContext = dataContext;
            NavigationProperties.SetNavigator(Window, Navigator.Object);
            Window.Find<Button>("MyButton").ExecuteClick();
            Window.ProcessEvents();
        }

        [Test]
        public void ParametersCanUseElementBindings()
        {
            Window.LoadContent(
              @"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
	                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:i='clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity'
                    Name='MyButton'
                    Content='Bye!'
                    >
                    <i:Interaction.Triggers>
                        <i:EventTrigger EventName='Click'>
                            <NavigateControllerAction x:Name='NavBehavior' Controller='MyController' Action='MyAction'>
                                <NavigateControllerAction.Parameters>
                                    <Parameter ParameterName='btn' Value='{Binding ElementName=MyButton}' />
                                    <Parameter ParameterName='btnContent' Value='{Binding ElementName=MyButton, Path=Content}' />
                                    <Parameter ParameterName='x' Value='Wazoo' />
                                </NavigateControllerAction.Parameters>
                            </NavigateControllerAction>
                        </i:EventTrigger>
                    </i:Interaction.Triggers>
                </Button>");

            ExpectNavigationRequest("MyController", "MyAction", new { btn = Window.Find<Button>("MyButton"), btnContent = "Bye!", x = "Wazoo" });
            NavigationProperties.SetNavigator(Window, Navigator.Object);
            Window.Find<Button>("MyButton").ExecuteClick();
            Window.ProcessEvents();
        }

        [Test]
        public void ParametersCanUseRelativeSourceBindings()
        {
            Window.LoadContent(
              @"<Button xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
	                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:magellan='http://xamlforge.com/magellan'
                    xmlns:i='clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity'
                    Name='MyButton'
                    Content='Hello'
                    >
                    <i:Interaction.Triggers>
                        <i:EventTrigger EventName='Click'>
                            <magellan:NavigateControllerAction x:Name='NavBehavior' Controller='MyController' Action='MyAction'>
                                <magellan:NavigateControllerAction.Parameters>
                                    <magellan:Parameter ParameterName='win' Value='{Binding RelativeSource={RelativeSource FindAncestor, AncestorType=Window}}' />
                                    <magellan:Parameter ParameterName='winTitle' Value='{Binding RelativeSource={RelativeSource FindAncestor, AncestorType=Window}, Path=Title}' />
                                </magellan:NavigateControllerAction.Parameters>
                            </magellan:NavigateControllerAction>
                        </i:EventTrigger>
                    </i:Interaction.Triggers>
                </Button>");

            ExpectNavigationRequest("MyController", "MyAction", new { win = Window, winTitle = "Hello" });
            Window.Title = "Hello";
            NavigationProperties.SetNavigator(Window, Navigator.Object);
            Window.Find<Button>("MyButton").ExecuteClick();
            Window.ProcessEvents();
        }
    }
}
