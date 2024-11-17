using System;
using System.Collections.Generic;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.User;
using System.Linq;

namespace CafeOrderManager.Service.User
{
    public class UserMapper : BaseMapper<UserDbo, UserDto, UserListDto>
    {
      
        public UserMapper(IAuthService authService) : base(authService)
        {
         
        }

        public override UserListDto ToListDto(UserDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.Name = dbo.Name;
            dto.Surname = dbo.Surname;
          
            //dto.Phone = dbo.Phone.AddPhoneMask();
            dto.Email = dbo.Email;
            dto.LoginId = dbo.LoginId;
          
            return dto;
        }

        public override UserDto ToDto(UserDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.Name = dbo.Name;
            dto.Surname = dbo.Surname;
            dto.FullName = dbo.Name + " " + dbo.Surname;
            dto.LoginId = dbo.LoginId;
            //dto.Phone = dbo.Phone.AddPhoneMask();
            dto.Email = dbo.Email;
          
            return dto;
        }

        public override UserDbo ToCreate(UserDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.Name = dto.Name;
            dbo.Surname = dto.Surname;
            dbo.LoginId = dto.LoginId;
            if (dto.Password != null)
                dbo.Password = CryptographyExtension.ComputeSha256Hash(dto.Password);
            dbo.Email = dto.Email;
            //dbo.Phone = dto.Phone.ClearPhoneMask();
          
            return dbo;
        }

        public override UserDbo ToUpdate(UserDbo dbo, UserDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.Name = dto.Name;
            dbo.Surname = dto.Surname;
            dbo.LoginId = dto.LoginId;
            if (dto.Password != null)
                dbo.Password = CryptographyExtension.ComputeSha256Hash(dto.Password);
            dbo.Email = dto.Email;
            //dbo.Phone = dto.Phone.ClearPhoneMask();
            return dbo;
        }

        //public virtual IEnumerable<DropdownDto>
        //    ToDropdown(IEnumerable<UserDbo> list, bool addActiveWorkOrderCount = false, bool addActiveDeliveryWorkOrderCount = false) =>
        //    (from item in list select ToDropdown(item, addActiveWorkOrderCount, addActiveDeliveryWorkOrderCount));

        //public DropdownDto ToDropdown(UserDbo dbo, bool addActiveWorkOrderCount = false, bool addActiveDeliveryWorkOrderCount = false)
        //{
        //    var dto = base.ToDropdown(dbo);
        //    dto.Name = dbo.Name + " " + dbo.Surname;
        //    dto.Value = dbo.Id.ToString();
        //    if (addActiveWorkOrderCount)
        //    {
        //        dto.Data = new
        //        {
        //            activeWorkOrderCount = dbo.TransportsIDrive
        //                ?.Count(x => x.TransportationProcessType != TransportationProcessTypeEnum.Completed && x.Status != StatusEnum.Deleted)
        //        };
        //    }
        //    if (addActiveDeliveryWorkOrderCount)
        //    {
        //        var count = dbo.PersonnelDeliveries
        //            ?.Count(x => x.DeliveryStatus != DeliveryStatusEnum.Completed && x.Status != StatusEnum.Deleted);
        //        dto.Data = new
        //        {
        //            activeWorkOrderCount = count ?? 0
        //        };
        //    }

        //    return dto;
        //}
    }
}