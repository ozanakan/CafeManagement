using System;
using System.Threading.Tasks;
using System.Transactions;
using CafeOrderManager.Infrastructure.Configuration;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Exceptions;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.User;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Service.User;
using CafeOrderManager.Storage.Repositories;
using Microsoft.Extensions.Options;

namespace CafeOrderManager.Service.Security
{
    public class
        SecurityService : BaseService<UserRepository, UserMapper, UserDbo, UserDto, UserListDto, UserFilterDto>
    {
        private readonly SettingsConfig _settings;

        public SecurityService(UserRepository repository, UserMapper mapper,
            IOptions<SettingsConfig> settings,
            IAuthService authService) : base(repository, mapper, authService)
        {
            _settings = settings.Value;
           }

        public async Task<Result<UserListDto>> Login(UserFilterDto filterDto)
        {
            var result = new Result<UserListDto>();
            try
            {
                if (filterDto.Password != null)
                    filterDto.Password = CryptographyExtension.ComputeSha256Hash(filterDto.Password);
                var dbo = await _repository.Login(filterDto);

                if (dbo == null)
                    throw new CustomException("validation.login.wrong_username_or_password");
                if (dbo.Status != StatusEnum.Active)
                    throw new CustomException("validation.login.user_not_active");

                var dto = _mapper.ToListDto(dbo);
                dto.Token = _authService.CreateAndWriteToken(dto.Id, filterDto.IsMobile, _settings.JwtSecurityKey);
               
                result.Success(dto);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserListDto>> UserInfo()
        {
            var result = new Result<UserListDto>();
            try
            {
                var authUser = await _authService.GetUser();
                var user = await _repository.Detail(authUser.UserId, false);
                if (user.Status == StatusEnum.Active)
                    result.Success(_mapper.ToListDto(user));
                else
                    result.Success(null);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }


    }
}