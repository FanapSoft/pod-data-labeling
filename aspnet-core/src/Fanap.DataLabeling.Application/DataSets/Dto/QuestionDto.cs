using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Collections.Generic;

namespace Fanap.DataLabeling.DataSets
{
    public class QuestionDto
    {
        public AnswerType AnswerType { get; set; }
        public string Title { get; set; }
        public List<AnswerOptionDto> Options { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Src { get; set; }
    }
}
