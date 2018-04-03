using System.Data.Entity;

namespace IdentityServer4.EF6.IntegrationTests
{
    public class DbContextOptions<TContext>
        where TContext : DbContext
    {
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
    }
}
