using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Targets
{
    public class TargetDefinition : FullAuditedEntity<Guid>
    {
        public TargetType Type { get; set; }
        public Guid? DataSetId { get; set; }
        public Datasets.Dataset DataSet { get; set; }
        public int AnswerCount { get; set; }
        public int GoldenCount { get; set; }
        public double UMin { get; set; }
        public double UMax { get; set; }
        public double T { get; set; }
        public double BonusTrue { get; set; }
        public double BonusFalse { get; set; }

    }
}
