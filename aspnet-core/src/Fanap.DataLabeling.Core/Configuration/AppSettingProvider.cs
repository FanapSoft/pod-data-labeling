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
                
                ,new SettingDefinition(AppSettingNames.PodUri, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodApiToken, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodApiBaseAddress, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodClientId, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodClientSecret, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                
                
                ,new SettingDefinition(AppSettingNames.AuthenticationAudience, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationClockSkew, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationIssuer, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationLifeTime, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationSecretKey, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }
    }
}
