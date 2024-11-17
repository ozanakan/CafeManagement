using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Payment;

namespace CafeOrderManager.Service.Payment
{
    public class PaymentMapper : BaseMapper<PaymentDbo, PaymentDto, PaymentListDto>
    {
        public PaymentMapper(IAuthService authService) : base(authService)
        {
        }

        public override PaymentListDto ToListDto(PaymentDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.OrderId = dbo.OrderId;
            dto.Amount= dbo.Amount;
            dto.PaymentDate = dbo.PaymentDate;
            dto.PaymentStatus = dbo.PaymentStatus;
            return dto;
        }

        public override PaymentDto ToDto(PaymentDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.OrderId = dbo.OrderId;
            dto.Amount = dbo.Amount;
            dto.PaymentDate = dbo.PaymentDate;
            dto.PaymentStatus = dbo.PaymentStatus;
            return dto;
        }


        public override PaymentDbo ToCreate(PaymentDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.OrderId = dto.OrderId;
            dbo.Amount = dto.Amount;
            dbo.PaymentDate = dto.PaymentDate;
            dbo.PaymentStatus = dto.PaymentStatus;
            return dbo;
        }

        public override PaymentDbo ToUpdate(PaymentDbo dbo, PaymentDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.OrderId = dto.OrderId;
            dbo.Amount = dto.Amount;
            dbo.PaymentDate = dto.PaymentDate;
            dbo.PaymentStatus = dto.PaymentStatus;
            return dbo;
        }

    }
}
