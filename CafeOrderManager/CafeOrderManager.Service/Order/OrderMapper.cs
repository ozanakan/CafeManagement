using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Order;
using CafeOrderManager.Service.OrderItem;

namespace CafeOrderManager.Service.Order
{
    public class OrderMapper : BaseMapper<OrderDbo, OrderDto, OrderListDto>
    {
        private readonly OrderItemMapper _orderItemMapper; 
        public OrderMapper(IAuthService authService,OrderItemMapper orderItemMapper) : base(authService)
        {
            _orderItemMapper=orderItemMapper;
        }

        public override OrderListDto ToListDto(OrderDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.OrderNumber = dbo.OrderNumber;
            dto.OrderStatus = dbo.OrderStatus;
            dto.TableId = dbo.TableId;
            //dto.OrderItemList = _orderItemMapper.ToListDto(dbo.OrderItems);
            dto.OrderItemList = _orderItemMapper.ToListDto(dbo.OrderItems); // Koleksiyon dönüşümü
            return dto;
        }

        public override OrderDto ToDto(OrderDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.OrderNumber = dbo.OrderNumber;
            dto.OrderStatus = dbo.OrderStatus;
            dto.TableId = dbo.TableId;
            return dto;
        }


        public override OrderDbo ToCreate(OrderDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.OrderNumber = dto.OrderNumber;
            dbo.OrderStatus = dto.OrderStatus;
            dbo.TableId = dto.TableId;
            return dbo;
        }

        public override OrderDbo ToUpdate(OrderDbo dbo, OrderDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.OrderNumber = dto.OrderNumber;
            dbo.OrderStatus = dto.OrderStatus;
            dbo.TableId = dto.TableId;
            return dbo;
        }

    }
}
