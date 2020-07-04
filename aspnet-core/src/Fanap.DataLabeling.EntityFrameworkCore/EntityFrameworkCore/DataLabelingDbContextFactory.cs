using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Fanap.DataLabeling.Configuration;
using Fanap.DataLabeling.Web;

namespace Fanap.DataLabeling.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class DataLabelingDbContextFactory : IDesignTimeDbContextFactory<DataLabelingDbContext>
    {
        public DataLabelingDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DataLabelingDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DataLabelingDbContextConfigurer.Configure(builder, configuration.GetConnectionString(DataLabelingConsts.ConnectionStringName));

            return new DataLabelingDbContext(builder.Options);
        }
    }
}
