using System;
using System.Collections.Generic;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Infrastructure.Models
{
    public class AuthUserDto
    {
        public int UserId { get; set; }
        public string NameSurname { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? PasswordChangeDate { get; set; }
    }
}