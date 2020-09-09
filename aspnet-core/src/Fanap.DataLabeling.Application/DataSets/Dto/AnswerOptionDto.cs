using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Datasets;
using System;

namespace Fanap.DataLabeling.DataSets
{
    [AutoMap(typeof(AnswerOption))]
    public class AnswerOptionDto : FullAuditedEntityDto<Guid>
    {
        public AnswerType Type { get; set; }
        public string Title { get; set; }
        public string Src { get; set; }
        public int Index { get; set; }
        public Guid DataSetId { get; set; }
    }
}
