using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class TransferFundToContactDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("wallet")]
        public string Wallet { get; set; }

        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("destContact")]
        public ContactDto DestContact { get; set; }
    }
}
