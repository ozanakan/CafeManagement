using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
//using Z.EntityFramework.Plus;

namespace CafeOrderManager.Storage.Repositories
{
    public abstract class BaseRepository<TDbContext, TDbo, TFilterDto> : IBaseRepository<TDbo, TFilterDto>
        where TDbo : BaseDbo
        where TFilterDto : BaseFilterDto<TDbo>
        where TDbContext : DbContext
    {
        protected readonly TDbContext _context;

        protected BaseRepository(TDbContext context)
        {
            _context = context;
        }

        #region Helper Methods

        protected abstract IQueryable<TDbo> GetQueryable();
        protected virtual IQueryable<TDbo> AlwaysIncludeInDetail(IQueryable<TDbo> query) => query;

        protected virtual IQueryable<TDbo> AlwaysIncludeInList(IQueryable<TDbo> query) => query;

        protected virtual IQueryable<TDbo> OrderBy(IQueryable<TDbo> query, TFilterDto filter) =>
            query.OrderByDescending(x => x.CreatedDate);

        protected virtual IQueryable<TDbo> FilterByFilterModel(IQueryable<TDbo> query,
            TFilterDto filter)
            => OrderBy(query.Where(x =>
                (filter.Id == null || x.Id == filter.Id)
                && (filter.IdList == null || !filter.IdList.Any() || filter.IdList.Contains(x.Id))
                && (filter.IgnoreIdList == null || !filter.IgnoreIdList.Any() || !filter.IgnoreIdList.Contains(x.Id))
                && (filter.Status == null || filter.Status == x.Status)
                && (filter.StatusList == null || !filter.StatusList.Any() || filter.StatusList.Contains(x.Status))
                && (filter.MinCreatedDate == null || filter.MinCreatedDate <= x.CreatedDate)
                && (filter.MaxCreatedDate == null || filter.MaxCreatedDate >= x.CreatedDate)), filter);


        protected IQueryable<TDbo> GetQueryableForDetail(bool track,
            Func<IQueryable<TDbo>, IQueryable<TDbo>> include = null)
        {
            var query = AlwaysIncludeInDetail(GetQueryable());
            if (track == false)
                query = query.AsNoTracking();

            if (include != null)
                query = include(query);

            return query;
        }

        public virtual bool CreateValidation(TDbo dbo) => true;
        public virtual bool UpdateValidation(TDbo dbo) => true;
        public Task<bool> Any(TFilterDto filter)
        {
            var query = FilterByFilterModel(GetQueryable(), filter);
            return query.AnyAsync();
        }

        public virtual bool DeleteValidation(TDbo dbo) => true;


        #endregion

        #region Main Methods

        public virtual async Task<(IEnumerable<TDbo> Data, PaginationDto Pagination)> List(
            TFilterDto filter, Func<IQueryable<TDbo>, IQueryable<TDbo>> include = null)
        {
            var query = AlwaysIncludeInList(GetQueryable());
            if (include != null)
                query = include(query);

            if (filter.Track == false)
                query = query.AsNoTracking();

            query = FilterByFilterModel(query, filter);


            PaginationDto paging = null;
            if (filter.PageSize != null)
            {
                paging = new PaginationDto()
                {
                    PageNumber = filter.PageNumber ?? 1,
                    PageSize = filter.PageSize,
                };
                paging.TotalCount = await query.CountAsync();
                paging.PageCount = (int)Math.Ceiling((double)paging.TotalCount / filter.PageSize.Value);

                if (filter.PageNumber != null)
                {
                    query = query.Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value);
                }

                query = query.Take(filter.PageSize.Value);
            }
            else
            {
                paging = new PaginationDto()
                {
                    PageNumber = 1,
                    PageSize = 1,
                };
                paging.TotalCount = await query.CountAsync();
                paging.PageCount = 1;
            }

            return (await query.ToListAsync(), paging);
        }

        public virtual async Task<int> Count(TFilterDto filter)
        {
            var query = AlwaysIncludeInList(GetQueryable());
            query = query.AsNoTracking();
            query = FilterByFilterModel(query, filter);
            return (await query.CountAsync());
        }

        public virtual async Task<TDbo> Detail(int id, bool track = true,
            Func<IQueryable<TDbo>, IQueryable<TDbo>> include = null)
        {
            return await GetQueryableForDetail(track, include).FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<TDbo> Create(TDbo dbo)
        {
            CreateValidation(dbo);
            _context.Add(dbo);
            await _context.SaveChangesAsync();
            return dbo;
        }

        public virtual async Task<IEnumerable<TDbo>> Create(IEnumerable<TDbo> dbos)
        {
            _context.AddRange(dbos);
            await _context.SaveChangesAsync();
            return dbos;
        }

        public virtual async Task<bool> Update(TDbo dbo)
        {
            if (dbo.Status == StatusEnum.Deleted)
                DeleteValidation(dbo);
            else
                UpdateValidation(dbo);
            _context.Update(dbo);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<IEnumerable<TDbo>> Update(IEnumerable<TDbo> dbos)
        {
            _context.UpdateRange(dbos);
            await _context.SaveChangesAsync();
            return dbos;
        }

        //public virtual async Task<int> BatchUpdate(TFilterDto filter, Expression<Func<TDbo, TDbo>> updateFactory)
        //{
        //    return await FilterByFilterModel(GetQueryable(), filter).UpdateAsync(updateFactory);
        //}

        //public virtual async Task<int> BatchDelete(TFilterDto filter)
        //{
        //    return await FilterByFilterModel(GetQueryable(), filter).DeleteAsync();
        //}


        #endregion
    }
}