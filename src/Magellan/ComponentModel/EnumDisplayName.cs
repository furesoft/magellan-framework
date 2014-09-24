using System;

namespace Magellan.ComponentModel
{
    /// <summary>
    /// An attribute that can be applied to members of an enum to indicate the display name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : Attribute
    {
        private readonly string displayName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public EnumDisplayNameAttribute(string displayName)
        {
            this.displayName = displayName;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return displayName; }
        }
    }
}
