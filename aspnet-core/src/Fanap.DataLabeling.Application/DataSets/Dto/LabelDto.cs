using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Labels;
using System;

namespace Fanap.DataLabeling.DataSets
{
    [AutoMap(typeof(Label))]
    public class LabelDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid? DatasetId { get; set; }

    }
}
