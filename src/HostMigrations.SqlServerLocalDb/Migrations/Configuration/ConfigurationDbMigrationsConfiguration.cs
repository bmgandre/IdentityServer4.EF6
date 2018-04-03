using System.Data.Entity.Migrations;
using IdentityServer4.EF6.DbContexts;

namespace HostMigrations.SqlServerLocalDb.Configuration
{
    public class ConfigurationDbMigrationsConfiguration : DbMigrationsConfiguration<ConfigurationDbContext>
    {
        public ConfigurationDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Configuration";
            MigrationsDirectory = "Configuration";
        }
    }
}
