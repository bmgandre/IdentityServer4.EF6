using System.Data.Entity;
using System.Data.Entity.Migrations;
using IdentityServer4.EF6.DbContexts;

namespace HostMigrations.Oracle.Migrations.Configuration
{
    public class ConfigurationDbMigrationsConfiguration : DbMigrationsConfiguration<ConfigurationDbContext>
    {
        static ConfigurationDbMigrationsConfiguration()
        {
            DbConfiguration.SetConfiguration(new HostDbConfiguration());
        }

        public ConfigurationDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Configuration";
            MigrationsDirectory = "Configuration";
        }
    }
}
