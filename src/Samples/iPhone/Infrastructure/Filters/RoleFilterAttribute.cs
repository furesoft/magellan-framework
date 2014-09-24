using System;
using System.Threading;
using Magellan.Framework;

namespace iPhone.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RoleFilterAttribute : Attribute, IActionFilter
    {
        private readonly string roleName;
        private IMessageService messageService;

        public RoleFilterAttribute(string roleName)
        {
            this.roleName = roleName;
        }

        public string RoleName
        {
            get { return roleName; }
        }

        public IMessageService MessageService
        {
            get { return messageService = messageService ?? new MessageService(); }
            set { messageService = value; }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(RoleName))
            {
                MessageService.ShowInformation(string.Format("This action is only available to users in the {0} group. Try elevating before starting the application.", RoleName));
                context.OverrideResult = new DoNothingResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
