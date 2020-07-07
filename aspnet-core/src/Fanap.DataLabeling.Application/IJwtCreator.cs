
using Abp.Dependency;
using Fanap.DataLabeling.Authorization.Users;
using Microsoft.IdentityModel.Tokens;

namespace Fanap.DataLabeling.Jwt
{
    public interface IJwtCreator: ITransientDependency
    {
        string Create(User user, string accessToken ,string clientId);
        TokenValidationParameters GetTokenValidationParameters();
    }
}