using IdentityServer4.EF6.Options;
using System.Data.Entity.Infrastructure;

namespace IdentityServer4.EF6.DbContexts
{
    public class ConfigurationDbContextFactory : IDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext Create()
        {
            var configuration = OptionSettingsReader.GetConfigurationRoot();
            var connectionString = OptionSettingsReader.GetConnectionString(configuration);
            var options = OptionSettingsReader.GetConfigurationStoreOptions(configuration);
            return new ConfigurationDbContext(connectionString, options);
        }
    }
}