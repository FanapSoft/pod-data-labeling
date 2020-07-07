using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class PodResultLog
    {
        [JsonProperty("hasError")]
        public bool? HasError { get; set; }

        [JsonProperty("referenceNumber")]
        public long? ReferenceNumber { get; set; }

        public override string ToString()
        {
            return $"{nameof(HasError)} : {HasError}, {nameof(ReferenceNumber)} : {ReferenceNumber}";
        }
    }

    public class PodResult
    {
        [JsonProperty("hasError")]
        public bool HasError { get; set; }

        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("errorCode")]
        public long ErrorCode { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("ott")]
        public string Ott { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
    
    public class PodResult<TResult> : PodResult
    {
        [JsonProperty("result")]        
        public TResult Result { get; set; }
    }
}