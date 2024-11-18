using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Interfaces
{
    public interface IBaseService<TDbo, TDto, TListDto, TFilterDto>
    {
        Task<Result<IEnumerable<TListDto>>> List(TFilterDto filterDto);
        Task<(IEnumerable<TListDto> Data, PaginationDto Pagination)> _List(TFilterDto filterDto);
        Task<Result<TDto>> Detail(int id);
        Task<Result<IEnumerable<int>>> Create(TDto dto);
        Task<IEnumerable<TDbo>> _Create(TDto dto);
        Task<Result<bool>> Update(int id, TDto dto);
        Task<bool> _Update(TDbo dbo, TDto dto);
        Task<Result<bool>> UpdateStatus(int id, StatusEnum status);
        Task<Result<bool>> Delete(int id);
        Task<Result<bool>> DeleteMultiple(TFilterDto filter);
        Task<List<TDbo>> DeleteMultipleValidateOrAddProcess(List<TDbo> dboList);
        Task<bool> _Delete(TDbo dbo);
        Task<Result<IEnumerable<DropdownDto>>> Dropdown(TFilterDto filterDto);
        Task<(IEnumerable<DropdownDto> Data, PaginationDto Pagination)> _Dropdown(TFilterDto filterDto);
    }
}