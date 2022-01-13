using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Data.EF
{

    public class RecruimentWebsiteDbContextFactory : IDesignTimeDbContextFactory<RecruimentWebsiteDbContext>
    {
        public RecruimentWebsiteDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("RecruimentWebsite");

            var optionsBuilder = new DbContextOptionsBuilder<RecruimentWebsiteDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new RecruimentWebsiteDbContext(optionsBuilder.Options);
        }
    }
}
