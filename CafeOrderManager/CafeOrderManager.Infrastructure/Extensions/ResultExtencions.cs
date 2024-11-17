using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Extensions
{
    public static class ResultExtensions
    {
        public static bool EnsureSucces(this Result result)
        {
            return result?.Status == HttpStatus.Success;
        }
    }
}