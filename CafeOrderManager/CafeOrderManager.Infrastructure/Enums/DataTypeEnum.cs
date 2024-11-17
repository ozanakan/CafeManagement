using CafeOrderManager.Infrastructure.Attributes;

namespace CafeOrderManager.Infrastructure.Enums
{
    public enum DataTypeEnum
    {
        [StringValue("enum.data_type.numeric")]
        Numeric = 1,
        [StringValue("enum.data_type.string")]
        String,
        [StringValue("enum.data_type.datetime")]
        Datetime,
        [StringValue("enum.data_type.bool")]
        Bool,
        [StringValue("enum.data_type.enum")]
        Enum,
        [StringValue("enum.data_type.decimal")]
        Decimal,
        [StringValue("enum.data_type.json")]
        Json,
        [StringValue("enum.data_type.image")]
        Image
    }
}