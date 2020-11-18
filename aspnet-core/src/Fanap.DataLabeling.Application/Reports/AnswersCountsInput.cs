using System;

namespace Fanap.DataLabeling.Reports
{
    public class AnswersCountsInput
    {
        public long? UserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? DataSetId { get; set; }
    }
}
