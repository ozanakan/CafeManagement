namespace CafeOrderManager.Infrastructure.Exceptions
{
    public class CustomException : BaseException
    {
        public CustomException() : base($"exception.custom_error")
        {
        }

        public CustomException(string message) : base(message)
        {
        }
    }
}