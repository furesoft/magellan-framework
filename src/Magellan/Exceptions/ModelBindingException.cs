using System.Text;
using Magellan.Framework;
using Magellan.Routing;

namespace Magellan.Exceptions
{
    /// <summary>
    /// An exception thrown when an error occurs trying to map navigation route parameters to methods on 
    /// controllers or view models.
    /// </summary>
    public class ModelBindingException : NavigationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingException"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="request">The request.</param>
        public ModelBindingException(ModelBindingContext context, ResolvedNavigationRequest request)
            : base(BuildMessage(context, request))
        {
        }

        private static string BuildMessage(ModelBindingContext context, ResolvedNavigationRequest request)
        {
            var message = new StringBuilder();
            message.AppendFormat(
                "The method '{0}' on target '{1}' requires a parameter named '{2}', which was not supplied.",
                context.TargetMethod.Name,
                context.TargetMethod.DeclaringType.FullName,
                context.TargetParameterName
                );

            message.AppendLine().AppendLine();

            message.Append("Candidate route parameters are:");
            foreach (var key in request.RouteValues.Keys)
            {
                message.AppendLine().Append(" - " + key);
            }
            return message.ToString();
        }
    }
}
