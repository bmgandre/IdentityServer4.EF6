// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.EF6;
using IdentityServer4.EF6.DbContexts;
using IdentityServer4.EF6.Interfaces;
using IdentityServer4.EF6.Options;
using IdentityServer4.EF6.Stores;
using IdentityServer4.Stores;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EF database support to IdentityServer.
    /// </summary>
    public static class OperationalStoreIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddOperationalStoreUsingAppSettings(this IIdentityServerBuilder builder)
        {
            var configuration = OptionSettingsReader.GetConfigurationRoot();
            var connectionString = OptionSettingsReader.GetConnectionString(configuration);
            var options = OptionSettingsReader.GetOperationalStoreOptions(configuration);

            builder.AddOperationalStore(options, (_) => new PersistedGrantDbContext(connectionString, options));
            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStoreWithDefaultOptions(
            this IIdentityServerBuilder builder,
            Func<OperationalStoreOptions, PersistedGrantDbContext> createDbContext)
        {
            return builder.AddOperationalStore(new OperationalStoreOptions(), createDbContext);
        }

        public static IIdentityServerBuilder AddOperationalStore<TContext>(
            this IIdentityServerBuilder builder,
            OperationalStoreOptions storeOptions,
            Func<OperationalStoreOptions, TContext> createDbContextFunc)
            where TContext : DbContext, IPersistedGrantDbContext
        {
            if (createDbContextFunc == null)
            {
                throw new ArgumentNullException(nameof(createDbContextFunc));
            }

            builder.Services.AddSingleton(storeOptions);
            builder.Services.AddScoped<IPersistedGrantDbContext>((_) => createDbContextFunc(storeOptions));
            builder.RegisterOperationalStores();

            return builder;
        }

        public static IIdentityServerBuilder RegisterOperationalStores(this IIdentityServerBuilder builder)
        {
            builder.Services.AddSingleton<TokenCleanup>();
            builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();
            builder.Services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();

            return builder;
        }

        private class TokenCleanupHost : IHostedService
        {
            private readonly TokenCleanup _tokenCleanup;
            private readonly OperationalStoreOptions _options;

            public TokenCleanupHost(TokenCleanup tokenCleanup, OperationalStoreOptions options)
            {
                _tokenCleanup = tokenCleanup;
                _options = options;
            }

            public Task StartAsync(CancellationToken cancellationToken)
            {
                if (_options.EnableTokenCleanup)
                {
                    _tokenCleanup.Start(cancellationToken);
                }
                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                if (_options.EnableTokenCleanup)
                {
                    _tokenCleanup.Stop();
                }
                return Task.CompletedTask;
            }
        }
    }
}