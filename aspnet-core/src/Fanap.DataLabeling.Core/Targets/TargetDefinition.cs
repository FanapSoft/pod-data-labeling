﻿using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Targets
{
    public class TargetDefinition : FullAuditedEntity<Guid>
    {
        public TargetType Type { get; set; }
        public long OwnerId { get; set; }
        public User Owner { get; set; }
        public Guid? DataSetId { get; set; }
        public Datasets.Dataset DataSet { get; set; }
        public int AnswerCount { get; set; }

    }
}
