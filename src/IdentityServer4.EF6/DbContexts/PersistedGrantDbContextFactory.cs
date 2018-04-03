using IdentityServer4.EF6.DbContexts;
using IdentityServer4.EF6.Options;
using System.Data.Entity.Infrastructure;

namespace IdentityServer4.EF6.DbContexts
{
    internal class PersistedGrantDbContextFactory : IDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext Create()
        {
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityServer4.EF6;trusted_connection=yes;";
            return new PersistedGrantDbContext(connectionString, new OperationalStoreOptions());
        }
    }
}