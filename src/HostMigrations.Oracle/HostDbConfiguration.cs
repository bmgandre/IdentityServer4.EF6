using System.Data.Entity;
using System.Data.Entity.Migrations.History;
using IdentityServer4.EF6.Options;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;

namespace HostMigrations.Oracle
{
    public class HostDbConfiguration : DbConfiguration
    {
        public HostDbConfiguration()
        {
            SetProviderServices("Oracle.ManagedDataAccess.Client", EFOracleProviderServices.Instance);
            SetProviderFactory("Oracle.ManagedDataAccess.Client", OracleClientFactory.Instance);

            var configuration = OptionSettingsReader.GetConfigurationRoot();
            var schema = OptionSettingsReader.GetDefaultSchema(configuration);
            SetDefaultHistoryContext((connection, _) => new HistoryContext(connection, schema));
        }
    }
}
