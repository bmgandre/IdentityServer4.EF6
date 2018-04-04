using IdentityServer4.EF6.DbContexts;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace HostMigrations.Oracle.Migrations.PersistedGrant
{
    public class PersistedGrantDbMigrationsConfiguration : DbMigrationsConfiguration<PersistedGrantDbContext>
    {
        static PersistedGrantDbMigrationsConfiguration()
        {
            DbConfiguration.SetConfiguration(new HostDbConfiguration());
        }

        public PersistedGrantDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "PersistedGrant";
            MigrationsDirectory = "PersistedGrant";
        }
    }
}