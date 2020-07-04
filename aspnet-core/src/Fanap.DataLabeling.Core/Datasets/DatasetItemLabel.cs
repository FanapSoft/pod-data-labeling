using Abp.Domain.Entities;
using Fanap.DataLabeling.Labels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Datasets
{
    public class DatasetItemLabel: Entity
    {
        public Label Label { get; set; }
        public DatasetItem DatasetItem { get; set; }
    }
}
