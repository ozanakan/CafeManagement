using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CafeOrderManager.Infrastructure.Enums;
using CafeOrderManager.Infrastructure.Exceptions;
using CafeOrderManager.Infrastructure.Interfaces;
using CafeOrderManager.Infrastructure.Models;
using CafeOrderManager.Model.Dto.User;
//using CafeOrderManager.Service.Shared;
using CafeOrderManager.Storage.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace CafeOrderManager.Service.Auth
{
    public class AuthService : IAuthService
    {
        public readonly static string JWT_VERSION = "v1";
        public readonly static string JWT_ISSUER = "CafeOrderManager";
        public readonly static string JWT_AUDIENCE = "CafeOrderManager_Users";
        private readonly IHttpContextAccessor _context;
        private readonly UserRepository _userRepository;
        //private readonly NoAuthService _noAuthService;
        private IMemoryCache _memoryCache;

        public AuthService(IMemoryCache memoryCache, UserRepository userRepository,            IHttpContextAccessor context)
        {
            _userRepository = userRepository;
            //_noAuthService = noAuthService;
            _memoryCache = memoryCache;
            _context = context;
        }

        public string CreateAndWriteToken(int userId, bool isMobile,
            string JwtSecurityKey)
        {
            var claims = new[]
            {
                new Claim("tkn_ver", JWT_VERSION),
                new Claim("tkn_id", Guid.NewGuid().ToString()),
                new Claim("usr_id", userId.ToString()),
                new Claim("is_mbl", isMobile.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                JWT_ISSUER,
                JWT_AUDIENCE,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserId()
        {
            if (!IsLogin()) throw new UnAuthorizedException();


            var cp = claimsPrincipal;

            if (!cp.Identity.AuthenticationType.Contains("Federation"))
            {
                throw new UnAuthorizedException();
            }

            return int.Parse(cp.Claims.FirstOrDefault(i => i.Type == "usr_id").Value);
        }

        public async Task<AuthUserDto> GetUser()
        {
            if (!IsLogin()) return null;
            var userId = GetUserId();

            AuthUserDto user = null;
            if (!_memoryCache.TryGetValue(userId, out user))
            {
                //Get User Info From DB

                var dbo = await _userRepository.Detail(userId, false);

                if (dbo == null)
                    return null;

                user = new AuthUserDto
                {
                    UserId = dbo.Id,
                    NameSurname = dbo.Name + " " + dbo.Surname,
                    Status = dbo.Status,
                    //PasswordChangeDate = dbo.PasswordChangeDate
                };

                _memoryCache.Set(userId, user,
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60)));
            }

            return user;
        }


        public async System.Threading.Tasks.Task<Result<bool>>
            CanUserTakeAction() // kullanıcı her hangi bir action çalıştırabilir mi
        {
            var result = new Result<bool>();
            try
            {
                if (!IsLogin()) // kullanıcı giriş yapmamış ise kontrol AccessControl sistemi tarafından yapılacak
                    return result.Success(true);

                var canTakeAction = await _userRepository.Any(new UserFilterDto()
                {
                    Id = GetUserId(),
                    Status = StatusEnum.Active
                });

                return result.Success(canTakeAction);
            }
            catch (Exception e)
            {
                result.Error(e);
                Console.WriteLine(e);
            }

            return result;
        }

        public bool IsLogin()
        {
            return claimsPrincipal?.Identity?.IsAuthenticated == true;
        }

        private ClaimsPrincipal claimsPrincipal
        {
            get { return _context.HttpContext?.User; }
        }

        //public async Task<bool> HasAccess(params ActionEnum[] actions)
        //{
        //    var usr = await GetUser();

        //    if (usr?.Status != StatusEnum.Active)
        //        throw new UserNotActiveException();

        //    var userActions = usr.Actions;


        //    if (userActions == null || !userActions.Any())
        //        return false;

        //    return actions.All(f => userActions.Contains(f));
        //}

        //public async Task<bool> HasAccessAny(params ActionEnum[] actions)
        //{
        //    var usr = await GetUser();

        //    if (usr?.Status != StatusEnum.Active)
        //        throw new UserNotActiveException();

        //    var userActions = usr.Actions;


        //    if (userActions == null || !userActions.Any())
        //        return false;

        //    return actions.Any(f => userActions.Contains(f));
        //}

        //public async Task<bool> RemoveCache(int? userId = null, int? roleId = null)
        //{
        //    var filter = new UserFilterDto()
        //    {
        //        Id = userId,
        //        RoleId = roleId,
        //        StatusList = null
        //    };

        //    var userList = (await _userRepository.List(filter)).Data;

        //    foreach (var user in userList)
        //        _memoryCache.Remove(user.Id);

        //    return true;
        //}
    }
}