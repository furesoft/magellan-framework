using System;
using Magellan.Framework;

namespace iPhone.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NotImplementedYetAttribute : Attribute, IActionFilter
    {
        private readonly string message;
        private IMessageService messageService;

        public NotImplementedYetAttribute() : this(null)
        {
        }

        public NotImplementedYetAttribute(string message)
        {
            this.message = message;
            if (string.IsNullOrEmpty(this.message))
            {
                this.message = "This feature has not been implemented (this is just a demo after all).";
            }
        }

        public IMessageService MessageService
        {
            get { return messageService = messageService ?? new MessageService(); }
            set { messageService = value; }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            MessageService.ShowInformation(message);
            context.OverrideResult = new DoNothingResult();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
