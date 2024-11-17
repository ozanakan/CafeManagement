using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeOrderManager.Model.Dbo
{
    [Table("Order")]
    public class OrderDbo : BaseDbo
    {
        public OrderDbo()
        {
        }

        [Log(DataTypeEnum.String)]
        public string OrderNumber { get; set; }

        [Log(DataTypeEnum.Numeric)]
        public int TableId { get; set; }

        [Log(DataTypeEnum.Enum)]
        public OrderStatusEnum OrderStatus { get; set; } // 'pending', 'in_progress', 'completed', 'cancelled'

        public virtual IEnumerable<OrderItemDbo> OrderItems { get; set; }
        public virtual IEnumerable<PaymentDbo> Payments { get; set; }
        public TableDbo Table { get; set; }
    }
}
