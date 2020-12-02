using Abp.Domain.Entities.Auditing;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Datasets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Accounting
{
    public enum TransactionReason
    {
        None = 0,
        Promotion = 1,
        CollectCredit = 2,
        Withdraw = 3
    }
    public class Transaction : FullAuditedEntity<Guid>
    {
        public long OwnerId { get; set; }
        public User Owner { get; set; }
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public TransactionReason Reason { get; set; }
        public string ReasonDescription { get; set; }
        public Guid? ReferenceDataSetId { get; set; }
        public Dataset ReferenceDataSet { get; set; }

    }
}
