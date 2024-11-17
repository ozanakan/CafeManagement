using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.OrderItem;
using System.Collections.Generic;
using System.Linq;

namespace CafeOrderManager.Service.OrderItem
{
    public class OrderItemMapper : BaseMapper<OrderItemDbo, OrderItemDto, OrderItemListDto>
    {
        public OrderItemMapper(IAuthService authService) : base(authService)
        {
        }

        public override OrderItemListDto ToListDto(OrderItemDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.OrderId = dbo.OrderId;
            dto.ProductId = dbo.ProductId;
            dto.ProductName = dbo.Product.ProductName;
            dto.Quantity= dbo.Quantity;
           

            return dto;
        }
        public List<OrderItemDto> ToListDto(IEnumerable<OrderItemDbo> dbos) // Doğru türde dönüşüm
        {
            return dbos.Select(ToDto).ToList();
        }
        public override OrderItemDto ToDto(OrderItemDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.OrderId = dbo.OrderId;
            dto.ProductId = dbo.ProductId;
            dto.ProductName = dbo.Product?.ProductName;
            dto.ProductPrice = dbo.Product.Price;
            dto.CategoryId = dbo.Product.CategoryId;
            dto.Quantity = dbo.Quantity;
            return dto;
        }


        public override OrderItemDbo ToCreate(OrderItemDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.OrderId = dto.OrderId;
            dbo.ProductId = dto.ProductId;
            dbo.Quantity = dto.Quantity;
            return dbo;
        }

        public override OrderItemDbo ToUpdate(OrderItemDbo dbo, OrderItemDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.OrderId = dto.OrderId;
            dbo.ProductId = dto.ProductId;
            dbo.Quantity = dto.Quantity;

            return dbo;
        }
      

    }
}
