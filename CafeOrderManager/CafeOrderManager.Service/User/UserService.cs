
//    using CafeOrderManager.Infrastructure.Enums;
//using CafeOrderManager.Infrastructure.Exceptions;
//using CafeOrderManager.Infrastructure.Interfaces;
//using CafeOrderManager.Infrastructure.Models;
//using CafeOrderManager.Model.Dbo;
//using CafeOrderManager.Model.Dto.User;
//using CafeOrderManager.Service.UserInvite;
//using CafeOrderManager.Storage.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using CafeOrderManager.Infrastructure.Extensions;
//using CafeOrderManager.Service.Background;
//using CafeOrderManager.Service.Base;
//using CafeOrderManager.Service.Language;
//using Microsoft.EntityFrameworkCore;

//namespace CafeOrderManager.Service.User
//{
//    public class UserService : BaseService<UserRepository, UserMapper, UserDbo, UserDto, UserListDto, UserFilterDto>
//    {
//        private readonly UserRepository _repository;
     
//        public UserService(IAuthService authService, UserRepository repository,
//            UserMapper mapper
//           ) : base(
//            repository, mapper, authService)
//        {
//            _repository = repository;
//                 //moduleType = ModuleTypeEnum.User;
//        }

//        public override async Task RemoveCache(ActionTypeEnum actionType, UserDbo dbo)
//        {
//            switch (actionType)
//            {
//                case ActionTypeEnum.Update:
//                case ActionTypeEnum.Delete:
//                    await _authService.RemoveCache(dbo.Id);
//                    break;
//            }
//        }

//        public async Task<Result<List<ActionEnumDto>>> GetUsersActions(UserFilterDto filterDto)
//        {
//            var result = new Result<List<ActionEnumDto>>();
//            try
//            {
//                var user = await _authService.GetUser();
//                OrganizationTypeEnum? organizationType = user.OrganizationType;
//                if (organizationType == OrganizationTypeEnum.Admin && filterDto?.OrganizationType != OrganizationTypeEnum.Admin)
//                    organizationType = filterDto.OrganizationType;

//                var actions = EnumRepository.GetActions();
//                result.Success(actions);
//            }
//            catch (Exception exception)
//            {
//                result.Error(exception);
//            }

//            return result;
//        }


//        public override async Task<IEnumerable<UserDbo>> _Create(UserDto dto)
//        {
//            var user = await _authService.GetUser();

//            if (user.OrganizationType != OrganizationTypeEnum.Admin)
//            {
//                dto.OrganizationType = user.OrganizationType;
//                dto.UserType = dto.UserType;
//                dto.OrganizationId = user.OrganizationId;
//            }

//            dto.LanguageId = await _languageService.GetDefaultLanguageId();

//            //Eğer davet maili gönderilmiş mail adresiyle kullanıcı kaydı yapılıyorsa devet kısmından o mail adresi silinir.

//            #region Invite Delete

//            var inviteDbo = await _userInviteRepository.DetailWithEmail(dto.Email);
//            if (inviteDbo != null)
//                await _userInviteRepository.Update(_userInviteMapper.ToUpdateStatus(inviteDbo, StatusEnum.Deleted));

//            #endregion

//            var userDbo = await _repository.Create(_mapper.ToCreate(dto));

//            return new List<UserDbo> { userDbo };
//        }

//        public override async Task<bool> _Update(UserDbo dbo, UserDto dto)
//        {
//            //Eğer davet maili gönderilmiş mail adresiyle kullanıcı kaydı yapılıyorsa devet kısmından o mail adresi silinir.

//            #region Invite Delete

//            var inviteDbo = await _userInviteRepository.DetailWithEmail(dto.Email);
//            if (inviteDbo != null)
//                await _userInviteRepository.Update(_userInviteMapper.ToUpdateStatus(inviteDbo, StatusEnum.Deleted));

//            #endregion

//            var newDbo = await _repository.Update(_mapper.ToUpdate(dbo, dto));

