using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Web.Host.Controllers
{
    //[AbpMvcAuthorize]
    public class DataSetItemsController : AbpODataEntityController<DatasetItem, Guid>, ITransientDependency
    {
        public DataSetItemsController(IRepository<DatasetItem, Guid> repository)
            : base(repository)
        {
        }
    }
}
