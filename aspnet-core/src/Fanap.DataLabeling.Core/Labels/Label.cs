using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Labels
{
    public class Label : FullAuditedEntity<Guid>
    {
        public string Name { get; set; }
        public Dataset Dataset { get; set; }
    }
}
