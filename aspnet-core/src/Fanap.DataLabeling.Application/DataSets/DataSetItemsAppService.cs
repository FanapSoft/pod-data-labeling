using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public class DataSetItemsGetAllInput : PagedAndSortedResultRequestDto
    {
        public Guid? DataSetId { get; set; }

    }
    public interface IDataSetItemsAppService : IAsyncCrudAppService<DataSetItemDto, Guid, DataSetItemsGetAllInput>
    {
    }
    [AbpAuthorize]
    public class DataSetItemsAppService : AsyncCrudAppService<DatasetItem, DataSetItemDto, Guid, DataSetItemsGetAllInput>, IDataSetItemsAppService
    {

        public DataSetItemsAppService(IRepository<DatasetItem, Guid> repository) : base(repository)
        {

        }
        protected override IQueryable<DatasetItem> CreateFilteredQuery(DataSetItemsGetAllInput input)
        {
            return base.CreateFilteredQuery(input)
                .WhereIf(input.DataSetId != null, ff => ff.DatasetID == input.DataSetId);
        }
    }
}
