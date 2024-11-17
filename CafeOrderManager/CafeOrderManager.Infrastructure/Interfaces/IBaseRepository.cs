using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Interfaces
{
    public interface IBaseRepository<TDbo, TFilterDto>
    {
        Task<(IEnumerable<TDbo> Data, PaginationDto Pagination)> List(TFilterDto filter,
            Func<IQueryable<TDbo>, IQueryable<TDbo>> include = null);

        Task<TDbo> Detail(int id, bool track = true,
            Func<IQueryable<TDbo>, IQueryable<TDbo>> include = null);

        Task<TDbo> Create(TDbo dbo);

        Task<IEnumerable<TDbo>> Create(IEnumerable<TDbo> dbos);

        Task<bool> Update(TDbo dbo);

        Task<IEnumerable<TDbo>> Update(IEnumerable<TDbo> dbos);

        //Task<int> BatchUpdate(TFilterDto filter, Expression<Func<TDbo, TDbo>> updateFactory);

        //Task<int> BatchDelete(TFilterDto filter);
        bool CreateValidation(TDbo dbo);
        bool UpdateValidation(TDbo dbo);
        Task<bool> Any(TFilterDto filter);
    }
}