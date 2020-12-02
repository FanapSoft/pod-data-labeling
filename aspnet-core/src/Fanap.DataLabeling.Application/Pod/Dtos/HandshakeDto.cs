using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class UserDto {
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
        [JsonProperty("given_name")]
        public string GivenName { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("preferred_username")]
        public string PreferredUsername { get; set; }
    }
    public class HandshakeDto
    {
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }
        [JsonProperty("keyFormat")]
        public string KeyFormat { get; set; }
        [JsonProperty("keyId")]
        public string KeyId { get; set; }
        [JsonProperty("privateKey")]
        public string PrivateKey { get; set; }
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }
        [JsonProperty("user")]
        public UserDto User { get; set; }

    }
}
