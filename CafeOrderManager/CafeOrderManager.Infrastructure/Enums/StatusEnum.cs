using CafeOrderManager.Infrastructure.Attributes;

namespace CafeOrderManager.Infrastructure.Enums
{
    public enum StatusEnum
    {
        [StringValue("enum.status.active")]
        Active = 1,
        [StringValue("enum.status.passive")]
        Passive = 2,
        [StringValue("enum.status.deleted")]
        Deleted = 3
    }
}