using System;

namespace Fanap.DataLabeling.Reports
{
    public class AnswersCountsOutput
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public long UserId { get; set; }
        public long Count { get; set; }
        public DateTime Date { get; set; }
    }
}
