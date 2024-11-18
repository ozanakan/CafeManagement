using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Models;

namespace CafeOrderManager.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthUserDto> GetUser();
        public int GetUserId();
        public bool IsLogin();
        public string CreateAndWriteToken(int userId, bool isMobile, string JwtSecurityKey);
    }
}