//            //Create log
//            CreateLog(ActionTypeEnum.Update, newDbo.GetLogProps(), dbo.GetLogProps());
//            return newDbo;
//        }

//        public override async Task<(IEnumerable<UserListDto> Data, PaginationDto Pagination)> _List(
//            UserFilterDto filterDto)
//        {
//            var user = await _authService.GetUser();

//            if (user.OrganizationType != OrganizationTypeEnum.Admin)
//            {
//                filterDto.OrganizationId = user.OrganizationId;
//                filterDto.OrganizationType = user.OrganizationType;
//            }

//            var list = await _repository.List(filterDto);
//            return (_mapper.ToListDto(list.Data), list.Pagination);
//        }

//        public virtual async Task<Result<bool>> ChangeLanguage(int languageId)
//        {
//            var result = new Result<bool>();
//            try
//            {
//                var dbo = await _repository.Detail(_authService.GetUserId());

//                if (dbo == null)
//                    throw new RecordNotFoundException();

//                dbo.LanguageId = languageId;

//                result.Success(await _repository.Update(dbo));

//                await RemoveCache(ActionTypeEnum.Update, dbo);
//            }
//            catch (Exception exception)
//            {
//                result.Error(exception);
//            }

//            return result;
//        }

//        public override async Task<bool> _Delete(UserDbo dbo)
//        {
//            var user = await _authService.GetUser();

//            // the logged in user cannot delete his role.
//            if (user.UserId == dbo.Id)
//                throw new CustomException("validation.you_are_cannot_delete_your_user");

//            return await _repository.Update(_mapper.ToUpdateStatus(dbo, StatusEnum.Deleted));
//        }

//        public override async Task<List<UserDbo>> DeleteMultipleValidateOrAddProcess(List<UserDbo> dboList)
//        {
//            var user = await _authService.GetUser();

//            return dboList.Select(x =>
//            {
//                // the logged in user cannot delete his role.
//                if (user.UserId == x.Id)
//                    throw new CustomException("validation.you_are_cannot_delete_your_user");

//                return x;
//            }).ToList();
//        }

//        public override async Task<(IEnumerable<DropdownDto> Data, PaginationDto Pagination)> _Dropdown(UserFilterDto filterDto)
//        {
//            (IEnumerable<UserDbo> Data, PaginationDto Pagination) list;
//            if (filterDto.AddActiveWorkOrderCount)
//                list = await _repository.List(filterDto, x => x.Include(y => y.TransportsIDrive));
//            if (filterDto.AddActiveDeliveryWorkOrderCount)
//                list = await _repository.List(filterDto, x => x.Include(y => y.PersonnelDeliveries));
//            else
//                list = await _repository.List(filterDto);

//            return (_mapper.ToDropdown(list.Data, filterDto.AddActiveWorkOrderCount, filterDto.AddActiveDeliveryWorkOrderCount), list.Pagination);
//        }

//        public override Task<bool> _UpdateStatus(UserDbo dbo, StatusEnum status)
//        {
//            var userId = _authService.GetUserId();
//            if (dbo.Id == userId)
//                throw new CustomException("validation.cannot_change_your_status");
//            return base._UpdateStatus(dbo, status);
//        }

//        public async Task<Result<bool>> ChangePassword(string currentPassword, string newPassword)
//        {
//            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
//                throw new CustomException("validation.password_cannot_be_empty");

//            var result = new Result<bool>();
//            try
//            {
//                var userId = _authService.GetUserId();
//                var dbo = await _repository.Detail(userId);

//                if (dbo?.Password != CryptographyExtension.ComputeSha256Hash(currentPassword))
//                    throw new CustomException("validation.current_password_is_not_correct");

//                dbo.Password = CryptographyExtension.ComputeSha256Hash(newPassword);
//                dbo.PasswordChangeDate = DateTime.Now;
//                result.Success(await _repository.Update(dbo));
//            }
//            catch (Exception exception)
//            {
//                result.Error(exception);
//            }

//            return result;
//        }
//    }
//}
