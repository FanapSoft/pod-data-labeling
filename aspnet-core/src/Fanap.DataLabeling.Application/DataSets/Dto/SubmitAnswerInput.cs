using System;

namespace Fanap.DataLabeling.DataSets
{
    public class SubmitAnswerInput
    {
        public bool Ignored { get; set; }
        public string IgnoreReason { get; set; }
        public Guid DataSetId { get; set; }
        public Guid DataSetItemId { get; set; }
        public int AnswerIndex { get; set; }
        public string QuestionObject { get; set; }
        public long DurationToAnswerInSeconds { get; set; }
    }
}
