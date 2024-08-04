using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<OTContext>
{
    public OTContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var builder = new DbContextOptionsBuilder<OTContext>()
            .UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("VeniceApi"));
        return new OTContext(builder.Options);
    }
}
