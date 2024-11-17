using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.OrderItem;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CafeOrderManager.Service.OrderItem
{

    public class OrderItemService : BaseService<OrderItemRepository, OrderItemMapper, OrderItemDbo, OrderItemDto, OrderItemListDto, OrderItemFilterDto>
    {
        public OrderItemService(IAuthService authService, OrderItemRepository repository, OrderItemMapper mapper) : base(repository, mapper, authService)
        {

        }

        public async Task AddOrderItemsAsync(IEnumerable<OrderItemDto> orderItems, int orderId)
        {
            foreach (var orderItemDto in orderItems)
            {
                orderItemDto.OrderId = orderId;
                var orderItemDbo = _mapper.ToCreate(orderItemDto);
                await _repository.Create(orderItemDbo);
            }
        }

    }
}