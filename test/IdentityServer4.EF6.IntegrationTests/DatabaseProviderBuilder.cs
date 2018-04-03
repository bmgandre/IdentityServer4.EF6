// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Data.Entity;

namespace IdentityServer4.EF6.IntegrationTests
{
    /// <summary>
    /// Helper methods to initialize DbContextOptions for the specified database provider and context.
    /// </summary>
    public class DatabaseProviderBuilder
    {
        public static DbContextOptions<T> BuildLocalDb<T>(string name) where T : DbContext
        {
            var settings = new DbContextOptions<T>
            {
                Provider = "System.Data.SqlClient",
                ConnectionString = $@"Data Source=(LocalDb)\MSSQLLocalDB;database=Test.IdentityServer4.EF6-2.0.0.{name};trusted_connection=yes;"
            };
            return settings;
        }
    }
}