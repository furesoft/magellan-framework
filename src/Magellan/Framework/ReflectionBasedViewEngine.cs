using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Magellan.Diagnostics;

namespace Magellan.Framework
{
    /// <summary>
    /// A base class for view engines that use reflection to locate the view based on namespace and type 
    /// name conventions.
    /// </summary>
    public abstract class ReflectionBasedViewEngine : IViewEngine
    {
        private readonly List<Assembly> additionalViewAssemblies = new List<Assembly>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionBasedViewEngine"/> class.
        /// </summary>
        protected ReflectionBasedViewEngine(params Assembly[] additionalViewAssemblies)
        {
            NamespaceConvention = new DefaultViewNamespaceConvention();
            NamingConvention = new DefaultViewNamingConvention();

            if (additionalViewAssemblies == null) return;
            foreach (var assembly in additionalViewAssemblies)
            {
                AdditionalViewAssemblies.Add(assembly);
            }
        }

        /// <summary>
        /// Gets the a collection of additional assemblies that will be used to discover a view. 
        /// </summary>
        /// <value>The additional view assemblies.</value>
        public IList<Assembly> AdditionalViewAssemblies
        {
            get { return additionalViewAssemblies; }
        }

        /// <summary>
        /// Gets or sets the view namespace provider. This class is used to generate the combinations of view 
        /// locations and to encapsulate conventions surrounding the project structure.
        /// </summary>
        /// <value>The view namespace provider.</value>
        public IViewNamespaceConvention NamespaceConvention { get; set; }

        /// <summary>
        /// Gets or sets the view namespace provider. This class is used to generate the combinations of view 
        /// locations and to encapsulate conventions surrounding the project structure.
        /// </summary>
        /// <value>The view namespace provider.</value>
        public IViewNamingConvention NamingConvention { get; set; }

        /// <summary>
        /// When implemented in a derived class, indicated whether this view engine should even attempt to 
        /// locate views and handle the request. If this method returns false, no reflection will be 
        /// performed.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <returns></returns>
        protected virtual bool ShouldHandle(ControllerContext controllerContext, ViewResultOptions options, string viewName)
        {
            return true;
        }

        /// <summary>
        /// When implemented in a derived class, allows the derived class to restrict the criteria used to select potential types.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="candidates">The list of candidate types.</param>
        /// <returns></returns>
        protected abstract IEnumerable<Type> FilterCandidateTypes(ControllerContext controllerContext, ViewResultOptions options, string viewName, IEnumerable<Type> candidates);

        /// <summary>
        /// Creates the view result for the specified view type.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected abstract ViewEngineResult CreateViewResult(ControllerContext controllerContext, ViewResultOptions options, Type type);

        /// <summary>
        /// Attempts to find the view, or returns information about the locations that were searched.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="options">The view parameters.</param>
        /// <param name="view">The view.</param>
        /// <returns>
        /// A <see cref="ViewEngineResult"/> containing the resolved view or information about the locations that were searched.
        /// </returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, ViewResultOptions options, string view)
        {
            if (!ShouldHandle(controllerContext, options, view))
            {
                return new ViewEngineResult(false, new string[0]);
            }

            var possibleViewNames = NamingConvention.GetAlternativeNames(controllerContext, view);
            var allTypes = FindTypesNamed(controllerContext, possibleViewNames);
            var candidateTypes = FilterCandidateTypes(controllerContext, options, view, allTypes);

            var namespaces = CreateShortlistOfNamespacesInWhichToSearch(controllerContext, view);

            var match = namespaces.Select(ns => FindFirstTypeInNamespace(candidateTypes, ns)).Where(x => x != null).FirstOrDefault();
            if (match != null)
            {
                return CreateViewResult(controllerContext, options, match);
            }

            // Make a list of all search examples used above to create a friendly error message
            var searchAttempts = namespaces.SelectMany(ns => 
                possibleViewNames.Select(name => string.IsNullOrEmpty(ns) 
                    ? string.Format("{0}", name) 
                    : string.Format("{0}.{1}", ns, name)
                    ));

            TraceSources.MagellanSource.TraceInformation("The {0} could not find the view '{1}'. The following locations were searched: \r\n{2}", GetType().FullName, view, string.Join("\r\n- ", searchAttempts.ToArray()));
            return new ViewEngineResult(false, searchAttempts);
        }

        private IEnumerable<string> CreateShortlistOfNamespacesInWhichToSearch(ControllerContext controllerContext, string view)
        {
            var current = controllerContext.Controller.GetType().Namespace;
            while (current != null && current.Trim().Length > 0)
            {
                foreach (var alternative in NamespaceConvention.GetNamespaces(controllerContext, current, view))
                    yield return alternative;

                var lastDot = current.LastIndexOf('.');
                if (lastDot < 0) break;
                current = current.Substring(0, lastDot);
            }
            yield return null;
        }

        private IEnumerable<Type> FindTypesNamed(ControllerContext controllerContext, IEnumerable<string> names)
        {
            var types = GetAllTypesFromAllAssemblies(controllerContext);
            return names.SelectMany(name => types.Where(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)));
        }

        private IEnumerable<Type> GetAllTypesFromAllAssemblies(ControllerContext controllerContext)
        {
            var assembly = controllerContext.Controller.GetType().Assembly;
            return assembly.GetTypes().Union(
                additionalViewAssemblies.SelectMany(x => x.GetTypes())
                );
        }

        private static Type FindFirstTypeInNamespace(IEnumerable<Type> types, string namespaceName)
        {
            return types.FirstOrDefault(x => string.Equals(x.Namespace, namespaceName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
