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
            //moduleType = ModuleTypeEnum.Security;
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

        public async Task<Result<bool>> ForgetPassword(UserFilterDto filterDto)
        {
            var result = new Result<bool>();

            try
            {
                if (filterDto.Email == null)
                {
                    result.Success(false);
                    return result;
                }

                var user = await _repository.DetailWithEmailOrLoginId(filterDto.Email);
                if (user == null)
                {
                    throw new CustomException("validation.email_or_username_not_already_exist");
                }

                user.PasswordKey = Guid.NewGuid().ToString();
                await _repository.Update(user);

                result.Success(true);
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }

        public async Task<Result<UserListDto>> ResetPassword(UserDto model)
        {
            var result = new Result<UserListDto>();

            try
            {
                using (TransactionScope tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (string.IsNullOrEmpty(model.PasswordKey))
                    {
                        throw new CustomException("general.error.forget_password_link_empty");
                    }

                    UserDbo user = await _repository.DetailWithPasswordKey(model.PasswordKey);
                    if (user == null)
                        throw new CustomException("general.error.reset_password.user_not_already_exist");

                    if (user.Password != CryptographyExtension.ComputeSha256Hash(model.Password))
                    {
                        user.Password = CryptographyExtension.ComputeSha256Hash(model.Password);
                        user.PasswordKey = "";
                        bool updateResult = await _repository.Update(user);
                        if (updateResult)
                        {
                            #region Login

                            var userFilter = new UserFilterDto
                            {
                                LoginId = user.LoginId,
                                Password = user.Password,
                            };
                            var userDbo = await _repository.Login(userFilter);

                            var newUserLogin = _mapper.ToListDto(userDbo);
                            newUserLogin.Token =
                                _authService.CreateAndWriteToken(newUserLogin.Id, false, _settings.JwtSecurityKey);

                            result.Success(newUserLogin);
                        }

                        #endregion

                        tran.Complete();
                    }
                    else
                        throw new CustomException("message.current_password_must_not_be_same_as_new_password");

                }
            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }


    }
}