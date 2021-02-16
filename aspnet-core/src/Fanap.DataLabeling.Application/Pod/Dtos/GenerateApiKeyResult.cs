using Newtonsoft.Json;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class GenerateApiKeyResult
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }
    }
}
