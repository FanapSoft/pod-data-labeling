using Newtonsoft.Json;

namespace Fanap.DataLabeling.Clients.Pod.Dtos
{
    public class UserProfileInfo
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nickName")]
        public string NickName { get; set; }

        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("ssoId")]
        public string ssoId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; } 
        
        [JsonProperty("cellphoneNumber")]
        public string CellphoneNumber { get; set; }


        public override string ToString()
        {
            return $"{nameof(Username)} : {Username}, {nameof(Email)} : {Email}, {nameof(CellphoneNumber)} : {CellphoneNumber}";
        }
    }
}
