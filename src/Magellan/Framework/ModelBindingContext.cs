using System;
using System.Reflection;
using Magellan.Routing;

namespace Magellan.Framework
{
    /// <summary>
    /// Provides information to <see cref="IModelBinder">model binders</see> about the target action parameter and the values available.
    /// </summary>
    public class ModelBindingContext
    {
        private readonly string targetParameterName;
        private readonly MethodInfo targetMethod;
        private readonly Type targetParameterType;
        private readonly RouteValueDictionary potentialValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingContext"/> class.
        /// </summary>
        /// <param name="targetParameterName">Name of the target parameter.</param>
        /// <param name="targetMethod">The target method.</param>
        /// <param name="targetParameterType">Type of the target parameter.</param>
        /// <param name="potentialValues">The potential values.</param>
        public ModelBindingContext(string targetParameterName, MethodInfo targetMethod, Type targetParameterType, RouteValueDictionary potentialValues)
        {
            this.targetParameterName = targetParameterName;
            this.targetMethod = targetMethod;
            this.targetParameterType = targetParameterType;
            this.potentialValues = potentialValues;
        }

        /// <summary>
        /// Gets the method that the model binder is mapping to.
        /// </summary>
        /// <value>The target method.</value>
        public MethodInfo TargetMethod
        {
            get { return targetMethod; }
        }

        /// <summary>
        /// Gets the name of the parameter on the action (typically the method argument).
        /// </summary>
        /// <value>The name of the target parameter.</value>
        public string TargetParameterName
        {
            get { return targetParameterName; }
        }

        /// <summary>
        /// Gets the type of the parameter on the action (typically the method argument type).
        /// </summary>
        /// <value>The type of the target parameter.</value>
        public Type TargetParameterType
        {
            get { return targetParameterType; }
        }

        /// <summary>
        /// Gets the navigation request values that are available for mapping.
        /// </summary>
        /// <value>The potential values.</value>
        public RouteValueDictionary RouteValues
        {
            get { return potentialValues; }
        }
    }
}