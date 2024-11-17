using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Model.Dbo;
using System;
using System.Collections.Generic;

namespace CafeOrderManager.Model.Dto.User
{
    public class UserFilterDto : BaseFilterDto<UserDbo>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LoginId { get; set; }
        public string LoginIdEquals { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}