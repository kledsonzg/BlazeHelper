namespace BlazeHelper.Exceptions
{
    public class NotSupportedBetStatusException : Exception
    {
        public NotSupportedBetStatusException(string message, Exception inner) : base(message, inner) {}
        public NotSupportedBetStatusException(string message) : base(message) {}
    }
}