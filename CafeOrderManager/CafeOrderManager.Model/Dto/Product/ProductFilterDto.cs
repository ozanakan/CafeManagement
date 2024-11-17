using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Model.Dbo;

namespace CafeOrderManager.Model.Dto.Product
{
    public class ProductFilterDto : BaseFilterDto<ProductDbo>
    {
        public int? CategoryId { get; set; }
    }
}
