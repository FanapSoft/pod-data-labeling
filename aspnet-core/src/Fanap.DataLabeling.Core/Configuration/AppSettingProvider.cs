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
                ,new SettingDefinition(AppSettingNames.PodApiToken, "96bdf9f117e74738a00cd0da34f6c18e", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodApiBaseAddress, "https://api.pod.ir/srv/core", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodClientId, "18035391i35f649cbac6ab4d4c9bf45fb", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.PodClientSecret, "66fe1d81", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                
                
                ,new SettingDefinition(AppSettingNames.AuthenticationAudience, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationClockSkew, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationIssuer, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationLifeTime, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
                ,new SettingDefinition(AppSettingNames.AuthenticationSecretKey, "", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }
    }
}
