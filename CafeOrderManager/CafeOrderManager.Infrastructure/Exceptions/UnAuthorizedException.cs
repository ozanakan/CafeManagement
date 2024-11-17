namespace CafeOrderManager.Infrastructure.Exceptions
{
    public class UnAuthorizedException : BaseException
    {
        public UnAuthorizedException() : base($"exception.unauthorized")
        {
        }

        public UnAuthorizedException(string message) : base(message)
        {
        }
    }
}