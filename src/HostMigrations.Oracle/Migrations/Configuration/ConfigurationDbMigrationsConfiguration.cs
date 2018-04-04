using IdentityServer4.EF6.DbContexts;
using System.Data.Entity;
using System.Data.Entity.Migrations;

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