using System.Data.Entity;
using System.Data.Entity.Migrations;
using IdentityServer4.EF6.DbContexts;

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
