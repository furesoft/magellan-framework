using System.Windows;
using Transitionals;
using Transitionals.Transitions;

namespace Magellan.Transitionals.Transitions
{
    /// <summary>
    /// A transition that creates a zoom-in effect.
    /// </summary>
    public class ZoomInTransition : StoryboardTransition
    {
        private static readonly ZoomInFrameworkElement FrameworkElement = new ZoomInFrameworkElement();

        /// <summary>
        /// Initializes the <see cref="ZoomInTransition"/> class.
        /// </summary>
        static ZoomInTransition()
        {
            AcceptsNullContentProperty.OverrideMetadata(typeof(ZoomInTransition), new FrameworkPropertyMetadata(NullContentSupport.Both));
            IsNewContentTopmostProperty.OverrideMetadata(typeof(ZoomInTransition), new FrameworkPropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomInTransition"/> class.
        /// </summary>
        public ZoomInTransition()
        {
            OldContentStyle = FrameworkElement.OldStyle;
            OldContentStoryboard = FrameworkElement.OldStoryboard;
            NewContentStyle = FrameworkElement.NewStyle;
            NewContentStoryboard = FrameworkElement.NewStoryboard;
        }
    }
}