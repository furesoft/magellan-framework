using System.Windows;
using Transitionals;
using Transitionals.Transitions;

namespace Magellan.Transitionals.Transitions
{
    /// <summary>
    /// A transition that creates a zoom-out effect.
    /// </summary>
    public class ZoomOutTransition : StoryboardTransition
    {
        private static readonly ZoomOutFrameworkElement FrameworkElement = new ZoomOutFrameworkElement();

        /// <summary>
        /// Initializes the <see cref="ZoomOutTransition"/> class.
        /// </summary>
        static ZoomOutTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(ZoomOutTransition), new FrameworkPropertyMetadata(NullContentSupport.Both));
            IsNewContentTopmostProperty.OverrideMetadata(typeof(ZoomOutTransition), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomOutTransition"/> class.
        /// </summary>
        public ZoomOutTransition()
        {
            OldContentStyle = FrameworkElement.OldStyle;
            OldContentStoryboard = FrameworkElement.OldStoryboard;
            NewContentStyle = FrameworkElement.NewStyle;
            NewContentStoryboard = FrameworkElement.NewStoryboard;
        }
    }
}