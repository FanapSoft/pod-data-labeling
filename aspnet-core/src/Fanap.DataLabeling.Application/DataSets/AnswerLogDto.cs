using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Datasets;
using System;

namespace Fanap.DataLabeling.DataSets
{
    [AutoMap(typeof(AnswerLog))]
    public class AnswerLogDto: AuditedEntityDto<Guid>
    {
        public bool Ignored { get; set; }
        public string IgnoreReason { get; set; }
        public Guid DataSetId { get; set; }
        public Guid DataSetItemId { get; set; }
        public int Answer { get; set; }
        public string QuestionObject { get; set; }
    }
}
