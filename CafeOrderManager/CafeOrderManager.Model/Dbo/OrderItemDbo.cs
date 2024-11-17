using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeOrderManager.Model.Dbo
{
    [Table("OrderItem")]
    public class OrderItemDbo : BaseDbo
    {
        public OrderItemDbo()
        {
        }

        [Log(DataTypeEnum.Numeric)]
        public int OrderId { get; set; }

        [Log(DataTypeEnum.Numeric)]
        public int ProductId { get; set; }

        [Log(DataTypeEnum.Numeric)]
        public int Quantity { get; set; }


        public ProductDbo Product { get; set; }
        public OrderDbo Order { get; set; }
       
    }
}
