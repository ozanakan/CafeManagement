using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Table;
using CafeOrderManager.Service.Base;
using CafeOrderManager.Storage.Repositories;
namespace CafeOrderManager.Service.Table
{

    public class TableService : BaseService<TableRepository, TableMapper, TableDbo, TableDto, TableListDto, TableFilterDto>
    {
        public TableService(IAuthService authService, TableRepository repository, TableMapper mapper) : base(repository, mapper, authService)
        {

        }

    }
}