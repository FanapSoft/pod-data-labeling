
using Newtonsoft.Json;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class RelatedUser
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("nationalCode")]
        public string NationalCode { get; set; }

        [JsonProperty("cellPhoneNumber")]
        public string CellPhoneNumber { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
