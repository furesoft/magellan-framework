using System;
using System.ComponentModel;
using System.Linq;

namespace Magellan.Framework
{
    /// <summary>
    /// Serves as a base class for the "third object" in seperated presentation patterns - view models, 
    /// presenters, and so on.  
    /// </summary>
    public abstract class PresentationObject : INotifyPropertyChanged, IDataErrorInfo, IStoreValidationMessages
    {
        private readonly BusyState busyState = new BusyState();
        private readonly ValidationMessageDictionary validationMessages = new ValidationMessageDictionary();

        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationObject"/> class.
        /// </summary>
        protected PresentationObject()
        {
            busyState.PropertyChanged += (x, y) => NotifyChanged("IsBusy");
            validationMessages.ErrorsChanged += (x, y) => NotifyChanged("");
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the <see cref="BusyState"/> that tracks whether background operations are running within 
        /// this presentation object.
        /// </summary>
        /// <value>The state of the busy.</value>
        public BusyState BusyState
        {
            get { return busyState; }
        }

        /// <summary>
        /// Gets a value indicating whether any background operations are in progress.
        /// </summary>
        /// <value><c>true</c> if this presentation object is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get { return BusyState.IsBusy; }
        }

        /// <summary>
        /// Raises a property changed event for the given property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void NotifyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises a property changed events for a given set of properties.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="otherProperties">The other properties.</param>
        protected void NotifyChanged(string propertyName, params string[] otherProperties)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            foreach (var other in otherProperties)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(other));    
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Gets the validation messages associated with this object.
        /// </summary>
        /// <value>The validation messages.</value>
        public virtual ValidationMessageDictionary ValidationMessages
        {
            get { return validationMessages; }
        }

        /// <summary>
        /// Gets a string containing the validation messages for a particular key (usually a property name), with each message seperated
        /// by a newline.
        /// </summary>
        /// <value></value>
        public virtual string this[string columnName]
        {
            get { return string.Join(Environment.NewLine, ValidationMessages[columnName].ToArray()); }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value></value>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public virtual string Error
        {
            get { return string.Join(Environment.NewLine, ValidationMessages.AllMessages()); }
        }
    }
}