using System.Linq;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Table;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage.Repositories
{
    public class TableRepository : BaseRepository<CafeOrderManagerDbContext, TableDbo, TableFilterDto>
    {
        public TableRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<TableDbo> GetQueryable() => _context.Table.AsQueryable();
        protected override IQueryable<TableDbo> AlwaysIncludeInList(IQueryable<TableDbo> query) =>
       query.Include(t => t.Orders);

        protected override IQueryable<TableDbo> AlwaysIncludeInDetail(IQueryable<TableDbo> query) =>
            query.Include(t => t.Orders);
        protected override IQueryable<TableDbo> FilterByFilterModel(IQueryable<TableDbo> query,
            TableFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
                (string.IsNullOrEmpty(filter.SearchText) || x.TableNumber.ToString().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Contains(filter.SearchText.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            );
        protected override IQueryable<TableDbo> OrderBy(IQueryable<TableDbo> query, TableFilterDto filter)
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