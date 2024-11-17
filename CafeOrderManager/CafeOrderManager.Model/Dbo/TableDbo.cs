//using System;
//using System.Collections.Generic;
using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeOrderManager.Model.Dbo
{
    [Table("Table")]
    public class TableDbo : BaseDbo
    {
        public TableDbo()
        {
        }

        [Log(DataTypeEnum.String)]
        public string TableNumber { get; set; }

        [Log(DataTypeEnum.Enum)]
        public TableStatusEnum TableStatus { get; set; }

        public virtual IEnumerable<OrderDbo> Orders { get; set; }

    }
}
