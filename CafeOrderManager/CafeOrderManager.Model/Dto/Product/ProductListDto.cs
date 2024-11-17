using CafeOrderManager.Infrastructure.Bases;

namespace CafeOrderManager.Model.Dto.Product
{
    public class ProductListDto : BaseListDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int? StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
}
