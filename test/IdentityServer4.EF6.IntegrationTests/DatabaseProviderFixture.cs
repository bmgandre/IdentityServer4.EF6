// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IdentityServer4.EF6.IntegrationTests
{
    /// <summary>
    /// xUnit ClassFixture for creating and deleting integration test databases.
    /// </summary>
    /// <typeparam name="T">DbContext of Type T</typeparam>
    public class DatabaseProviderFixture<T> : IDisposable where T : DbContext
    {
        public object StoreOptions;
        public List<DbContextOptions<T>> Options = new List<DbContextOptions<T>>();

        public DatabaseProviderFixture()
        {
        }
        
        public void Dispose()
        {
            foreach (var option in Options.ToList())
            {
                using (var context = (T)Activator.CreateInstance(typeof(T), option.ConnectionString, StoreOptions))
                {
                    context.Database.Delete();
                }
            }
        }
    }
}