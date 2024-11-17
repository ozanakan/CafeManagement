using CafeOrderManager.Infrastructure.Attributes;

namespace CafeOrderManager.Infrastructure.Enums
{
    public enum OrderStatusEnum
    {
        [StringValue("enum.table.order_status.pending")]
        Pending = 1,
        [StringValue("enum.table.order_status.in_progress")]
        InProgress = 2,
        [StringValue("enum.table.order_status.completed")]
        Completed = 3,
        [StringValue("enum.table.order_status.cancelled")]
        Cancelled = 4
    }
}