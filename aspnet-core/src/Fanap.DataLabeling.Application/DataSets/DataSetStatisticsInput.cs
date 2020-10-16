using Fanap.DataLabeling.Datasets;
using System;

namespace Fanap.DataLabeling.DataSets
{
    public class DataSetStatisticsInput
    {
        public LabelingStatus? LabelingStatus { get; set; }
        public long? UserId { get; set; }
    }
}