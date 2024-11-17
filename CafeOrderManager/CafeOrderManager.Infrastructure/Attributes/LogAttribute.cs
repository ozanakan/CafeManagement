using System;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Infrastructure.Attributes
{
    public class LogAttribute : Attribute
    {
        public LogAttribute(DataTypeEnum dataType)
        {
            DataType = dataType;
        }
        public DataTypeEnum DataType { get; set; }
    }
}