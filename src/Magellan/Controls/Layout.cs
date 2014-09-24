using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Magellan.Diagnostics;

namespace Magellan.Controls
{
    /// <summary>
    /// Presents a shared layout in the control tree, and merges the <see cref="Zone">zones</see> into the
    /// <see cref="ZonePlaceHolder">zone place holders</see> specified in the layout.
    /// </summary>
    [StyleTypedProperty(Property="ItemContainerStyle", StyleTargetType=typeof(Zone))]
    [ContentProperty("Zones")]
    public class Layout : Control, IAddChild
    {
        private bool sourceLoaded;

        /// <summary>
        /// Initializes the <see cref="Layout"/> class.
        /// </summary>
        static Layout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Layout), new FrameworkPropertyMetadata(typeof(Layout)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Layout"/> class.
        /// </summary>
        public Layout()
        {
            Loaded += MasterLoaded;
            Zones = new ZoneCollection();
        }

        /// <summary>
        /// Dependency propety for the <see cref="Zones"/> property.
        /// </summary>
        public static readonly DependencyProperty ZonesProperty = DependencyProperty.Register("Zones", typeof(ZoneCollection), typeof(Layout), new UIPropertyMetadata(null));
        
        /// <summary>
        /// Dependency propety for the <see cref="Source"/> property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(Layout), new UIPropertyMetadata(string.Empty, SourcePropertySet));

        /// <summary>
        /// Dependency property for the <see cref="Content"/> property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(ContentControl), typeof(Layout), new UIPropertyMetadata(null, ContentChanged));

        /// <summary>
        /// Gets or sets the zones.
        /// </summary>
        /// <value>The zones.</value>
        public ZoneCollection Zones
        {
            get { return (ZoneCollection)GetValue(ZonesProperty); }
            set { SetValue(ZonesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the path to the XAML file containing the layout template to use.
        /// </summary>
        /// <value>The source.</value>
        [Category("Appearance")]
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public ContentControl Content
        {
            get { return (ContentControl)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private static void SourcePropertySet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (Layout)d;

            @this.sourceLoaded = false;
            @this.LoadLayoutFromSource();
        }
 
        private void MasterLoaded(object sender, RoutedEventArgs e)
        {
            LoadLayoutFromSource();
        }

        private void LoadLayoutFromSource()
        {
            if (string.IsNullOrEmpty(Source)) return;
            if (!IsLoaded) return;
            if (sourceLoaded) return;
            sourceLoaded = true;

            Content = (ContentControl)Application.LoadComponent(new Uri(Source, UriKind.RelativeOrAbsolute));
        }

        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = ((Layout)d);
            @this.AddVisualChild(@this.Content);
            @this.AddLogicalChild(@this.Content);
            @this.ReloadZones();
            @this.InvalidateVisual();
        }

        /// <summary>
        /// Overrides <see cref="M:System.Windows.Media.Visual.GetVisualChild(System.Int32)"/>, and returns a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">The zero-based index of the requested child element in the collection.</param>
        /// <returns>
        /// The requested child element. This should not return null; if the provided index is out of range, an exception is thrown.
        /// </returns>
        protected override Visual GetVisualChild(int index)
        {
            return Content;
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of visual child elements for this element.
        /// </returns>
        protected override int VisualChildrenCount
        {
            get
            {
                return Content == null ? 0 : 1;
            }
        }

        /// <summary>
        /// Called to remeasure a control.
        /// </summary>
        /// <param name="constraint">The maximum size that the method can return.</param>
        /// <returns>
        /// The size of the control, up to the maximum specified by <paramref name="constraint"/>.
        /// </returns>
        protected override Size MeasureOverride(Size constraint)
        {
            if (Content != null)
            {
                Content.Measure(constraint);
            }
            return base.MeasureOverride(constraint);
        }

        /// <summary>
        /// Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control"/> object.
        /// </summary>
        /// <param name="arrangeBounds">The computed size that is used to arrange the content.</param>
        /// <returns>The size of the control.</returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (Content != null)
            {
                Content.Arrange(new Rect(new Point(0.00, 0.00), arrangeBounds));
            }
            return base.ArrangeOverride(arrangeBounds);
        }

        [DebuggerNonUserCode]
        private void ZonesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Ensure zone names are unique
            var names = Zones.GroupBy(x => x.ZonePlaceHolderName).Where(x => x.Count() > 1).Select(x => x.Key);
            if (names.Count() > 0)
            {
                throw new InvalidOperationException(string.Format("The following zone names are duplicated: {0}", string.Join(", ", names.ToArray())));
            }

            ReloadZones();
        }

        private void ReloadZones()
        {
            if (Content == null)
                return;

            var pairs = new List<KeyValuePair<ZonePlaceHolder, Zone>>();
            
            foreach (var zone in Zones)
            {
                var control = Content.FindName(zone.ZonePlaceHolderName);
                if (control == null)
                {
                    TraceSources.MagellanSource.TraceWarning("A ZonePlaceHolder by the name of '{0}' does not exist on the layout '{1}'.", zone.ZonePlaceHolderName, Content);
                    continue;
                }

                var zonePlaceHolder = control as ZonePlaceHolder;
                if (zonePlaceHolder == null)
                {
                    TraceSources.MagellanSource.TraceWarning("The control '{0}' in layout '{1}' is not a ZonePlaceHolder.", zone.ZonePlaceHolderName, Content);
                    continue;
                }

                pairs.Add(new KeyValuePair<ZonePlaceHolder, Zone>(zonePlaceHolder, zone));
            }

            NameScope.SetNameScope(Content, null);

            foreach (var pair in pairs)
            {
                pair.Key.Content = pair.Value.Content;
            }
        }

        /// <summary>
        /// Adds a child object.
        /// </summary>
        /// <param name="value">The child object to add.</param>
        public void AddChild(object value)
        {
            var zone = value as Zone;
            if (zone == null) throw new NotSupportedException("Only Zones can be added to a Layout element.");
            Zones.Add(zone);
        }

        /// <summary>
        /// Adds the text content of a node to the object.
        /// </summary>
        /// <param name="text">The text to add to the object.</param>
        public void AddText(string text)
        {
            throw new NotSupportedException("Only Zones can be added to a Layout element.");
        }
    }
}


