using System.Linq;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Product;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage.Repositories
{
    public class ProductRepository : BaseRepository<CafeOrderManagerDbContext, ProductDbo, ProductFilterDto>
    {
        public ProductRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<ProductDbo> GetQueryable() => _context.Product.AsQueryable();

        protected override IQueryable<ProductDbo> AlwaysIncludeInList(IQueryable<ProductDbo> query) =>
            query.Include(p => p.Category).Include(p => p.OrderItems);

        protected override IQueryable<ProductDbo> AlwaysIncludeInDetail(IQueryable<ProductDbo> query) =>
            query.Include(p => p.Category).Include(p => p.OrderItems);

        protected override IQueryable<ProductDbo> FilterByFilterModel(IQueryable<ProductDbo> query,
            ProductFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
              (filter.CategoryId == null || x.CategoryId == filter.CategoryId)
             &&   (string.IsNullOrEmpty(filter.SearchText) ||
            x.ProductName.ToString().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Contains(filter.SearchText.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            );
        protected override IQueryable<ProductDbo> OrderBy(IQueryable<ProductDbo> query, ProductFilterDto filter)
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