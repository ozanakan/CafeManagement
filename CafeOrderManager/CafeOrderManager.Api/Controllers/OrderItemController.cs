using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.OrderItem;
using CafeOrderManager.Service.OrderItem;

namespace CafeOrderManager.Api.Controllers
{
    public class OrderItemController : BaseController<OrderItemService, OrderItemFilterDto, OrderItemDto, OrderItemDbo, OrderItemListDto>
    {
        public OrderItemController(OrderItemService service) : base(service)
        {

        }


    }
}