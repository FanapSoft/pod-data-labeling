using System;

namespace Fanap.DataLabeling.Credit
{
    public class GetCreditInput
    {
        public long UserId { get; set; }
        public Guid DataSetId { get; set; }
    }
}