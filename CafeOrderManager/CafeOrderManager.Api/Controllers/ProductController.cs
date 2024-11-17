using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Product;
using CafeOrderManager.Service.Product;

namespace CafeOrderManager.Api.Controllers
{
    public class ProductController : BaseController<ProductService, ProductFilterDto, ProductDto, ProductDbo, ProductListDto>
    {
        public ProductController(ProductService service) : base(service)
        {

        }


    }
}