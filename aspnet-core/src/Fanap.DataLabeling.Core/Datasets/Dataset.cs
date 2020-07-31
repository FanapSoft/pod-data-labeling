using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Labels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fanap.DataLabeling.Datasets
{
    public class Dataset : FullAuditedEntity<Guid>
    {
        public bool IsActive { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public QuestionType QuestionType { get; set; }
        public AnswerType AnswerType { get; set; }
        public DatasetType Type { get; set; }
        public LabelingStatus LabelingStatus { get; set; }
        public ICollection<Label> PermittedLabels { get; set; }
        public string FieldName { get; set; }
        /// <summary>
        /// Is this {{Label.Title}}? 
        /// </summary>
        public string QuestionTemplate { get; set; }
        public string QuestionSrc { get; set; }
        public ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}
