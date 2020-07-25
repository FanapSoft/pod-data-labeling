using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Labels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fanap.DataLabeling.Datasets
{
    public enum DatasetType
    {
        None = 0,
        Csv = 1,
        Excel = 2,
        Images = 3
    }
    public enum LabelingStatus
    {
        None = 0,
        Started = 1,
        Finished = 2
    }
    public class Dataset : FullAuditedEntity<Guid>
    {
        public bool IsActive { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public DatasetType Type { get; set; }
        public LabelingStatus LabelingStatus { get; set; }
        public ICollection<Label> PermittedLabels { get; set; }
        public string FieldName { get; set; }

    }
}
