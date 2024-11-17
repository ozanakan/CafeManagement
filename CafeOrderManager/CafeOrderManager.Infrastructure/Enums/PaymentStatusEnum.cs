using CafeOrderManager.Infrastructure.Attributes;

namespace CafeOrderManager.Infrastructure.Enums
{
    public enum PaymentStatusEnum
    {
        [StringValue("enum.table.payment_status.paid")]
        Paid = 1,
        [StringValue("enum.table.payment_status.unpaid")]
        UnPaid = 2,
    }
}