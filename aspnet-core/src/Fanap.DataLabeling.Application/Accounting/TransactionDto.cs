using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fanap.DataLabeling.DataSets;
using Fanap.DataLabeling.Users.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Accounting
{
    [AutoMap(typeof(Transaction))]
    public class TransactionDto: FullAuditedEntityDto<Guid>
    {

        public long OwnerId { get; set; }
        public UserDto Owner { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public TransactionReason Reason { get; set; }
        public string ReasonDescription { get; set; }
        public Guid? ReferenceDataSetId { get; set; }
        public DatasetDto ReferenceDataSet { get; set; }
    }
}
