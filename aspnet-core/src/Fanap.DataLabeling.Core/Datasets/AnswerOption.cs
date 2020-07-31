using Abp.Domain.Entities.Auditing;
using System;

namespace Fanap.DataLabeling.Datasets
{
    public class AnswerOption: FullAuditedEntity<Guid>
    {
        public AnswerType Type { get; set; }
        public string Title { get; set; }
        public string Src { get; set; }
        public int Index { get; set; }
        public Dataset DataSet { get; set; }
        public Guid DataSetId { get; set; }
    }
}
