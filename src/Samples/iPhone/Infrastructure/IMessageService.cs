namespace iPhone.Infrastructure
{
    public interface IMessageService
    {
        void ShowInformation(string messageFormat, params object[] args);
    }
}
