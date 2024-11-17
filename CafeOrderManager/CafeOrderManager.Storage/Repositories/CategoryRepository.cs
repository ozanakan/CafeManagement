using System.Linq;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Category;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage.Repositories
{
    public class CategoryRepository : BaseRepository<CafeOrderManagerDbContext, CategoryDbo, CategoryFilterDto>
    {
        public CategoryRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<CategoryDbo> GetQueryable() => _context.Category.AsQueryable();

        protected override IQueryable<CategoryDbo> AlwaysIncludeInList(IQueryable<CategoryDbo> query) =>
            query.Include(c => c.Products); 

        protected override IQueryable<CategoryDbo> AlwaysIncludeInDetail(IQueryable<CategoryDbo> query) =>
            query.Include(c => c.Products); 
        protected override IQueryable<CategoryDbo> FilterByFilterModel(IQueryable<CategoryDbo> query,
            CategoryFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
                (string.IsNullOrEmpty(filter.SearchText) || x.CategoryName.ToString().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Contains(filter.SearchText.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            );
        protected override IQueryable<CategoryDbo> OrderBy(IQueryable<CategoryDbo> query, CategoryFilterDto filter)
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