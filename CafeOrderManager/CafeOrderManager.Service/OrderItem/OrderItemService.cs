using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.OrderItem;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;

namespace CafeOrderManager.Service.OrderItem
{

    public class OrderItemService : BaseService<OrderItemRepository, OrderItemMapper, OrderItemDbo, OrderItemDto, OrderItemListDto, OrderItemFilterDto>
    {
        public OrderItemService(IAuthService authService, OrderItemRepository repository, OrderItemMapper mapper) : base(repository, mapper, authService)
        {

        }

    }
}