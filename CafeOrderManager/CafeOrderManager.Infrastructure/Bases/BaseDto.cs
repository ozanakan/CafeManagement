using System;
using System.ComponentModel.DataAnnotations.Schema;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Infrastructure.Bases
{
    public class BaseDto
    {
        public int? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedUserId { get; set; }
        public StatusEnum? Status { get; set; }
    }
}