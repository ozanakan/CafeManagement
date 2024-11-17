using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.User;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Exceptions;
using CafeOrderManager.Infrastructure.Extensions;

namespace CafeOrderManager.Storage.Repositories
{
    public class UserRepository : BaseRepository<CafeOrderManagerDbContext, UserDbo, UserFilterDto>
    {
        public UserRepository(CafeOrderManagerDbContext context) : base(context)
        {
        }

        protected override IQueryable<UserDbo> GetQueryable() => _context.User.AsQueryable();

        //protected override IQueryable<UserDbo> AlwaysIncludeInDetail(IQueryable<UserDbo> query) =>
        //    query.Include(f => f.Role).Include(f => f.Organization).Include(x => x.Department);

        //protected override IQueryable<UserDbo> AlwaysIncludeInList(IQueryable<UserDbo> query) =>
        //    query.Include(f => f.Role).Include(x => x.Organization).Include(x => x.Department);

        protected override IQueryable<UserDbo> FilterByFilterModel(IQueryable<UserDbo> query, UserFilterDto filter) =>
            base.FilterByFilterModel(query, filter).Where(x =>
                (string.IsNullOrEmpty(filter.LoginId) || x.LoginId.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")
                    .Contains(filter.LoginId.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
                && (string.IsNullOrEmpty(filter.LoginIdEquals) ||
                    x.LoginId.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "").Equals(filter.LoginIdEquals.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
            
                // AuthorizedServiceTypes alanında kullanıcının ilgilendiği serviceType ları içerir bu filtre ile ilgilendiği tiplere göre kullancıları filtreleriz.
                && (string.IsNullOrEmpty(filter.Name) || (x.Name.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "") + x.Surname.ToLower().Replace(" ", "")).Contains(filter.Name.ToLower().Replace(" ", "")))
                && (string.IsNullOrEmpty(filter.Email) || x.Email.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")
                    .Contains(filter.Email.ToLower().Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace(" ", "")))
                && (string.IsNullOrEmpty(filter.SearchText)
                    || (x.Name.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower() + x.Surname.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower()).Contains(filter.SearchText.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower())
                    || x.Surname.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower().Contains(filter.SearchText.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower())
                    || x.LoginId.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower().Contains(filter.SearchText.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower())
                    || x.Email.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower().Contains(filter.SearchText.Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower())
                  ));

        protected override IQueryable<UserDbo> OrderBy(IQueryable<UserDbo> query, UserFilterDto filter)
        {
            if (filter?.SortBy != null)
            {
                if (filter.SortBy.Key.Equals("fullName"))
                    if (filter.SortBy.Order.Equals("asc")) return query.OrderBy(f => f.Name);
                    else return query.OrderByDescending(f => f.Name);
                else if (filter.SortBy.Key.Equals("loginId"))
                    if (filter.SortBy.Order.Equals("asc")) return query.OrderBy(f => f.LoginId);
                    else return query.OrderByDescending(f => f.LoginId);
                else if (filter.SortBy.Key.Equals("email"))
                    if (filter.SortBy.Order.Equals("asc")) return query.OrderBy(f => f.Email);
                    else return query.OrderByDescending(f => f.Email);
                else if (filter.SortBy.Key.Equals("createdDate"))
                    if (filter.SortBy.Order.Equals("asc")) return query.OrderBy(f => f.CreatedDate);
                    else return query.OrderByDescending(f => f.CreatedDate);
                }

            return base.OrderBy(query, filter);
        }


        public async Task<UserDbo> Login(UserFilterDto filter)
        {
            return await GetQueryable().FirstOrDefaultAsync(f =>
                    (f.LoginId.Trim().Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower().Equals(filter.LoginId.Trim().Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower()) ||
                     f.Email.Trim().Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower().Equals(filter.LoginId.Trim().Replace("Ü", "u").Replace("U", "u").Replace("ü", "u").Replace("Ş", "s").Replace("S", "s").Replace("ş", "s").Replace("İ", "ı").Replace("i", "ı").Replace("I", "ı").Replace("Ö", "o").Replace("O", "o").Replace("ö", "o").Replace("Ç", "c").Replace("C", "c").Replace("ç", "c").Replace("Ğ", "g").Replace("G", "g").Replace("ğ", "g").ToLower())) &&
                    f.Password.Equals(filter.Password) &&
                    f.Status != StatusEnum.Deleted);
        }

        public async Task<UserDbo> DetailWithEmailOrLoginId(string email)
        {
            return await GetQueryable().FirstOrDefaultAsync(x =>
                (x.Email == email || x.LoginId == email) && x.Status != StatusEnum.Deleted);
        }

        public async Task<UserDbo> DetailWithPasswordKey(string passwordKey)
        {
            return await GetQueryable()
                .FirstOrDefaultAsync(x => x.PasswordKey == passwordKey && x.Status != StatusEnum.Deleted);
        }

      
    }
}