using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.DataSets
{
    [AutoMap(typeof(Dataset))]
    public class DatasetDto : FullAuditedEntityDto<Guid>
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public DatasetType Type { get; set; }
        public LabelingStatus LabelingStatus { get; set; }
        public ICollection<LabelDto> PermittedLabels { get; set; }
    }
}
