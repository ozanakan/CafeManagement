using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Table;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CafeOrderManager.Service.Table;

namespace CafeOrderManager.Api.Controllers
{
    public class TableController : BaseController<TableService, TableFilterDto, TableDto, TableDbo, TableListDto>
    {
        public TableController(TableService service) : base(service)
        {

        }

        [AllowAnonymous]
        [HttpPost("TableStatusUpdate")]
        public async Task<IActionResult> OrderStatusUpdate([FromBody] TableDto model)
        {
            var result = await _service.TableStatusUpdate(model);
            return GetResult(result);
        }

    }
}