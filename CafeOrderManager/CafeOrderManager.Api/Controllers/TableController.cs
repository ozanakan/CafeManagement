using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Table;
using CafeOrderManager.Service.Table;

namespace CafeOrderManager.Api.Controllers
{
    public class TableController : BaseController<TableService, TableFilterDto, TableDto, TableDbo, TableListDto>
    {
        public TableController(TableService service) : base(service)
        {

        }


    }
}