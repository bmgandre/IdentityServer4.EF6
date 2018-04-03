using System.Data.Entity;

namespace Host
{
    public class HostDbConfiguration : DbConfiguration
    {
        public HostDbConfiguration()
        {
            SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory());
            SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }
}