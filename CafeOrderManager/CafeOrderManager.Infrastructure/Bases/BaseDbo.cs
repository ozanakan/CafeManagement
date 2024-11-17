using System;
using System.ComponentModel.DataAnnotations.Schema;
using CafeOrderManager.Infrastructure.Attributes;
using CafeOrderManager.Infrastructure.Enums;

namespace CafeOrderManager.Infrastructure.Bases
{
    public class BaseDbo
    {
        public BaseDbo()
        {
            this.CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// Tablonun Id'si (Primary Key)
        /// </summary>
        [Log(DataTypeEnum.Numeric)]
        public int Id { get; set; }

        /// <summary>
        /// Kaydın Durumu
        /// </summary>
        [Column(TypeName = "int2")]
        [Log(DataTypeEnum.Enum)]
        public StatusEnum Status { get; set; } = StatusEnum.Active;

        /// <summary>
        /// Kayıt Tarihi
        /// </summary>
        [Column(TypeName = "timestamptz")]
        [Log(DataTypeEnum.Datetime)]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Kaydı Yapan Kullanıcının Id'si
        /// </summary>
        [Log(DataTypeEnum.Numeric)]
        public int? CreatedUserId { get; set; }
    }
}
