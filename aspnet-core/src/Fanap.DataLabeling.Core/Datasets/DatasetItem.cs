using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
using System.Text;

namespace Fanap.DataLabeling.Datasets
{
    public enum DatasetItemType
    {
        None = 0,
        TextContent = 1,
        File = 2

    }
    public class DatasetItem : FullAuditedEntity<Guid>
    {

        [MaxLength(1000)]
        [Required]
        public string Name { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public DatasetItemType Type { get; set; }
    }
}
