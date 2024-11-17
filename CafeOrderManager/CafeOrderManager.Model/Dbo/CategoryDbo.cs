using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeOrderManager.Model.Dbo
{
    [Table("Category")]
    public class CategoryDbo : BaseDbo
    {
        public CategoryDbo()
        {
        }

        [Log(DataTypeEnum.String)]
        public string CategoryName { get; set; }

        public virtual IEnumerable<ProductDbo> Products { get; set; }

    }
}
