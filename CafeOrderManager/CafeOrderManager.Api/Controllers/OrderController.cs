using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Order;
using CafeOrderManager.Service.Order;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CafeOrderManager.Api.Controllers
{
    public class OrderController : BaseController<OrderService, OrderFilterDto, OrderDto, OrderDbo, OrderListDto>
    {
        public OrderController(OrderService service) : base(service)
        {

        }

        [AllowAnonymous]
        [HttpPost("OrderStatusUpdate")]
        public async Task<IActionResult> OrderStatusUpdate([FromBody] OrderDto model)
        {
            var result = await _service.OrderStatusUpdate(model);
            return GetResult(result);
        }


    }
}