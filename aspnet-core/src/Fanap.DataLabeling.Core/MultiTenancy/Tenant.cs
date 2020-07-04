using Abp.MultiTenancy;
using Fanap.DataLabeling.Authorization.Users;

namespace Fanap.DataLabeling.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
