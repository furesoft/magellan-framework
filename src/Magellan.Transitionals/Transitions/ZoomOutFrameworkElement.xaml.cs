using System.Windows;
using System.Windows.Media.Animation;

namespace Magellan.Transitionals.Transitions
{
    /// <summary>
    /// Code behind for ZoomOutFrameworkElement.xaml.
    /// </summary>
    public partial class ZoomOutFrameworkElement : FrameworkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomOutFrameworkElement"/> class.
        /// </summary>
        public ZoomOutFrameworkElement()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the old storyboard.
        /// </summary>
        /// <value>The old storyboard.</value>
        public Storyboard OldStoryboard
        {
            get { return (Storyboard)Resources["OldStoryboard"]; }
        }

        /// <summary>
        /// Gets the new storyboard.
        /// </summary>
        /// <value>The new storyboard.</value>
        public Storyboard NewStoryboard
        {
            get { return (Storyboard)Resources["NewStoryboard"]; }
        }

        /// <summary>
        /// Gets the new style.
        /// </summary>
        /// <value>The new style.</value>
        public Style NewStyle
        {
            get { return (Style)Resources["NewStyle"]; }
        }

        /// <summary>
        /// Gets the old style.
        /// </summary>
        /// <value>The old style.</value>
        public Style OldStyle
        {
            get { return (Style)Resources["OldStyle"]; }
        }
    }
}
