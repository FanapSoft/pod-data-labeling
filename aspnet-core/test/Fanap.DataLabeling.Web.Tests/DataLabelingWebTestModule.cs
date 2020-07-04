using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Fanap.DataLabeling.EntityFrameworkCore;
using Fanap.DataLabeling.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Fanap.DataLabeling.Web.Tests
{
    [DependsOn(
        typeof(DataLabelingWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class DataLabelingWebTestModule : AbpModule
    {
        public DataLabelingWebTestModule(DataLabelingEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(DataLabelingWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(DataLabelingWebMvcModule).Assembly);
        }
    }
}