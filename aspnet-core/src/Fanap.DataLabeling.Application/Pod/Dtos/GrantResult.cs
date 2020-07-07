using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class GrantResult
    {
        [JsonProperty("grants")]
        public Grant[] Grants { get; set; }

    }

    public partial class Grant
    {
        [JsonProperty("clientDescription")]
        public string ClientDescription { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("clientUrl")]
        public string ClientUrl { get; set; }

        [JsonProperty("pendingPermittedUsers")]
        public PermittedUser[] PendingPermittedUsers { get; set; }

        [JsonProperty("permittedUsers")]
        public PermittedUser[] PermittedUsers { get; set; }

        [JsonProperty("scopes")]
        public string[] Scopes { get; set; }
    }

    public partial class PermittedUser
    {
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }
    }






    public class GrantData
    {
        [JsonProperty("grants")]
        public List<pendingPermittedUsers> PendingPermittedUsers { get; set; }
    }


    public class pendingPermittedUsers
    {
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }


}
