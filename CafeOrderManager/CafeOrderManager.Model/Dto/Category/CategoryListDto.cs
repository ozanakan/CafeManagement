using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Model.Dto.Category
{
    public class CategoryListDto : BaseListDto
    {
        public string CategoryName { get; set; }

        //public ProductListDto Product { get; set; }
    }
}
