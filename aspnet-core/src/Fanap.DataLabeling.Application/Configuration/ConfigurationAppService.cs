using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Runtime.Session;
using Fanap.DataLabeling.Configuration.Dto;

namespace Fanap.DataLabeling.Configuration
{
    [AbpAuthorize]
    [RemoteService(IsEnabled = false)]
    public class ConfigurationAppService : DataLabelingAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
