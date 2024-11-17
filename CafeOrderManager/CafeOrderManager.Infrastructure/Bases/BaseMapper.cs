using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Bases
{
    public abstract class BaseMapper<TDbo, TDto, TListDto>
        where TListDto : BaseListDto, new()
        where TDbo : BaseDbo, new()
        where TDto : BaseDto, new()
    {

        protected readonly IAuthService _authService;
        public BaseMapper(IAuthService authService)
        {
            _authService = authService;
        }

        public virtual IEnumerable<TListDto> ToListDto(IEnumerable<TDbo> list) =>
            (from item in list select ToListDto(item));
        public virtual TListDto ToListDto(TDbo dbo)
        {
            return new TListDto
            {
                Id = dbo.Id,
                CreatedDate = dbo.CreatedDate,
                CreatedUserId = dbo.CreatedUserId,
                Status = dbo.Status
            };
        }

        public virtual TDto ToDto(TDbo dbo)
        {
            return new TDto
            {
                Id = dbo.Id,
                CreatedDate = dbo.CreatedDate,
                CreatedUserId = dbo.CreatedUserId,
                Status = dbo.Status
            };
        }

        public virtual TDbo ToCreate(TDto dto) => new TDbo
        {
            Status = StatusEnum.Active,
            CreatedUserId = _authService.IsLogin() ? _authService.GetUserId() : null,
            CreatedDate = DateTime.UtcNow
        };

        public virtual TDbo ToUpdate(TDbo dbo, TDto dto)
        {
            dbo.Status = dto.Status.Value;
            return dbo;
        }

        public virtual TDbo ToUpdateStatus(TDbo dbo, StatusEnum status)
        {
            dbo.Status = status;
            return dbo;
        }

        public virtual IEnumerable<DropdownDto> ToDropdown(IEnumerable<TDbo> list) =>
            (from item in list select ToDropdown(item));

        public virtual DropdownDto ToDropdown(TDbo dbo) => new DropdownDto()
        {
            Value = dbo.Id.ToString()
        };

        public virtual async Task<IEnumerable<DropdownDto>> ToDropdownAsync(IEnumerable<TDbo> list)
        {
            List<DropdownDto> result = new List<DropdownDto>();
            foreach (var item in list)
            {
                result.Add(await ToDropdownAsync(item));
            }

            return result;
        }

        public virtual async Task<DropdownDto> ToDropdownAsync(TDbo dbo)
        {
            return new DropdownDto()
            {
                Value = dbo.Id.ToString()
            };
        }
    }
}