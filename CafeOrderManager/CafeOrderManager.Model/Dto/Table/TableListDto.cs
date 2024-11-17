using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Model.Dto.Table
{
    public class TableListDto : BaseListDto
    {
        public string TableNumber { get; set; }
        public TableStatusEnum TableStatus { get; set; }
    }
}
