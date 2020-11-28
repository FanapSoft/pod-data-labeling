using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Targets
{
    public class UserTarget: FullAuditedEntity<Guid>
    {
        public long OwnerId { get; set; }
        public User Owner { get; set; }
        public Guid TargetDefinitionId { get; set; }
        public TargetDefinition TargetDefinition { get; set; }
    }
}
