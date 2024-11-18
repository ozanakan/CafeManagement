using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CafeOrderManager.Infrastructure.Attributes;

namespace CafeOrderManager.Model.Dbo
{
    [Table("User")]
    public class UserDbo : BaseDbo
    {
        [StringLength(30)]
        [Required]
        [Log(DataTypeEnum.String)]
        public string Name { get; set; } 

        [StringLength(30)]
        [Required]
        [Log(DataTypeEnum.String)]
        public string Surname { get; set; } 

        [StringLength(20)]
        [Required]
        [Log(DataTypeEnum.String)]
        public string LoginId { get; set; } 

        [StringLength(64)]
        [Log(DataTypeEnum.String)]
        [Required]
        public string Password { get; set; } 

        [StringLength(100)]
        [Log(DataTypeEnum.String)]
        public string PasswordKey { get; set; }

        [StringLength(50)]
        [Log(DataTypeEnum.String)]
        public string Email { get; set; } 

        [Log(DataTypeEnum.String)]
        [StringLength(20)]
        public string Phone { get; set; }  


        public virtual UserDbo CreatedUser { get; set; }

        public virtual ICollection<UserDbo> CreatedUserUsers { get; set; }

    }
}