using System.Data.Entity;
using System.Data.SqlClient;

namespace IdentityServer4.EF6.IntegrationTests
{
    public class IntegrationTestDbConfiguration : DbConfiguration
    {
        public IntegrationTestDbConfiguration()
        {
            SetProviderFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }
}