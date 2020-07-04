using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Fanap.DataLabeling.Authorization.Roles;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.MultiTenancy;
using Fanap.DataLabeling.Datasets;
using Fanap.DataLabeling.Labels;

namespace Fanap.DataLabeling.EntityFrameworkCore
{
    public class DataLabelingDbContext : AbpZeroDbContext<Tenant, Role, User, DataLabelingDbContext>
    {
        /* Define a DbSet for each entity of the application */
        DbSet<Dataset> Datasets { get; set; }        
        DbSet<DatasetItem> DatasetItems { get; set; }        
        DbSet<Label> Labels { get; set; }        
        DbSet<DatasetItemLabel> DatasetItemLabels { get; set; }        
        public DataLabelingDbContext(DbContextOptions<DataLabelingDbContext> options)
            : base(options)
        {
        }
    }
}
