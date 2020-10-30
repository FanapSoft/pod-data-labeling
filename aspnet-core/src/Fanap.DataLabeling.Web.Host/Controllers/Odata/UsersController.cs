using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.OData.Controllers;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Authorization;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Web.Host.Controllers
{
    [AbpMvcAuthorize(permissions: PermissionNames.Pages_Users)]
    public class UsersController : AbpODataEntityController<User, long>, ITransientDependency
    {
        public UsersController(IRepository<User, long> repository)
            : base(repository)
        {
        }
    }
}
