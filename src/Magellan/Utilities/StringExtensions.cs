namespace Magellan.Utilities
{
    internal static class StringExtensions
    {
        public static string CleanErrorMessage(this string message)
        {
            message = message.Trim();
            message = message.TrimEnd('.');
            message = message + ".";
            return message;
        }
    }
}
