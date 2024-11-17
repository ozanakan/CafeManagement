using CafeOrderManager.Infrastructure.Bases;

namespace CafeOrderManager.Model.Dto.OrderItem
{
    public class OrderItemDto : BaseDto
    {

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }

    }
}