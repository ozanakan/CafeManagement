﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BieksperV2.Api.Attributes;
//using CafeOrderManager.Api.Attributes;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Extensions;
//using CafeOrderManager.Infrastructure.Exceptions;
//using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeOrderManager.Api.Controllers
{
    [ApiController]
    [AuthenticationControl]
    [Route("[controller]")]
    [Authorize]
    public abstract class
        BaseController<TService, TFilterDto, TDto, TDbo, TListDto> : ControllerBase
        where TDto : BaseDto, new()
        where TDbo : BaseDbo, new()
        where TListDto : BaseListDto, new()
        where TFilterDto : BaseFilterDto<TDbo>
        where TService : IBaseService<TDbo, TDto, TListDto, TFilterDto>
    {
        protected readonly TService _service;

        public BaseController(TService service)
        {
            _service = service;
        }

        // POST: [Controller]/list
        [HttpPost("list")]
        public virtual async Task<IActionResult> List([FromBody] TFilterDto filterDto)
        {
            var result = new Result<IEnumerable<TListDto>>();
            try
            {
                //await HasAccess(ActionTypeEnum.List);
                result = await _service.List(filterDto);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        // GET: [Controller]/4
        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = new Result<TDto>();
            try
            {
                //await HasAccess(ActionTypeEnum.List);
                //ValidateForDetail(id);
                result = await _service.Detail(id);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        // POST: [Controller]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TDto dto)
        {
            var result = new Result<IEnumerable<int>>();
            try
            {
                //await HasAccess(ActionTypeEnum.Create);
                //ValidateForCreate(dto);
                result = await _service.Create(dto);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        // PUT: [Controller]/4
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put([FromRoute] int id, [FromBody] TDto dto)
        {
            var result = new Result<bool>();
            try
            {
                //await HasAccess(ActionTypeEnum.Update);
                //ValidateForUpdate(dto);
                result = await _service.Update(id, dto);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        // DELETE: [Controller]/4
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = new Result<bool>();
            try
            {
                //await HasAccess(ActionTypeEnum.Delete);
                //ValidateForDelete(id);
                result = await _service.Delete(id);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        // DELETE: [Controller]/4
        [HttpDelete]
        public virtual async Task<IActionResult> Delete(TFilterDto filter)
        {
            var result = new Result<bool>();
            try
            {
                //await HasAccess(ActionTypeEnum.Delete);
                //ValidateForDeleteMultiple(filter);
                result = await _service.DeleteMultiple(filter);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        [HttpPost("dropdown")]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Dropdown([FromBody] TFilterDto filterDto)
        {
            var result = new Result<IEnumerable<DropdownDto>>();
            try
            {
                result = await _service.Dropdown(filterDto);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        // PUT: [Controller]/updateStatus/4/2
        [HttpPut("updateStatus/{id}/{status}")]
        public virtual async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromRoute] StatusEnum status)
        {
            var result = new Result<bool>();
            try
            {
                //await HasAccess(ActionTypeEnum.Update);
                //ValidateForUpdateStatus(id, status);
                result = await _service.UpdateStatus(id, status);
            }
            catch (Exception e)
            {
                result.Error(e);
            }

            return GetResult(result);
        }

        //// POST: [Controller]/dropdown
        //[HttpPost("dropdown")]
        //[AllowAnonymous]
        //public virtual async Task<IActionResult> Dropdown([FromBody] TFilterDto filterDto)
        //{
        //    var result = new Result<IEnumerable<DropdownDto>>();
        //    try
        //    {
        //        result = await _service.Dropdown(filterDto);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Error(e);
        //    }

        //    return GetResult(result);
        //}

        //#region Helpers

        //protected virtual void ValidateForCreate(TDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiValidationException();
        //}

        //protected virtual void ValidateForUpdate(TDto dto)
        //{
        //    if (!ModelState.IsValid)
        //        throw new ApiValidationException();
        //}

        //protected virtual void ValidateForDetail(int id)
        //{
        //    if (id == 0)
        //        throw new ApiValidationException();
        //}

        //protected virtual void ValidateForDelete(int id)
        //{
        //    if (id == 0)
        //        throw new ApiValidationException();
        //}

        //protected virtual void ValidateForDeleteMultiple(TFilterDto filter)
        //{
        //    if (filter?.IdList?.Any() != true)
        //        throw new ApiValidationException();
        //}

        //protected virtual void ValidateForUpdateStatus(int id, StatusEnum status)
        //{
        //    if (id == 0 || status <= 0)
        //        throw new ApiValidationException();
        //}

        //protected virtual async Task<bool> HasAccess(ActionTypeEnum actions,
        //    bool? hasAccess = true)
        //{
        //    if (hasAccess != true)
        //        throw new UnAuthorizedException();
        //    return true;
        //}

        //protected virtual async Task<bool> HasAccess(bool? hasAccess = true)
        //{
        //    if (hasAccess != true)
        //        throw new UnAuthorizedException();
        //    return true;
        //}
        //protected virtual async Task<bool> HasAccess(ActionEnum actions,
        //  bool? hasAccess = true)
        //{
        //    if (hasAccess != true)
        //        throw new UnAuthorizedException();
        //    return true;
        //}

        protected IActionResult GetResult(Result result)
        {
            if (result.EnsureSucces())
                return Ok(result);
            if (result.ExceptionType.Contains("UserNotActive"))
                return Unauthorized(result);
            if (result.ExceptionType.Contains("UnAuthorized"))
                return BadRequest(result);
            if (result.ExceptionType.Contains("Validation"))
            {
                result.ExceptionMessage = ModelState.Values.FirstOrDefault(f => f.Errors.Any())?.Errors.FirstOrDefault()
                    ?.ErrorMessage;
                return NotFound(result);
            }

            return BadRequest(result);
        }

        
    }
}