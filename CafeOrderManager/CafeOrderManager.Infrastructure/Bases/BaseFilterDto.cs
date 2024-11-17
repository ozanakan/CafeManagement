using System;
using System.Collections.Generic;
using System.Linq;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Bases
{
    public class BaseFilterDto<TDbo>
        where TDbo : BaseDbo
    {
        public int? Id { get; set; }
        public List<int> IdList { get; set; }
        public List<int> IgnoreIdList { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public bool Track { get; set; } = false;
        public DateTime? MaxCreatedDate { get; set; }
        public DateTime? MinCreatedDate { get; set; }
        public Func<IQueryable<TDbo>, IQueryable<TDbo>> Include { get; set; }
        public bool IsMobile { get; set; }
        public string SearchText { get; set; }
        public SortByDto SortBy { get; set; }
        public StatusEnum? Status { get; set; }

        public IList<StatusEnum> StatusList { get; set; } = new List<StatusEnum>()
        {
            StatusEnum.Active,
            StatusEnum.Passive
        };

        public void SetStatusActive()
        {
            StatusList = new List<StatusEnum>()
            {
                StatusEnum.Active
            };
        }
    }
}