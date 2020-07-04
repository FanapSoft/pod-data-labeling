using Abp.Authorization;
using Fanap.DataLabeling.Authorization.Roles;
using Fanap.DataLabeling.Authorization.Users;

namespace Fanap.DataLabeling.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
