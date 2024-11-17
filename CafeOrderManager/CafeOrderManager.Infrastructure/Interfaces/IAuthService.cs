using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Bases;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthUserDto> GetUser();
        public int GetUserId();
        public bool IsLogin();
        public string CreateAndWriteToken(int userId, bool isMobile, string JwtSecurityKey);
        //public Task<bool> HasAccess(params ActionEnum[] actions);
        //public Task<bool> HasAccessAny(params ActionEnum[] actions);
        //public Task<bool> RemoveCache(int? userId = null, int? roleId = null);
    }
}