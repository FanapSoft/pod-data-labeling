using System;

namespace Fanap.DataLabeling.DataSets
{
    public class AnswerStatisticsInput
    {
        public Guid? DataSetId{ get; set; }
        public long? UserId { get; set; }
        public bool CreditCalculated { get; set; }
    }
}