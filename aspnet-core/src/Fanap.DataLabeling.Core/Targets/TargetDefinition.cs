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
        public long OwnerId { get; set; }
        public User Owner { get; set; }

        public int AnswerCount { get; set; }

    }
}
