using System;
using System.Threading;
using Magellan.Framework;

namespace iPhone.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SleepAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Thread.Sleep(1000);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
