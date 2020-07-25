using Abp.Application.Services;
using System;

namespace Fanap.DataLabeling.DataSets
{
    public interface IDataSetsAppService : IAsyncCrudAppService<DatasetDto, Guid>
    {


    }
}
