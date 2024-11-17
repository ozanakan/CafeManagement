using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Product;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
namespace CafeOrderManager.Service.Product
{

    public class ProductService : BaseService<ProductRepository, ProductMapper, ProductDbo, ProductDto, ProductListDto, ProductFilterDto>
    {
        public ProductService(IAuthService authService, ProductRepository repository, ProductMapper mapper) : base(repository, mapper, authService)
        {

        }

    }
}