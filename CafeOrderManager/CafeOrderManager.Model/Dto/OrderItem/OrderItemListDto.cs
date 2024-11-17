using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Model.Dto.OrderItem
{
    public class OrderItemListDto : BaseListDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }

    }
}
