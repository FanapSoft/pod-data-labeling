using Fanap.DataLabeling.Clients.Pod.Dtos;
using Newtonsoft.Json;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class TransferToContact
    {
        [JsonProperty("causeType")]
        public string CauseType { get; set; }

        [JsonProperty("causeId")]
        public string CauseId { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("creationDate")]
        public int CreationDate { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("needVerify")]
        public bool NeedVerify { get; set; }

        [JsonProperty("currency")]
        public Currency Currency { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("wallet")]
        public string Wallet { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("destContact")]
        public ContactDto DestContact { get; set; }

        [JsonProperty("walletSrv")]
        public Wallet WalletSrv { get; set; }
    }
}

