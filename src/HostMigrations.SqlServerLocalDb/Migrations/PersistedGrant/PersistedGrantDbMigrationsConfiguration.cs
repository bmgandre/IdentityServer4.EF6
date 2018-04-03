using IdentityServer4.EF6.DbContexts;
using System.Data.Entity.Migrations;

namespace HostMigrations.SqlServerLocalDb.PersistedGrant
{
    public class PersistedGrantDbMigrationsConfiguration : DbMigrationsConfiguration<PersistedGrantDbContext>
    {
        public PersistedGrantDbMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "PersistedGrant";
            MigrationsDirectory = "PersistedGrant";
        }
    }
}