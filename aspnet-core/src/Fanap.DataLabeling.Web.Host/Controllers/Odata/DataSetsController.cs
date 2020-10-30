using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.OData.Controllers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Web.Host.Controllers
{
    //[AbpMvcAuthorize]
    public class DataSetsController : AbpODataEntityController<Dataset, Guid>, ITransientDependency
    {
        public DataSetsController(IRepository<Dataset, Guid> repository)
            : base(repository)
        {
        }
    }
}
