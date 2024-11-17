using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.Table;

namespace CafeOrderManager.Service.Table
{
    public class TableMapper : BaseMapper<TableDbo, TableDto, TableListDto>
    {
        public TableMapper(IAuthService authService) : base(authService)
        {
        }

        public override TableListDto ToListDto(TableDbo dbo)
        {
            var dto = base.ToListDto(dbo);
            dto.TableStatus = dbo.TableStatus;
            dto.TableNumber = dbo.TableNumber;
            return dto;
        }

        public override TableDto ToDto(TableDbo dbo)
        {
            var dto = base.ToDto(dbo);
            dto.TableStatus = dbo.TableStatus;
            dto.TableNumber = dbo.TableNumber;
            return dto;
        }


        public override TableDbo ToCreate(TableDto dto)
        {
            var dbo = base.ToCreate(dto);
            dbo.TableStatus = dto.TableStatus;
            dbo.TableNumber = dto.TableNumber;
            return dbo;
        }

        public override TableDbo ToUpdate(TableDbo dbo, TableDto dto)
        {
            dbo = base.ToUpdate(dbo, dto);
            dbo.TableStatus = dto.TableStatus;
            dbo.TableNumber = dto.TableNumber;

            return dbo;
        }
        public override DropdownDto ToDropdown(TableDbo dbo)
        {
            var dto = base.ToDropdown(dbo);
            dto.Name = dbo.TableNumber;
            return dto;
        }

    }
}
