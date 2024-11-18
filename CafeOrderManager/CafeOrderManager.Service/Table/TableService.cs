using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Table;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
using System;
using System.Threading.Tasks;

namespace CafeOrderManager.Service.Table
{

    public class TableService : BaseService<TableRepository, TableMapper, TableDbo, TableDto, TableListDto, TableFilterDto>
    {
        public TableService(IAuthService authService, TableRepository repository, TableMapper mapper) : base(repository, mapper, authService)
        {

        }


        public async Task<Result<bool>> TableStatusUpdate(TableDto model)
        {
            var result = new Result<bool>();

            try
            {
                // 1. Sipariş detaylarını getir
                var dbo = await _repository.Detail((int)model.Id);
                if (dbo == null)
                {
                    // Sipariş bulunamazsa hata dön
                    result.Error(new Exception("Masa bulunamadı"));
                    return result;
                }

                // Sipariş durumunu güncelle
                dbo.TableStatus = model.TableStatus;
                var updateResult = await _repository.Update(dbo);


                // İşlem başarılı
                result.Success(true);

            }
            catch (Exception exception)
            {
                result.Error(exception);
            }

            return result;
        }


    }
}