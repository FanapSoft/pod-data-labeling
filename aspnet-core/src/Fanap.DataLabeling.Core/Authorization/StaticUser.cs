using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Authorization
{
    public class StaticUser: FullAuditedEntity
    {
        public string PodUserName { get; set; }
        public bool IsAdmin { get; set; }
    }
}
