using CafeOrderManager.Infrastructure.Attributes;

namespace CafeOrderManager.Infrastructure.Enums
{
    public enum  TableStatusEnum
    { 
        [StringValue("enum.table.status.empty")]
        Empty = 1,
        [StringValue("enum.table.status.occupied")]
        Occupied = 2,
        [StringValue("enum.table.status.reserved")]
        Reserved = 3
    }
}