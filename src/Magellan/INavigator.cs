using System;
using Magellan.Abstractions;
using Magellan.Progress;
using Magellan.Routing;

namespace Magellan
{
    /// <summary>
    /// Navigators provide navigation services for a specific frame of navigation. They are created using 
    /// the <see cref="NavigatorFactory"/>.
    /// </summary>
    public interface INavigator : INavigationService
    {
        /// <summary>
        /// Gets the scheme that will be used for any URI's built by this navigator.
        /// </summary>
        /// <value>The scheme.</value>
        string Scheme { get; }

		/// <summary>
		/// Raised when the <see cref="Close"/> method is called.
		/// </summary>
    	event EventHandler CloseRequested;

        /// <summary>
        /// Gets the <see cref="INavigatorFactory"/> that produced this <see cref="INavigator"/>.
        /// </summary>
        /// <value>The factory.</value>
        INavigatorFactory Factory { get; }

    	/// <summary>
    	/// If this navigator was created in the context of a parent navigator, returns the parent.
    	/// </summary>
		INavigator Parent { get; }

        /// <summary>
        /// Gets a collection of registered progress observers.
        /// </summary>
        /// <value>The progress listeners.</value>
        NavigationProgressListenerCollection ProgressListeners { get; }

    	/// <summary>
    	/// Gets a state bag that can be used to share information between requests in a navigator. Used commonly when 
    	/// implementing wizards. 
    	/// </summary>
    	RouteValueDictionary State { get; }

		/// <summary>
		/// Closes this navigator, if supported. 
		/// </summary>
		void Close();

        /// <summary>
        /// Resolves and navigates to the given route request.
        /// </summary>
        /// <param name="request">The request.</param>
        void ProcessRequest(NavigationRequest request); 
    }
}