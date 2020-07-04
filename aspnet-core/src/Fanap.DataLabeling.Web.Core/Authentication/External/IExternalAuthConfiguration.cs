using System.Collections.Generic;

namespace Fanap.DataLabeling.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
