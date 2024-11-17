using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Payment;
using CafeOrderManager.Service.Payment;

namespace CafeOrderManager.Api.Controllers
{
    public class PaymentController : BaseController<PaymentService, PaymentFilterDto, PaymentDto, PaymentDbo, PaymentListDto>
    {
        public PaymentController(PaymentService service) : base(service)
        {

        }


    }
}