using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Datasets;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.DataSets
{
    [AutoMap(typeof(DatasetItem))]
    public class DataSetItemDto: FullAuditedEntityDto<Guid>
    {
        [MaxLength(1000)]
        public string Name { get; set; }
        public string Content { get; set; }
        [MaxLength(1000)]
        public string FilePath { get; set; }
        [MaxLength(10)]
        public string FileExtension { get; set; }
        [MaxLength(1000)]
        public string FileName { get; set; }
        public long FileSize { get; set; }
        [MaxLength(1000)]
        public bool IsGoldenData { get; set; }
        public DatasetItemType Type { get; set; }
        public LabelDto FinalLabel { get; set; }
        public Guid? FinalLabelId { get; set; }
        public LabelDto Label { get; set; }
        public Guid? LabelId { get; set; }

        public DatasetDto Dataset { get; set; }
        public Guid DatasetID { get; set; }

    }
}
