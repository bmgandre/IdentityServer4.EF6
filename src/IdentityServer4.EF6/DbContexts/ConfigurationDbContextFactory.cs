using IdentityServer4.EF6.Options;
using System.Data.Entity.Infrastructure;

namespace IdentityServer4.EF6.DbContexts
{
    public class ConfigurationDbContextFactory : IDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext Create()
        {
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityServer4.EF6;trusted_connection=yes;";
            return new ConfigurationDbContext(connectionString, new ConfigurationStoreOptions());
        }
    }
}
