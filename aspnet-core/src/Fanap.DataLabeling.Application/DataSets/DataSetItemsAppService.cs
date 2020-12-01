using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public class DataSetItemsGetAllInput : PagedAndSortedResultRequestDto
    {
        public string LabelName { get; set; }
        public Guid? DataSetId { get; set; }
        public bool? IsGoldenData { get; set; } 
        public bool? OnlyNonDecidedGoldens { get; set; }
    }
    public interface IDataSetItemsAppService : IAsyncCrudAppService<DataSetItemDto, Guid, DataSetItemsGetAllInput>
    {
        Task<DataSetItemDto> SetGolden(EntityDto<Guid> dto);
    }
    [AbpAuthorize]
    public class DataSetItemsAppService : AsyncCrudAppService<DatasetItem, DataSetItemDto, Guid, DataSetItemsGetAllInput>, IDataSetItemsAppService
    {

        public DataSetItemsAppService(IRepository<DatasetItem, Guid> repository) : base(repository)
        {

        }
        protected override IQueryable<DatasetItem> CreateFilteredQuery(DataSetItemsGetAllInput input)
        {
            return base.CreateFilteredQuery(input).Include(ff => ff.Label)
                .WhereIf(input.IsGoldenData != null, ff => ff.IsGoldenData)
                .WhereIf(input.OnlyNonDecidedGoldens != null, ff => ff.ConfirmedGoldenData == null)
                .WhereIf(!input.LabelName.IsNullOrWhiteSpace(), ff => ff.Label.Name.Contains(input.LabelName))
                .WhereIf(input.DataSetId != null, ff => ff.DatasetID == input.DataSetId);
        }

        public async Task<DataSetItemDto> SetGolden(EntityDto<Guid> dto)
        {
            var found = await GetAsync(dto);
            found.IsGoldenData = true;
            var result = await UpdateAsync(found);
            return result;
        }
    }
}
