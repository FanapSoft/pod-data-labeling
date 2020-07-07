using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class UserAndBusiness
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("guilds")]
        public Guild[] Guilds { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("latitude")]
        public long Latitude { get; set; }

        [JsonProperty("longitude")]
        public long Longitude { get; set; }

        [JsonProperty("subscriptionCount")]
        public long SubscriptionCount { get; set; }

        [JsonProperty("subscribed")]
        public bool Subscribed { get; set; }

        [JsonProperty("numOfComments")]
        public long NumOfComments { get; set; }

        [JsonProperty("rate")]
        public Rate Rate { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("ssoId")]
        public long SsoId { get; set; }

        [JsonProperty("numOfProducts")]
        public long NumOfProducts { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; }

        [JsonProperty("tags")]
        public object[] Tags { get; set; }

        [JsonProperty("tagTrees")]
        public object[] TagTrees { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("apiToken")]
        public string ApiToken { get; set; }

        [JsonProperty("agent")]
        public Agent Agent { get; set; }

        [JsonProperty("numOfLike")]
        public long NumOfLike { get; set; }

        [JsonProperty("numOfDislike")]
        public long NumOfDislike { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        public int BusinessType { get; set; }
        public string Sheba { get; set; }
    }
}