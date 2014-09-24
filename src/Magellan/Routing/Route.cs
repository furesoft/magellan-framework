using System;
using System.Linq;
using Magellan.Exceptions;
using Magellan.Utilities;

namespace Magellan.Routing
{
    /// <summary>
    /// The default implementation of <see cref="IRoute"/>. Most of the details of this class are delegated 
    /// to types such as <see cref="IRouteParser"/> (the default being <see cref="RouteParser"/>) and 
    /// <see cref="IRouteValidator"/> (defaulting to <see cref="RouteValidator"/>).
    /// </summary>
    public class Route : IRoute
    {
        private readonly string pathSpecification;
        private readonly Func<IRouteHandler> routeHandler;
        private readonly IRouteValidator validator;
        private readonly IRouteParser parser;
        private readonly RouteValueDictionary defaults;
        private readonly RouteValueDictionary constraints;
        private ParsedRoute parsedRoute;

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routeSpecification">The path specification, which gives the pattern that paths will 
        /// follow. For example, "/patients/{action}".</param>
        /// <param name="routeHandler">A delegate that will produce a route handler when needed for this 
        /// route.</param>
        /// <param name="defaults">The default values of this route.</param>
        /// <param name="constraints">The constraints that will apply to this route.</param>
        public Route(string routeSpecification, Func<IRouteHandler> routeHandler, RouteValueDictionary defaults, RouteValueDictionary constraints) 
            : this(routeSpecification, routeHandler, defaults, constraints, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routeSpecification">The path specification, which gives the pattern that paths will
        /// follow. For example, "/patients/{action}".</param>
        /// <param name="routeHandler">A delegate that will produce a route handler when needed for this
        /// route.</param>
        /// <param name="defaults">The default values of this route.</param>
        /// <param name="constraints">The constraints that will apply to this route.</param>
        /// <param name="validator">An object charged with validating the route configuration, ensuring the 
        /// route doesn't violate any expectations around how a route can be declared.</param>
        public Route(string routeSpecification, Func<IRouteHandler> routeHandler, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteValidator validator)
            : this(routeSpecification, routeHandler, defaults, constraints, null, validator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routeSpecification">The path specification, which gives the pattern that paths will
        /// follow. For example, "/patients/{action}".</param>
        /// <param name="routeHandler">A delegate that will produce a route handler when needed for this
        /// route.</param>
        /// <param name="defaults">The default values of this route.</param>
        /// <param name="constraints">The constraints that will apply to this route.</param>
        /// <param name="parser">An object charged with parsing the route specification, producing a
        /// <see cref="ParsedRoute"/>.</param>
        public Route(string routeSpecification, Func<IRouteHandler> routeHandler, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteParser parser)
            : this(routeSpecification, routeHandler, defaults, constraints, parser, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        /// <param name="routeSpecification">The path specification, which gives the pattern that paths will
        /// follow. For example, "/patients/{action}".</param>
        /// <param name="routeHandler">A delegate that will produce a route handler when needed for this
        /// route.</param>
        /// <param name="defaults">The default values of this route.</param>
        /// <param name="constraints">The constraints that will apply to this route.</param>
        /// <param name="parser">An object charged with parsing the route specification, producing a
        /// <see cref="ParsedRoute"/>.</param>
        /// <param name="validator">An object charged with validating the route configuration, ensuring the
        /// route doesn't violate any expectations around how a route can be declared.</param>
        public Route(string routeSpecification, Func<IRouteHandler> routeHandler, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteParser parser, IRouteValidator validator)
        {
            Guard.ArgumentNotNull(routeHandler, "routeHandler");

            pathSpecification = routeSpecification ?? string.Empty;
            this.routeHandler = routeHandler;

            this.validator = validator ?? new RouteValidator();
            this.defaults = defaults ?? new RouteValueDictionary();
            this.constraints = constraints ?? new RouteValueDictionary();
            
            this.parser = parser ?? new RouteParser(
                new ParameterSegmentRecognizer(),
                new LiteralSegmentRecognizer(),
                new CatchAllParameterSegmentRecognizer()
                );
        }

        /// <summary>
        /// Gets the specification that was used to set the format of paths to this route, such as 
        /// "/patients/{action}".
        /// </summary>
        /// <value>The specification.</value>
        public string Specification
        {
            get { return pathSpecification; }
        }

        /// <summary>
        /// Gets the route validator, which will ensure any expectations about the format and configuration
        /// of this route are not violated.
        /// </summary>
        /// <value>The validator.</value>
        public IRouteValidator Validator
        {
            get { return validator; }
        }

        /// <summary>
        /// Gets the parser, which will parse the path specification for this route to produce the main 
        /// object for matching paths and routes.
        /// </summary>
        /// <value>The parser.</value>
        public IRouteParser Parser
        {
            get { return parser; }
        }

        /// <summary>
        /// Gives the route a chance to inialize itself. This method is called when the route is registered
        /// in a route collection. It should be used for any pre-compilation, caching or validation tasks.
        /// </summary>
        public void Validate()
        {
            if (parsedRoute == null)
            {
                parsedRoute = parser.Parse(this, pathSpecification, defaults, constraints);
                var result = validator.Validate(parsedRoute);
                if (!result.Success)
                {
                    throw new InvalidRouteException(
                        this,
                        result,
                        string.Format("The route with specification '{0}' is invalid: {1}", pathSpecification, string.Join("", result.Errors.Select(x => Environment.NewLine + " - " + x).ToArray()))
                        );
                }
            }
        }

        /// <summary>
        /// Selects or creates an appropriate route handler for this route.
        /// </summary>
        /// <returns>
        /// An <see cref="IRouteHandler"/> that will execute a navigation request to this route.
        /// </returns>
        public IRouteHandler CreateRouteHandler()
        {
            return routeHandler();
        }

        /// <summary>
        /// Matches a path, such as "/patients/{action}", to this route, indicating whether or not the route
        /// successfully matches the path.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// An object indicating the result of the path match.
        /// </returns>
        public RouteMatch MatchPathToRoute(string request)
        {
            Validate();
            var result = parsedRoute.MatchPathToRoute(this, request);

            foreach (var defaultValue in defaults)
            {
                if (!result.Values.ContainsKey(defaultValue.Key))
                {
                    result.Values.Add(defaultValue.Key, defaultValue.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Matches a set of route data to this route, producing the probable path that should be used if
        /// this route was linked to.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>
        /// An object indicating the result of the route match.
        /// </returns>
        public PathMatch MatchRouteToPath(RouteValueDictionary values)
        {
            Validate();
            return parsedRoute.MatchRouteToPath(this, values);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Specification;
        }
    }
}