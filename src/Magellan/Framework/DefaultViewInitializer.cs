using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using Magellan.Diagnostics;
using Magellan.Exceptions;
using Magellan.Routing;
using Magellan.Utilities;
using Magellan.Views;

namespace Magellan.Framework
{
    /// <summary>
    /// The default implementation of <see cref="IViewInitializer"/>, supporting WPF/Silverlight.
    /// </summary>
    public class DefaultViewInitializer : IViewInitializer
    {
        private readonly Func<IEnumerable<MethodInfo>, MethodInfo> initializeMethodSelector;
        private readonly ModelBinderDictionary modelBinders;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewInitializer"/> class.
        /// </summary>
        /// <param name="modelBinders">The model binders.</param>
        public DefaultViewInitializer(ModelBinderDictionary modelBinders)
            : this(DefaultMethodSelector, modelBinders)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewInitializer"/> class.
        /// </summary>
        /// <param name="initializeMethodSelector">A filter used to decide whether a method can be used to
        /// select the Initialize method on a ViewModel.</param>
        /// <param name="modelBinders">The model binders.</param>
        public DefaultViewInitializer(Func<IEnumerable<MethodInfo>, MethodInfo> initializeMethodSelector, ModelBinderDictionary modelBinders)
        {
            Guard.ArgumentNotNull(initializeMethodSelector, "initializeMethodSelector");
            
            this.initializeMethodSelector = initializeMethodSelector;
            this.modelBinders = modelBinders ?? new ModelBinderDictionary(new DefaultModelBinder());
        }

        /// <summary>
        /// Initializes the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// <param name="request">The request.</param>
        public virtual void Prepare(object view, object model, ResolvedNavigationRequest request)
        {
            CallInitializeMethods(model, request);
            AssignModelToView(view, model);
            AssignNavigator(view, request);
            AssignNavigator(model, request);
            WireUpLoadedEvent(model, view);
        }

        private object GetModelFromView(object view)
        {
            if (view is IModelBound)
            {
                return ((IModelBound)view).Model;
            }
            if (view is FrameworkElement)
            {
                return ((FrameworkElement)view).DataContext;
            }
            return null;
        }

        /// <summary>
        /// Notifies the view that it has been activated.
        /// </summary>
        /// <param name="view">The view.</param>
        public void NotifyActivated(object view)
        {
            NotifyActivatedInternal(view);
            NotifyActivatedInternal(GetModelFromView(view));
        }

        private static void NotifyActivatedInternal(object target)
        {
            var aware = target as IViewAware;
            if (aware == null)
                return;

            aware.Activated();
        }

        /// <summary>
        /// Notifies the view that it is being deactivated.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        public void NotifyDeactivating(object view, CancelEventArgs e)
        {
            NotifyDeactivatingInternal(e, view);
            NotifyDeactivatingInternal(e, GetModelFromView(view));
        }

        private static void NotifyDeactivatingInternal(CancelEventArgs e, object target)
        {
            if (e.Cancel)
                return;

            var aware = target as IViewAware;
            if (aware == null)
                return;

            aware.Deactivating(e);
        }

        /// <summary>
        /// Notifies the view that it has been deactivated.
        /// </summary>
        /// <param name="view">The view.</param>
        public void NotifyDeactivated(object view)
        {
            NotifyDeactivatedInternal(view);
            NotifyDeactivatedInternal(GetModelFromView(view));
        }

        private static void NotifyDeactivatedInternal(object target)
        {
            var aware = target as IViewAware;
            if (aware == null)
                return;

            aware.Deactivated();
        }

        /// <summary>
        /// Discovers and invokes any Initialize methods according to the initializeMethodSelector passed to 
        /// the constructor.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="request">The request.</param>
        protected virtual void CallInitializeMethods(object model, ResolvedNavigationRequest request)
        {
            if (model == null || initializeMethodSelector == null)
                return;

            var initializers = model.GetType().GetMethods() as IEnumerable<MethodInfo>;
            var initializer = initializeMethodSelector(initializers);
            if (initializer == null)
            {
                return;
            }

            var arguments = new List<object>();
            foreach (var parameterInfo in initializer.GetParameters())
            {
                var bindingContext = new ModelBindingContext(parameterInfo.Name, initializer, parameterInfo.ParameterType, request.RouteValues);
                var binder = modelBinders.GetBinder(parameterInfo.ParameterType);
                var argument = binder.BindModel(request, bindingContext);
                arguments.Add(argument);
            }

            initializer.Invoke(model, arguments.ToArray());
        }

        private static MethodInfo DefaultMethodSelector(IEnumerable<MethodInfo> methods)
        {
            return methods
                .Where(x => x.Name == "Initialize")
                .Where(x => x.IsPublic)
                .OrderByDescending(x => x.GetParameters().Length)
                .FirstOrDefault();
        }

        /// <summary>
        /// If the target implements <see cref="INavigationAware"/>, assigns the Navigator to it, and sets 
        /// the NavigationProperties.Navigator attached property.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="request">The request.</param>
        protected virtual void AssignNavigator(object target, ResolvedNavigationRequest request)
        {
            if (target == null)
                return;

            var navigator = request.Navigator;
            if (navigator == null) 
                return;
            
            var navigationAware = target as INavigationAware;
            var dependencyObject = target as DependencyObject;

            if (navigationAware != null)
            {
                TraceSources.MagellanSource.TraceVerbose("The object '{0}' implements the INavigationAware interface, so it is being provided with a navigator.", navigationAware.GetType().Name);
                navigationAware.Navigator = navigator;
            }

            if (dependencyObject != null)
            {
                NavigationProperties.SetNavigator(dependencyObject, navigator);
                NavigationProperties.SetCurrentRequest(dependencyObject, request);
            }
        }

        /// <summary>
        /// Assigns the model to view the view, setting it as the DataContext or using the 
        /// <see cref="IModelBound"/> interface.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        protected virtual void AssignModelToView(object view, object model)
        {
            // Connect the model to the view
            if (view is IModelBound)
            {
                TraceSources.MagellanSource.TraceVerbose("The view '{0}' implements the IView interface, so the model is being set as the Model on the IView.", view.GetType().Name);
                ((IModelBound)view).Model = model;
            }
            else if (view is FrameworkElement)
            {
                TraceSources.MagellanSource.TraceVerbose("The view '{0}' does not implement the IView interface, so the model is being set as the DataContext.", view.GetType().Name);
                ((FrameworkElement)view).DataContext = model;
            }
            else
            {
                throw new ViewNotSupportedException("This view does not implement the IModelBound interface, and it is not a framework element. The default ViewInitializer does not know how to assign the model to this view. Please override the ViewInitializer in your ViewEngineResult to one that knows how to handle this type of view.");
            }
        }

        /// <summary>
        /// Wires up an event handler for the Loaded event, calling the model's Loaded notification method
        /// if supported.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="view">The view.</param>
        protected virtual void WireUpLoadedEvent(object model, object view)
        {
            var viewAware = model as IViewAware;
            var element = view as FrameworkElement;
            if (viewAware == null || element == null)
            {
                return;
            }

            viewAware.ViewAttached(view);
        }


    }
}
