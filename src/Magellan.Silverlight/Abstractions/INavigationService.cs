using System.Windows.Navigation;

namespace Magellan.Abstractions
{
    public interface INavigationService
    {
        /// <summary>
        /// Occurs when navigation begins.
        /// </summary>
        event NavigatingCancelEventHandler Navigating;

        /// <summary>
        /// Occurs when navigation completes.
        /// </summary>
        event NavigatedEventHandler Navigated;

        /// <summary>
        /// Occurs when navigation fails.
        /// </summary>
        event NavigationFailedEventHandler NavigationFailed;

        /// <summary>
        /// Gets a value indicating whether the back button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets a value indicating whether the forward button should be enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go forward; otherwise, <c>false</c>.
        /// </value>
        bool CanGoForward { get; }

        /// <summary>
        /// Navigates to the last page in the browser journal.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        object Content { get; }

        /// <summary>
        /// Navigates to the next page in the browser journal.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Navigates to the specified content.
        /// </summary>
        /// <param name="content">The page.</param>
        /// <returns></returns>
        bool NavigateDirectToContent(object content);

        /// <summary>
        /// Navigates to the specified content.
        /// </summary>
        /// <param name="content">The page.</param>
        /// <returns></returns>
        bool NavigateDirectToContent(object content, object customState);

        /// <summary>
        /// Gets the dispatcher associated with this navigation service.
        /// </summary>
        /// <value>The dispatcher.</value>
        IDispatcher Dispatcher { get; }
    }
}
