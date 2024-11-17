using System;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Infrastructure.Bases
{
    public class BaseListDto
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public StatusEnum Status { get; set; }
     
    }
}