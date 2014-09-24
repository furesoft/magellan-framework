using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Magellan.Controls
{
    /// <summary>
    /// A content presenter that presents the content from a zone provided by a shared layout.
    /// </summary>
    [ContentProperty("Content")]
    public class ZonePlaceHolder : ContentControl
    {
        /// <summary>
        /// Initializes the <see cref="Zone"/> class.
        /// </summary>
        static ZonePlaceHolder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZonePlaceHolder), new FrameworkPropertyMetadata(typeof(ZonePlaceHolder)));
        }
    }
}