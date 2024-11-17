using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeOrderManager.Model.Dbo
{
    [Table("Product")]
    public class ProductDbo : BaseDbo
    {
        public ProductDbo()
        {
        }

        [Log(DataTypeEnum.String)]
        public string ProductName { get; set; }
        
        [Log(DataTypeEnum.Decimal)]
        public decimal Price { get; set; }
        
        [Log(DataTypeEnum.Numeric)]
        public int? StockQuantity { get; set; }
        
        [Log(DataTypeEnum.Numeric)]
        public int CategoryId { get; set; }
       
        public virtual CategoryDbo Category{ get; set; }

        public virtual IEnumerable<OrderItemDbo> OrderItems { get; set; }

    }
}
