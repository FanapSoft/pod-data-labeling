using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Fanap.DataLabeling.Configuration;
using Fanap.DataLabeling.EntityFrameworkCore;
using Fanap.DataLabeling.Migrator.DependencyInjection;

namespace Fanap.DataLabeling.Migrator
{
    [DependsOn(typeof(DataLabelingEntityFrameworkModule))]
    public class DataLabelingMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public DataLabelingMigratorModule(DataLabelingEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(DataLabelingMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                DataLabelingConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DataLabelingMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
