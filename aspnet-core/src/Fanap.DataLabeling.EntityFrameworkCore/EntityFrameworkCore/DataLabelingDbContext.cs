﻿using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Fanap.DataLabeling.Authorization.Roles;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.MultiTenancy;
using Fanap.DataLabeling.Datasets;
using Fanap.DataLabeling.Labels;
using Fanap.DataLabeling.Pod;

namespace Fanap.DataLabeling.EntityFrameworkCore
{
    public class DataLabelingDbContext : AbpZeroDbContext<Tenant, Role, User, DataLabelingDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<ExternalToken> ExternalTokens { get; set; }
        public DbSet<DatasetItem> DatasetItems { get; set; }        
        public DbSet<Label> Labels { get; set; }        
        public DbSet<DatasetItemLabel> DatasetItemLabels { get; set; }        
        public DataLabelingDbContext(DbContextOptions<DataLabelingDbContext> options)
            : base(options)
        {
        }
    }
}
