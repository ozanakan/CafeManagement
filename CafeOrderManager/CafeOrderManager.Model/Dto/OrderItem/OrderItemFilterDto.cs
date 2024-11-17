using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Model.Dbo;

namespace CafeOrderManager.Model.Dto.OrderItem
{
    public class OrderItemFilterDto : BaseFilterDto<OrderItemDbo>
    {
       public int OrderId { get; set; }
    }
}
