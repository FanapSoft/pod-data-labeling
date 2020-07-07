using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class BusinessSummary
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("numOfProducts")]
        public long NumOfProducts { get; set; }

        [JsonProperty("sheba")]
        public string Sheba { get; set; }
    }
}