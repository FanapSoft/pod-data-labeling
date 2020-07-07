using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Pod.Dtos
{

    public class LoginResponseDto
    {
        public LoginResponseDto()
        {
            Language = "fa-IR";
        }

        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FristName { get; set; }

        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty("username", NullValueHandling = NullValueHandling.Ignore)]
        public string Username { get; set; }

        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        [JsonProperty("refid", NullValueHandling = NullValueHandling.Ignore)]
        public int RefId { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int? Code { get; set; }

        [JsonProperty("cellphoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string CellphoneNumber { get; set; }

        [JsonProperty("individualUserPhone", NullValueHandling = NullValueHandling.Ignore)]
        public string IndividualUserPhone { get; set; }
    }

}
