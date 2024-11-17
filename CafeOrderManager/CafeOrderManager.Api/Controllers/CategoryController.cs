using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Category;
using CafeOrderManager.Service.Category;

namespace CafeOrderManager.Api.Controllers
{
    public class CategoryController : BaseController<CategoryService, CategoryFilterDto, CategoryDto, CategoryDbo, CategoryListDto>
    {
        public CategoryController(CategoryService service) : base(service)
        {

        }


    }
}