using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Fanap.DataLabeling.Authorization;

namespace Fanap.DataLabeling
{
    [DependsOn(
        typeof(DataLabelingCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class DataLabelingApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<DataLabelingAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(DataLabelingApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
