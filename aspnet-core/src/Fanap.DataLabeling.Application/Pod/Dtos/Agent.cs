using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public partial class Agent
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("cellphoneNumber")]
        public string CellphoneNumber { get; set; }
    }
}