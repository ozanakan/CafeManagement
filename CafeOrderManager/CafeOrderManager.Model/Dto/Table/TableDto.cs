using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace CafeOrderManager.Model.Dto.Table
{
    public class TableDto : BaseDto
    {
        public string TableNumber { get; set; }
        public TableStatusEnum TableStatus { get; set; }
    }
}