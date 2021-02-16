using Newtonsoft.Json;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class Wallet
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
