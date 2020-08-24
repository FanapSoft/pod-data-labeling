using Abp.Application.Services;
using System;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public interface IDataSetsAppService : IAsyncCrudAppService<DatasetDto, Guid>
    {
        Task<ImportOutput> Import(ImportInput input);
    }
}
