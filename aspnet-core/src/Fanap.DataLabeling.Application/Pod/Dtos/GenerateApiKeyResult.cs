using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanap.DataLabeling.Pod.Dtos
{


    public class GenerateApiKeyResult
    {

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

    }
}
