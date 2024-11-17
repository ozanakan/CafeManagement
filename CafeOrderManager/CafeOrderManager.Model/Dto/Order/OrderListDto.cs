using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Model.Dto.OrderItem;
using System.Collections.Generic;

namespace CafeOrderManager.Model.Dto.Order
{
    public class OrderListDto : BaseListDto
    {
        public string OrderNumber { get; set; }
        public int TableId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }

        
        public List<OrderItemDto> OrderItemList { get; set; }

    }
}
