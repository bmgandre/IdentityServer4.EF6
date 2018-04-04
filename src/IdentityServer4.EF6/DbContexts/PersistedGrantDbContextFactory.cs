using IdentityServer4.EF6.Options;
using System.Data.Entity.Infrastructure;

namespace IdentityServer4.EF6.DbContexts
{
    internal class PersistedGrantDbContextFactory : IDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext Create()
        {
            var configuration = OptionSettingsReader.GetConfigurationRoot();
            var connectionString = OptionSettingsReader.GetConnectionString(configuration);
            var storeOptions = OptionSettingsReader.GetOperationalStoreOptions(configuration);
            return new PersistedGrantDbContext(connectionString, storeOptions);
        }
    }
}