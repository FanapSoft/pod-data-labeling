using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Labels;
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
        [MaxLength(1000)]
        public string FilePath { get; set; }
        [MaxLength(10)]
        public string FileExtension { get; set; }
        [MaxLength(1000)]
        public string FileName { get; set; }
        public long FileSize { get; set; }
        [MaxLength(1000)]
        public bool IsGoldenData { get; set; }
        public DatasetItemType Type { get; set; }
        public Label FinalLabel { get; set; }
        public Guid? FinalLabelId { get; set; }
        public Label Label { get; set; }
        public Guid? LabelId { get; set; }

        public Dataset Dataset { get; set; }
        public Guid DatasetID { get; set; }
    }
}
