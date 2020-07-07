using System.Collections.Generic;
using Abp.Configuration;

namespace Fanap.DataLabeling.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                
                ,new SettingDefinition(AppSettingNames.PodUri, "https://accounts.pod.ir/oauth2", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodApiToken, "f669f5d0f6da47b7b78602b59c3f93c7", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodApiBaseAddress, "https://api.pod.ir/srv/core", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodClientId, "7115red6f47fbb526d2abb79db84a", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodClientSecret, "ea54a2e5", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                
                
                ,new SettingDefinition(AppSettingNames.AuthenticationAudience, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationClockSkew, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationIssuer, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationLifeTime, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationSecretKey, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }
    }
}
