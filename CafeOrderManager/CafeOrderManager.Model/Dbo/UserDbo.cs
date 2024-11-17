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
        public string Name { get; set; } //user name

        [StringLength(30)]
        [Required]
        [Log(DataTypeEnum.String)]
        public string Surname { get; set; } //user surname

        [StringLength(20)]
        [Required]
        [Log(DataTypeEnum.String)]
        public string LoginId { get; set; } //user login id (username)

        [StringLength(64)]
        [Log(DataTypeEnum.String)]
        [Required]
        public string Password { get; set; } //user password

        [StringLength(100)]
        [Log(DataTypeEnum.String)]
        public string PasswordKey { get; set; } //Code sent because you forgot your password (verified) 

        [StringLength(50)]
        [Log(DataTypeEnum.String)]
        public string Email { get; set; } //email

        [Log(DataTypeEnum.String)]
        [StringLength(20)]
        public string Phone { get; set; }  //Phone


        public virtual UserDbo CreatedUser { get; set; }



        #region CreatedUser References

        //User
        public virtual ICollection<UserDbo> CreatedUserUsers { get; set; }

        #endregion
    }
}