using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;

namespace Fanap.DataLabeling.DataSets
{
    public class QuestionDto
    {
        public bool TargetEnded { get; set; }
        public bool G { get; set; }
        public Guid DatasetItemId { get; set; }
        public AnswerType AnswerType { get; set; }
        public string Title { get; set; }
        public List<AnswerOptionDto> Options { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionSubjectFileSrc { get; set; }
        public string QuestionFileSrc { get; set; }
    }
}
