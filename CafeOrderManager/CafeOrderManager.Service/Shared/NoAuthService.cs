//using System.Linq;
//using System.Threading.Tasks;
//using CafeOrderManager.Infrastructure.Enums;
//using CafeOrderManager.Model.Dbo;
//using Microsoft.EntityFrameworkCore;

//namespace BieksperV2.Service.Shared
//{
//    public class NoAuthService
//    {


//        public NoAuthService()
//        {
//        }

//        public IQueryable<UserDbo> AlwaysIncludeForOrganizationAndDeviceCanSee(IQueryable<UserDbo> q) => q
//            .Include(f => f.Role).Include(f => f.Organization).ThenInclude(f => f.Location);


//        public async Task<bool> HasAccess(UserDbo user, params ActionEnum[] actions)
//        {
//            if (user == null || user.Role == null || user.Role.Actions == null || !user.Role.Actions.Any() ||
//                user.Status != StatusEnum.Active)
//                return false;

//            var userActions = user.Role?.Actions;

//            return actions.All(f => userActions.Contains(f));
//        }
//    }
//}