using System.Windows;

namespace iPhone.Infrastructure
{
    public class MessageService : IMessageService
    {
        public void ShowInformation(string messageFormat, params object[] args)
        {
            var message = string.Format(messageFormat, args);
            MessageBox.Show(message, "iPhone", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
