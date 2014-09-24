using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Magellan.Controls
{
    /// <summary>
    /// Represents a zone used in a <see cref="Layout"/>.
    /// </summary>
    [ContentProperty("Content")]
    public class Zone : Freezable
    {
        /// <summary>
        /// Dependency property for the ZoneName property.
        /// </summary>
        public static readonly DependencyProperty ZonePlaceHolderNameProperty = DependencyProperty.Register("ZonePlaceHolderName", typeof(string), typeof(Zone), new UIPropertyMetadata(string.Empty, ZoneNameSet));

        /// <summary>
        /// Dependency property for the Content property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(Zone), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="Zone"/> class.
        /// </summary>
        public Zone()
        {
        }

        /// <summary>
        /// Gets or sets the content that will be placed in the zone.
        /// </summary>
        /// <value>The content.</value>
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the zone.
        /// </summary>
        /// <value>The name of the zone.</value>
        public string ZonePlaceHolderName
        {
            get { return (string)GetValue(ZonePlaceHolderNameProperty); }
            set { SetValue(ZonePlaceHolderNameProperty, value); }
        }

        /// <summary>
        /// Makes the <see cref="T:System.Windows.Freezable"/> object unmodifiable or tests whether it can be made unmodifiable.
        /// </summary>
        /// <param name="isChecking">true to return an indication of whether the object can be frozen (without actually freezing it); false to actually freeze the object.</param>
        /// <returns>
        /// If <paramref name="isChecking"/> is true, this method returns true if the <see cref="T:System.Windows.Freezable"/> can be made unmodifiable, or false if it cannot be made unmodifiable. If <paramref name="isChecking"/> is false, this method returns true if the if the specified <see cref="T:System.Windows.Freezable"/> is now unmodifiable, or false if it cannot be made unmodifiable.
        /// </returns>
        [DebuggerNonUserCode]
        protected override bool FreezeCore(bool isChecking)
        {
            if (string.IsNullOrEmpty(ZonePlaceHolderName))
            {
                throw new InvalidOperationException("The ZonePlaceHolderName property must be set.");
            }
            return base.FreezeCore(isChecking);
        }

        [DebuggerNonUserCode]
        private static void ZoneNameSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.OldValue))
            {
                throw new InvalidOperationException("The ZonePlaceHolderName has already been set. Once set, it cannot be changed.");
            }
        }

        /// <summary>
        /// When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable"/> derived class.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new Zone();
        }
    }
}