using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Pod.Dtos
{
    public class ContactDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("cellphoneNumber")]
        public string CellPhoneNumber { get; set; }

        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("creationDate")]
        public string CreationDate { get; set; }

        [JsonProperty("lastModifyDate")]
        public string LastModifyDate { get; set; }
    }
}
