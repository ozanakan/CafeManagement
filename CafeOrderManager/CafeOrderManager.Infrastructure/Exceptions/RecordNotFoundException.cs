namespace CafeOrderManager.Infrastructure.Exceptions
{
    public class RecordNotFoundException : BaseException
    {
        public RecordNotFoundException() : base($"exception.null_record")
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }
    }
}