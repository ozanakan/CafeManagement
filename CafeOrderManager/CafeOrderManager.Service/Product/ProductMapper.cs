using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Product;

namespace CafeOrderManager.Service.Product
{
    public class ProductMapper : BaseMapper<ProductDbo, ProductDto, ProductListDto>
    {
        public ProductMapper(IAuthService authService) : base(authService)
        {
        }

        public override ProductListDto ToListDto(ProductDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.ProductName = dbo.ProductName;
            dto.Price= dbo.Price;
                dto.StockQuantity= dbo.StockQuantity;
            dto.CategoryId= dbo.CategoryId;
            return dto;
        }

        public override ProductDto ToDto(ProductDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.ProductName = dbo.ProductName;
            dto.Price = dbo.Price;
            dto.StockQuantity = dbo.StockQuantity;
            dto.CategoryId = dbo.CategoryId;
            return dto;
        }


        public override ProductDbo ToCreate(ProductDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.ProductName = dto.ProductName;
            dbo.Price = dto.Price;
            dbo.StockQuantity = dto.StockQuantity;
            dbo.CategoryId = dto.CategoryId;
            return dbo;
        }

        public override ProductDbo ToUpdate(ProductDbo dbo, ProductDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.ProductName = dto.ProductName;
            dbo.Price = dto.Price;
            dbo.StockQuantity = dto.StockQuantity;
            dbo.CategoryId = dto.CategoryId;

            return dbo;
        }

        public override DropdownDto ToDropdown(ProductDbo dbo)
        {
            var dto = base.ToDropdown(dbo);
            dto.Name = dbo.ProductName;
            dto.Data = new
            {
                dbo.StockQuantity
            };
            return dto;
        }


    }
}
