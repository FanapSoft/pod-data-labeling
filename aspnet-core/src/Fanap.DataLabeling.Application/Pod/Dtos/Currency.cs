using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class Currency
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)} : {Name}, {nameof(Code)} : {Code}";
        }
    }
}