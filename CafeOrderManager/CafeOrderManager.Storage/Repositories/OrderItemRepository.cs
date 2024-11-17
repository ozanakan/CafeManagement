using System.Linq;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.OrderItem;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage.Repositories
{
    public class OrderItemRepository : BaseRepository<CafeOrderManagerDbContext, OrderItemDbo, OrderItemFilterDto>
    {
        public OrderItemRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<OrderItemDbo> GetQueryable() => _context.OrderItem.AsQueryable();
        protected override IQueryable<OrderItemDbo> AlwaysIncludeInList(IQueryable<OrderItemDbo> query) =>
            query.Include(oi => oi.Product)
                 .ThenInclude(p => p.Category)
                 .Include(oi => oi.Order);
        protected override IQueryable<OrderItemDbo> AlwaysIncludeInDetail(IQueryable<OrderItemDbo> query) =>
            query.Include(oi => oi.Product)
                 .ThenInclude(p => p.Category)
                 .Include(oi => oi.Order);

        protected override IQueryable<OrderItemDbo> FilterByFilterModel(IQueryable<OrderItemDbo> query,
            OrderItemFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
                 (filter.OrderId== null || x.OrderId== filter.OrderId)
              &&  (string.IsNullOrEmpty(filter.SearchText) || x.OrderId.ToString().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Contains(filter.SearchText.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            );
        protected override IQueryable<OrderItemDbo> OrderBy(IQueryable<OrderItemDbo> query, OrderItemFilterDto filter)
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