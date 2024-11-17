using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Category;

namespace CafeOrderManager.Service.Category
{
    public class CategoryMapper : BaseMapper<CategoryDbo, CategoryDto, CategoryListDto>
    {
        public CategoryMapper(IAuthService authService) : base(authService)
        {
        }

        public override CategoryListDto ToListDto(CategoryDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.CategoryName = dbo.CategoryName;
            return dto;
        }

        public override CategoryDto ToDto(CategoryDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.CategoryName = dbo.CategoryName;
            return dto;
        }


        public override CategoryDbo ToCreate(CategoryDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.CategoryName = dto.CategoryName;
            return dbo;
        }

        public override CategoryDbo ToUpdate(CategoryDbo dbo, CategoryDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.CategoryName = dto.CategoryName;
            return dbo;
        }

        public override DropdownDto ToDropdown(CategoryDbo dbo)
        {
            var dto = base.ToDropdown(dbo);
            dto.Name = dbo.CategoryName;
            return dto;
        }


    }
}
