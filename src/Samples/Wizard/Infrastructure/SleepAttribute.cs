using System;
using System.Threading;
using Magellan.Framework;

namespace Wizard.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SleepAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Thread.Sleep(200);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}


