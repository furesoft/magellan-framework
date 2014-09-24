using System.Windows;
using System.Windows.Controls;
using Magellan.Abstractions;
using Magellan.Progress;

namespace Magellan
{
    /// <summary>
    /// The service that builds <see cref="INavigator">navigators</see> for various types of frames.
    /// </summary>
    public interface INavigatorFactory
    {
        /// <summary>
        /// Gets a collection of objects that will be notified as the navigation infrastructure raises navigation events.
        /// </summary>
        /// <value>The progress listeners.</value>
        NavigationProgressListenerCollection ProgressListeners { get; }

        /// <summary>
        /// Creates an <see cref="INavigator"/> bound to the specified navigation service. This method can 
        /// be called multiple times for the same <paramref name="navigationService"/>.
        /// </summary>
        /// <param name="navigationService">The navigation service which will be used if the view renders 
        /// page information.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
		INavigator CreateNavigator(INavigationService navigationService);

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
		INavigator CreateNavigator(INavigationService navigationService, INavigator parent);

        /// <summary>
        /// Creates an <see cref="INavigator"/> bound to the navigation service that owns a given source 
        /// element. This method can  be called multiple times for the same <paramref name="sourceElement"/>.
        /// </summary>
        /// <param name="sourceElement">A UI element that lives inside the frame that you want a navigator 
        /// for.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
        INavigator GetOwningNavigator(UIElement sourceElement);

        /// <summary>
        /// Creates an <see cref="INavigator"/> bound to the specified frame. This method can 
        /// be called multiple times for the same <paramref name="frame"/>.
        /// </summary>
        /// <param name="frame">The navigation service which will be used if the view renders page information.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
		INavigator CreateNavigator(Frame frame);

    	/// <summary>
    	/// Creates an <see cref="INavigator"/> bound to the specified frame. This method can 
    	/// be called multiple times for the same <paramref name="frame"/>.
    	/// </summary>
    	/// <param name="frame">The navigation service which will be used if the view renders page information.</param>
    	/// <param name="parent">The parent navigator that created this navigator.</param>
    	/// <returns>
    	/// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
    	/// </returns>
    	INavigator CreateNavigator(Frame frame, INavigator parent);

        /// <summary>
        /// Creates an <see cref="INavigator"/> bound to the specified frame. This method can 
        /// be called multiple times for the same <paramref name="frame"/>.
        /// </summary>
        /// <param name="frame">The navigation service which will be used if the view renders page information.</param>
        /// <returns>
        /// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
        /// </returns>
		INavigator CreateNavigator(ContentControl frame);

		/// <summary>
		/// Creates an <see cref="INavigator"/> bound to the specified frame. This method can 
		/// be called multiple times for the same <paramref name="frame"/>.
		/// </summary>
		/// <param name="frame">The navigation service which will be used if the view renders page information.</param>
		/// <param name="parent">The parent navigator that created this navigator.</param>
		/// <returns>
		/// An instance of the <see cref="INavigator"/> interface which can be used for navigation.
		/// </returns>
		INavigator CreateNavigator(ContentControl frame, INavigator parent);
    }
}