using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Navigation;
using Magellan.Abstractions;
using Magellan.Exceptions;
using Magellan.Progress;
using Magellan.Routing;
using Magellan.Utilities;
using Magellan.Views;

namespace Magellan
{
    /// <summary>
    /// The core navigation management object in Magellan. Navigation factories produce navigators, which are bound to a frame or other control.
    /// </summary>
    public class NavigatorFactory : INavigatorFactory
    {
        private readonly string uriScheme;
        private readonly NavigationProgressListenerCollection navigationProgressListeners = new NavigationProgressListenerCollection();
        private readonly IRouteResolver routes;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorFactory"/> class.
        /// </summary>
        /// <param name="routeCatalogs">The route catalogs that will provide routes for this navigator. You
        /// can use ControllerRouteCatalog for Model-View-Controller, or TODO for
        /// Model-View-ViewModel.</param>
        public NavigatorFactory(params RouteCatalog[] routeCatalogs)
            : this(null, routeCatalogs)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorFactory"/> class.
        /// </summary>
        /// <param name="uriScheme">The URI scheme that will prefix handled URI's.</param>
        /// <param name="routeCatalogs">The route catalogs that will provide routes for this navigator. You
        /// can use a ControllerRouteCatalog for Model-View-Controller, or TODO for
        /// Model-View-ViewModel.</param>
        public NavigatorFactory(string uriScheme, params RouteCatalog[] routeCatalogs)
            : this(uriScheme, new RouteResolver(routeCatalogs))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorFactory"/> class.
        /// </summary>
        /// <param name="routes">The routes that navigators created from this factory will use when resolving requests.</param>
        public NavigatorFactory(IRouteResolver routes) 
            : this(null, routes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigatorFactory"/> class.
        /// </summary>
        /// <param name="uriScheme">The URI scheme that will prefix handled URI's.</param>
        /// <param name="routes">The routes that navigators created from this factory will use when resolving requests.</param>
        public NavigatorFactory(string uriScheme, IRouteResolver routes)
        {
            uriScheme = uriScheme ?? "magellan";

            Guard.ArgumentNotNullOrEmpty(uriScheme, "uriScheme");
            Guard.ArgumentNotNull(routes, "routes");

            if (!Uri.CheckSchemeName(uriScheme))
            {
                throw new ArgumentException(string.Format("The scheme '{0}' is not a valid URI scheme.", uriScheme));
            }

            this.uriScheme = uriScheme;
            this.routes = routes;
        }

        /// <summary>
        /// Gets a collection of objects that will be notified as the navigation infrastructure raises navigation events.
        /// </summary>
        /// <value>The progress listeners.</value>
        public NavigationProgressListenerCollection ProgressListeners
        {
            get { return navigationProgressListeners; }
        }

    	/// <summary>
    	/// Creates an <see cref="INavigator"/> bound to the specified navigation service. This method can 
    	/// be called multiple times for the same <paramref name="navigationService"/>.
    	/// </summary>
    	/// <param name="navigationService">The navigation service which will be used if the view renders 
    	/// page information.</param>
    	/// <returns>
    	/// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
    	/// </returns>
    	public INavigator CreateNavigator(INavigationService navigationService)
        {
        	return CreateNavigator(navigationService, null);
        }

    	/// <summary>
    	/// Creates an <see cref="INavigator"/> bound to the specified navigation service. This method can 
    	/// be called multiple times for the same <paramref name="navigationService"/>.
    	/// </summary>
    	/// <param name="navigationService">The navigation service which will be used if the view renders 
    	/// page information.</param>
    	/// <param name="parent">The parent navigator that created this navigator.</param>
    	/// <returns>
    	/// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
    	/// </returns>
    	public INavigator CreateNavigator(INavigationService navigationService, INavigator parent)
		{
			Guard.ArgumentNotNull(navigationService, "navigationService");
			
			return ExistingNavigator.Get(navigationService, 
				() => NewNavigator(() => navigationService, parent));
    	}

    	/// <summary>
        /// Creates an <see cref="INavigator"/> bound to the specified frame. This method can
        /// be called multiple times for the same <paramref name="frame"/>.
        /// </summary>
        /// <param name="frame">The navigation service which will be used if the view renders page information.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
        public INavigator CreateNavigator(Frame frame)
    	{
    		return CreateNavigator(frame, null);
    	}

    	/// <summary>
    	/// Creates an <see cref="INavigator"/> bound to the specified frame. This method can 
    	/// be called multiple times for the same <paramref name="frame"/>.
    	/// </summary>
    	/// <param name="frame">The navigation service which will be used if the view renders page information.</param>
    	/// <param name="parent">The parent navigator that created this navigator.</param>
    	/// <returns>
    	/// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
    	/// </returns>
    	public INavigator CreateNavigator(Frame frame, INavigator parent)
		{
			Guard.ArgumentNotNull(frame, "frame");
			
			return ExistingNavigator.Get(frame, 
				() => NewNavigator(() => new FrameNavigationServiceWrapper(frame.Dispatcher, frame), parent));
    	}

    	/// <summary>
        /// Creates an <see cref="INavigator"/> bound to the specified frame. This method can
        /// be called multiple times for the same <paramref name="frame"/>.
        /// </summary>
        /// <param name="frame">The navigation service which will be used if the view renders page information.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
        public INavigator CreateNavigator(ContentControl frame)
    	{
    		return CreateNavigator(frame, null);
    	}

    	/// <summary>
    	/// Creates an <see cref="INavigator"/> bound to the specified frame. This method can 
    	/// be called multiple times for the same <paramref name="frame"/>.
    	/// </summary>
    	/// <param name="frame">The navigation service which will be used if the view renders page information.</param>
    	/// <param name="parent">The parent navigator that created this navigator.</param>
    	/// <returns>
    	/// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
    	/// </returns>
    	public INavigator CreateNavigator(ContentControl frame, INavigator parent)
		{
			Guard.ArgumentNotNull(frame, "frame");


			if (frame is ButtonBase || frame is Label)
			{
				throw new ArgumentException("An attempt was made to create an INavigator for a '{0}', which should not be used as navigation containers. If the intention was to find a navigator that owns the UI element, the NavigatorFactory.GetOwningNavigator method should be used instead.");
			}

			return ExistingNavigator.Get(frame, () => 
				frame is Frame
    		       		? CreateNavigator((Frame) frame)
						: NewNavigator(() => new ContentNavigationServiceWrapper(frame), parent));
		}

    	/// <summary>
        /// Creates an <see cref="INavigator"/> bound to the navigation service that owns a given source
        /// element. This method can  be called multiple times for the same <paramref name="sourceElement"/>.
        /// </summary>
        /// <param name="sourceElement">A UI element that lives inside the frame that you want a navigator
        /// for.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
        public INavigator GetOwningNavigator(UIElement sourceElement)
        {
            Guard.ArgumentNotNull(sourceElement, "sourceElement");

            var existingNavigator = NavigationProperties.GetNavigator(sourceElement);
            if (existingNavigator != null)
            {
                return existingNavigator;
            }

            Func<INavigationService> lazyFrameGetter = () =>
            {
                var frame = null as Frame;
                DependencyObject parent = sourceElement;
                while (parent != null)
                {
                    frame = parent as Frame;
                    if (frame != null)
                        break;
                    parent = VisualTreeHelper.GetParent(parent);
                }

                if (frame == null)
                {
                    throw new ImpossibleNavigationRequestException("The Navigator for this UI element is not avaialable. Please ensure the current view has been loaded into a frame, or that the NavigationProperties.Navigator attached property has been set.");
                }

                var wrapper = new FrameNavigationServiceWrapper(sourceElement.Dispatcher, frame);
                return wrapper;
            };

            return NewNavigator(lazyFrameGetter, null);
        }

        private INavigator NewNavigator(Func<INavigationService> navigationService, INavigator parent)
        {
			return new Navigator(this, parent, uriScheme, routes, navigationService);
        }

		private static class ExistingNavigator
		{
			private static readonly DependencyProperty ExistingNavigatorProperty = DependencyProperty.RegisterAttached("ExistingNavigator", typeof(INavigator), typeof(ExistingNavigator), new UIPropertyMetadata(null));

			public static INavigator Get(object element, Func<INavigator> createNew)
			{
				INavigator existing = null;

				var dependencyObject = element as DependencyObject;
				if (dependencyObject != null)
					existing = (INavigator) dependencyObject.GetValue(ExistingNavigatorProperty);

				var service = element as INavigationService;
				if (service != null)
					existing = (INavigator)service.GetValue(ExistingNavigator.ExistingNavigatorProperty);

				if (existing == null)
				{
					existing = createNew();

					if (dependencyObject != null)
						dependencyObject.SetValue(ExistingNavigatorProperty, existing);

					if (service != null)
						service.SetValue(ExistingNavigatorProperty, existing);
				}

				return existing;
			}
		}
    }
}