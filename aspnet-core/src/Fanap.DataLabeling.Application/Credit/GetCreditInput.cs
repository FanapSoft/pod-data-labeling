using System;
using System.ComponentModel.DataAnnotations;

namespace Fanap.DataLabeling.Credit
{
    public class GetCreditInput
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public Guid DataSetId { get; set; }
    }
}