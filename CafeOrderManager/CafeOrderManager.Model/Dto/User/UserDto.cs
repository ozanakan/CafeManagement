using System;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Model.Dto.User
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string PasswordKey { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? PasswordChangeDate { get; set; } //Last Password Change Date
        public string FullName { get; set; }
      
    }
}