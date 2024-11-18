using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Payment;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CafeOrderManager.Service.Payment
{

    public class PaymentService : BaseService<PaymentRepository, PaymentMapper, PaymentDbo, PaymentDto, PaymentListDto, PaymentFilterDto>
    {
        private readonly OrderRepository _orderRepository;
        public PaymentService(IAuthService authService, PaymentRepository repository, PaymentMapper mapper, OrderRepository orderRepository) : base(repository, mapper, authService)
        {
            _orderRepository = orderRepository;
        }


        public async override Task<IEnumerable<PaymentDbo>> _Create(PaymentDto dto)
        {
            //Ödeme yapıldığında siparişin durumunu güncelliyoruz.
            if (dto.PaymentStatus == Infrastructure.Enums.PaymentStatusEnum.Paid)
            {
                var order = await _orderRepository.Detail(dto.OrderId);
                order.OrderStatus = Infrastructure.Enums.OrderStatusEnum.Completed;
                await _orderRepository.Update(order);
            }

            return await base._Create(dto);
        }



    }
}