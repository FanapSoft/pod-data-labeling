using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.DataSets;
using Fanap.DataLabeling.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Targets
{
    [AutoMap(typeof(TargetDefinition))]
    public class TargetDefinitionDto: FullAuditedEntityDto<Guid>
    {
        public TargetType Type { get; set; }
        public Guid? DataSetId { get; set; }
        public DatasetDto DataSet { get; set; }

        public int AnswerCount { get; set; }
        public int GoldenCount { get; set; }

        public double UMin { get; set; }
        public double UMax { get; set; }
        public double T { get; set; }
        public double BonusTrue { get; set; }
        public double BonusFalse { get; set; }

    }
}
