using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System;

namespace CafeOrderManager.Model.Dto.User
{
    public class UserListDto : BaseListDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string LoginId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}