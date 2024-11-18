using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Exceptions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Service.Base
{
    public abstract class
            BaseService<TRepository, TMapper, TDbo, TDto, TListDto, TFilterDto> : IBaseService<TDbo, TDto, TListDto, TFilterDto>
        where TDbo : BaseDbo, new()
        where TFilterDto : BaseFilterDto<TDbo>, new()
        where TDto : BaseDto, new()
        where TListDto : BaseListDto, new()
        where TRepository : IBaseRepository<TDbo, TFilterDto>
        where TMapper : BaseMapper<TDbo, TDto, TListDto>
    {
        protected readonly TRepository _repository;
        protected readonly TMapper _mapper;
        protected readonly IAuthService _authService;


        protected BaseService(TRepository repository, TMapper mapper, IAuthService authService)
        {
            _repository = repository;
            _mapper = mapper;
            _authService = authService;
        }

        public virtual async Task<Result<IEnumerable<TListDto>>> List(TFilterDto filterDto)
        {
            var result = new Result<IEnumerable<TListDto>>();
            try
            {
                var res = await _List(filterDto);
                result.Success(res.Data, res.Pagination);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<(IEnumerable<TListDto> Data, PaginationDto Pagination)> _List(TFilterDto filterDto)
        {
            var list = await _repository.List(filterDto);
            return (_mapper.ToListDto(list.Data), list.Pagination);
        }

        public virtual async Task<Result<TDto>> Detail(int id)
        {
            var result = new Result<TDto>();
            try
            {
                var dbo = await _repository.Detail(id, false);

                if (dbo == null)
                    throw new RecordNotFoundException();

                result.Success(_mapper.ToDto(dbo));
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<Result<IEnumerable<int>>> Create(TDto dto)
        {
            var result = new Result<IEnumerable<int>>();
            try
            {
                var dbo = await _Create(dto);
                result.Success(dbo.Select(x => x.Id));
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<IEnumerable<TDbo>> _Create(TDto dto)
        {

            return new List<TDbo> { await _repository.Create(_mapper.ToCreate(dto)) };
        }

        public virtual async Task<Result<bool>> Update(int id, TDto dto)
        {
            var result = new Result<bool>();
            try
            {
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var dbo = await _repository.Detail(id);

                    if (dbo == null)
                        throw new RecordNotFoundException();

                    result.Success(await _Update(dbo, dto));

                    tran.Complete();
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<bool> _Update(TDbo dbo, TDto dto)
        {
            return await _repository.Update(_mapper.ToUpdate(dbo, dto));
        }

        public virtual async Task<Result<bool>> UpdateStatus(int id, StatusEnum status)
        {
            var result = new Result<bool>();
            try
            {
                var dbo = await _repository.Detail(id, true);

                if (dbo == null)
                    throw new RecordNotFoundException();

                result.Success(await _UpdateStatus(dbo, status));

            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<bool> _UpdateStatus(TDbo dbo, StatusEnum status)
        {
            return await _repository.Update(_mapper.ToUpdateStatus(dbo, status));
        }

        public virtual async Task<Result<bool>> Delete(int id)
        {
            var result = new Result<bool>();
            try
            {
                var dbo = await _repository.Detail(id, true);
                result.Success(await _Delete(dbo));
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }


        public virtual async Task<Result<bool>> DeleteMultiple(TFilterDto filter)
        {
            var result = new Result<bool>();
            try
            {
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    filter.Track = true;
                    var dboList = (await _repository.List(filter)).Data?.ToList();

                    if (dboList?.Any() != true)
                        throw new RecordNotFoundException();

                    dboList = await DeleteMultipleValidateOrAddProcess(dboList);

                    var updatedDboList = new List<TDbo>();
                    foreach (var dbo in dboList)
                    {

                        var newDbo = _mapper.ToUpdateStatus(dbo, StatusEnum.Deleted);

                        updatedDboList.Add(newDbo);
                    }

                    await _repository.Update(updatedDboList);

                    tran.Complete();

                    result.Success(true);
                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<List<TDbo>> DeleteMultipleValidateOrAddProcess(List<TDbo> dboList)
        {
            // If you want to validate before delete multiple or add new process. You can override this method.
            return dboList;
        }

        public virtual async Task<bool> _Delete(TDbo dbo)
        {
            return await _repository.Update(_mapper.ToUpdateStatus(dbo, StatusEnum.Deleted));
        }

        public virtual async Task<Result<IEnumerable<DropdownDto>>> Dropdown(TFilterDto filterDto)
        {
            var result = new Result<IEnumerable<DropdownDto>>();
            try
            {
                filterDto.SetStatusActive();
                var res = await _Dropdown(filterDto);
                result.Success(res.Data, res.Pagination);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public virtual async Task<(IEnumerable<DropdownDto> Data, PaginationDto Pagination)> _Dropdown(
            TFilterDto filterDto)
        {
            var list = await _repository.List(filterDto);
            return (_mapper.ToDropdown(list.Data), list.Pagination);
        }


    }
}