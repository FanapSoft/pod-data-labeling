using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class GetUserProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonProperty("cellphone")]
        public string CellphoneNumber { get; set; }

        [JsonProperty("nationalCode")]
        public string NationalCode { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("agent")]
        public Agent Agent { get; set; }

        [JsonProperty("sheba")]
        public string Sheba { get; set; }

        [JsonProperty("guilds")]
        public Guild[] Guilds { get; set; }


        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("image")]
        public string LogoImage { get; set; }


    }
}
