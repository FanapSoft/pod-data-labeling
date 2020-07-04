using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class GetBusinessApiToken
    {
        [JsonProperty("apiToken")]
        public string ApiToken { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }
    }
}