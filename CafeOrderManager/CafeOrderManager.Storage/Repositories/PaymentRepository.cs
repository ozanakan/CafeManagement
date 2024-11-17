using System.Linq;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Payment;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderManager.Storage.Repositories
{
    public class PaymentRepository : BaseRepository<CafeOrderManagerDbContext, PaymentDbo, PaymentFilterDto>
    {
        public PaymentRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<PaymentDbo> GetQueryable() => _context.Payment.AsQueryable();
        protected override IQueryable<PaymentDbo> AlwaysIncludeInList(IQueryable<PaymentDbo> query) =>
       query.Include(p => p.Order);

        protected override IQueryable<PaymentDbo> AlwaysIncludeInDetail(IQueryable<PaymentDbo> query) =>
            query.Include(p => p.Order);
        protected override IQueryable<PaymentDbo> FilterByFilterModel(IQueryable<PaymentDbo> query,
            PaymentFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
                (string.IsNullOrEmpty(filter.SearchText) || x.Order.OrderNumber.ToString().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Contains(filter.SearchText.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            );
        protected override IQueryable<PaymentDbo> OrderBy(IQueryable<PaymentDbo> query, PaymentFilterDto filter)
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