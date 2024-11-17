
namespace CafeOrderManager.Infrastructure.Models
{
    public class PaginationDto
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? TotalCount { get; set; }
        public int? PageCount { get; set; }
    }
}