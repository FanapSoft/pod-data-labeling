using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Labels;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace Fanap.DataLabeling.Datasets
{
    public class AnswerLog: AuditedEntity<Guid>
    {
        public bool CreditCalculated { get; set; }
        public bool Ignored { get; set; }
        public string IgnoreReason { get; set; }
        public Dataset DataSet { get; set; }
        public Guid DataSetId { get; set; }
        public DatasetItem DataSetItem { get; set; }
        public Guid DataSetItemId { get; set; }
        public int Answer { get; set; }
        public string QuestionObject { get; set; }
        public Label DeterminedLabel { get; set; }
        public Guid? DeterminedLabelId { get; set; }
        public long DurationToAnswerInSeconds { get; set; }
    }
}
