using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Fanap.DataLabeling.EntityFrameworkCore
{
    public static class DataLabelingDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<DataLabelingDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<DataLabelingDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
