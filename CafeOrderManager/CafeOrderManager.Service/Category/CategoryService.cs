using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Category;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
namespace CafeOrderManager.Service.Category
{

    public class CategoryService : BaseService<CategoryRepository, CategoryMapper, CategoryDbo, CategoryDto, CategoryListDto, CategoryFilterDto>
    {
        public CategoryService(IAuthService authService, CategoryRepository repository, CategoryMapper mapper) : base(repository, mapper, authService)
        {

        }

    }
}