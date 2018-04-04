using IdentityServer4.EF6.DbContexts;
using IdentityServer4.EF6.Interfaces;
using IdentityServer4.EF6.Options;
using IdentityServer4.EF6.Services;
using IdentityServer4.EF6.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using System;
using System.Data.Entity;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EF database support to IdentityServer.
    /// </summary>
    public static class ConfigurationStoreIdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddConfigurationStoreUsingAppSettings(this IIdentityServerBuilder builder)
        {
            var configuration = OptionSettingsReader.GetConfigurationRoot();
            var connectionString = OptionSettingsReader.GetConnectionString(configuration);
            var options = OptionSettingsReader.GetConfigurationStoreOptions(configuration);

            builder.AddConfigurationStore(options, (_) => new ConfigurationDbContext(connectionString, options));

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStoreWithDefaultOptions(
            this IIdentityServerBuilder builder,
            Func<ConfigurationStoreOptions, ConfigurationDbContext> createDbContextFunc)
        {
            return builder.AddConfigurationStore(new ConfigurationStoreOptions(), createDbContextFunc);
        }

        public static IIdentityServerBuilder AddConfigurationStore<TContext>(
            this IIdentityServerBuilder builder,
            ConfigurationStoreOptions options,
            Func<ConfigurationStoreOptions, TContext> createDbContextFunc)
            where TContext : DbContext, IConfigurationDbContext
        {
            if (createDbContextFunc == null)
            {
                throw new ArgumentNullException(nameof(createDbContextFunc));
            }

            builder.Services.AddSingleton(options);
            builder.Services.AddScoped<IConfigurationDbContext>((_) => createDbContextFunc(options));
            builder.RegisterConfigurationStores();

            return builder;
        }

        public static IIdentityServerBuilder RegisterConfigurationStores(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            return builder;
        }

        /// <summary>
        /// Configures caching for IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfigurationStoreCache(
            this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();
            builder.AddCorsPolicyCache<CorsPolicyService>();

            return builder;
        }
    }
}