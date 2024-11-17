using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Payment;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
namespace CafeOrderManager.Service.Payment
{

    public class PaymentService : BaseService<PaymentRepository, PaymentMapper, PaymentDbo, PaymentDto, PaymentListDto, PaymentFilterDto>
    {
        public PaymentService(IAuthService authService, PaymentRepository repository, PaymentMapper mapper) : base(repository, mapper, authService)
        {

        }

    }
}