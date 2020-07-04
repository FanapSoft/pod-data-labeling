using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Fanap.DataLabeling.Configuration;

namespace Fanap.DataLabeling.Web.Host.Startup
{
    [DependsOn(
       typeof(DataLabelingWebCoreModule))]
    public class DataLabelingWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public DataLabelingWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DataLabelingWebHostModule).GetAssembly());
        }
    }
}
