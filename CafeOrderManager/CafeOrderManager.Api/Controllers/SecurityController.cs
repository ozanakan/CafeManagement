using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Extensions;
using CafeOrderManager.Model.Dbo;
using CafeOrderManager.Model.Dto.User;
using CafeOrderManager.Service.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeOrderManager.Api.Controllers
{
    public class SecurityController : BaseController<SecurityService, UserFilterDto, UserDto, UserDbo, UserListDto>
    {
        public SecurityController(SecurityService service) : base(service)
        {
        }

        // POST: security/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserFilterDto filterDto)
        {
            var result = await _service.Login(filterDto);
            return result.EnsureSucces() ? Ok(result) : NotFound(result);
        }

        // GET: security/userinfo
        [HttpGet("userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            var result = await _service.UserInfo();
            return result.EnsureSucces() ? Ok(result) : NotFound(result);
        }

    
    }
}