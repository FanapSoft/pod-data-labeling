﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.DataSets
{
    [AutoMap(typeof(Dataset))]
    public class DatasetDto : FullAuditedEntityDto<Guid>
    {
        public int AnswerReplicationCount { get; set; }
        public int AnswerBudgetCountPerUser { get; set; }
        public decimal UMin { get; set; }
        public decimal UMax { get; set; }
        public decimal T { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public QuestionType QuestionType { get; set; }
        public AnswerType AnswerType { get; set; }
        public DatasetType Type { get; set; }
        public LabelingStatus LabelingStatus { get; set; }
        public ICollection<LabelDto> PermittedLabels { get; set; }
        public string FieldName { get; set; }
        /// <summary>
        /// Is this {{Label.Title}}? 
        /// </summary>
        public string QuestionTemplate { get; set; }
        public string QuestionSrc { get; set; }
        public ICollection<AnswerOptionDto> AnswerOptions { get; set; }
    }
}
