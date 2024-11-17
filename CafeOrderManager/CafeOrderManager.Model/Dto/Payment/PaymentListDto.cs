using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System;

namespace CafeOrderManager.Model.Dto.Payment
{
    public class PaymentListDto : BaseListDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } // 'paid', 'unpaid'

    }
}
