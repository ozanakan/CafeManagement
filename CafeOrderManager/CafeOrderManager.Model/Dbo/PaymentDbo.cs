using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeOrderManager.Model.Dbo
{
    [Table("Payment")]
    public class PaymentDbo : BaseDbo
    {
        public PaymentDbo()
        {
        }

        [Log(DataTypeEnum.Numeric)]
        public int OrderId { get; set; }

        [Log(DataTypeEnum.Decimal)]
        public decimal Amount { get; set; }

        [Log(DataTypeEnum.Datetime)]
        public DateTime PaymentDate { get; set; }

        [Log(DataTypeEnum.Enum)]
        public PaymentStatusEnum PaymentStatus { get; set; } // 'paid', 'unpaid'


        public OrderDbo Order { get; set; }

    }
}
