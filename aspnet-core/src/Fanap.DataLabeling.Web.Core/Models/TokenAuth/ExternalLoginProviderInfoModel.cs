using Abp.AutoMapper;
using Fanap.DataLabeling.Authentication.External;

namespace Fanap.DataLabeling.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
