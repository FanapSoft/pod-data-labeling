using System.Threading.Tasks;
using Fanap.DataLabeling.Configuration.Dto;

namespace Fanap.DataLabeling.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
