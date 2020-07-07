using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public partial class Rate
    {
        [JsonProperty("rate")]
        public long RateRate { get; set; }

        [JsonProperty("rateCount")]
        public long RateCount { get; set; }
    }
}