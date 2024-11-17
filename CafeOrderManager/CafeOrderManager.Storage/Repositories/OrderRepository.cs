using System.Linq;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Order;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage.Repositories
{
    public class OrderRepository : BaseRepository<CafeOrderManagerDbContext, OrderDbo, OrderFilterDto>
    {
        public OrderRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<OrderDbo> GetQueryable() => _context.Order.AsQueryable();
       protected override IQueryable<OrderDbo> AlwaysIncludeInList(IQueryable<OrderDbo> query) =>
          query.Include(o => o.Table)
               .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Product); 

        protected override IQueryable<OrderDbo> AlwaysIncludeInDetail(IQueryable<OrderDbo> query) =>
            query.Include(o => o.Table)
                 .Include(o => o.OrderItems)
                 .ThenInclude(oi => oi.Product) 
                 .Include(o => o.Payments);

        protected override IQueryable<OrderDbo> FilterByFilterModel(IQueryable<OrderDbo> query,
            OrderFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
                (string.IsNullOrEmpty(filter.SearchText) || x.OrderNumber.ToString().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Contains(filter.SearchText.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            );
        protected override IQueryable<OrderDbo> OrderBy(IQueryable<OrderDbo> query, OrderFilterDto filter)
        {
            if (filter?.SortBy != null)
            {
                if (filter.SortBy.Key.Equals("id"))
                    if (filter.SortBy.Order.Equals("asc")) return query.OrderBy(f => f.Id);
                    else return query.OrderByDescending(f => f.Id);
            }

            return base.OrderBy(query, filter);
        }
    }
}