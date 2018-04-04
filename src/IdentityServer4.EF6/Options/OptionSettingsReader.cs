using IdentityServer4.EF6.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IdentityServer4.EF6.Options
{
    public class OptionSettingsReader
    {
        public static IConfigurationRoot GetConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("is4.ef6.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            return configuration;
        }

        public static string GetConnectionString(IConfigurationRoot configuration)
        {
            const string defaultConnection = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityServer4.EF6;trusted_connection=yes;";
            var connection = configuration.GetSection("Data:ConnectionString").Value;
            return string.IsNullOrEmpty(connection) ? defaultConnection : connection;
        }

        public static string GetDefaultSchema(IConfigurationRoot configuration)
        {
            var schema = configuration.GetValue<string>("Data:StoreOptions:DefaultSchema");
            return schema;
        }

        private class StoreSettingsEntry
        {
            public string Entity { get; set; }
            public string Table { get; set; }
            public string Schema { get; set; }
        }

        public static ConfigurationStoreOptions GetConfigurationStoreOptions(IConfigurationRoot configuration)
        {
            var defaultSchema = GetDefaultSchema(configuration);

            var entityTypeList = from type in Assembly.GetExecutingAssembly().GetTypes()
                                 where type.IsClass
                                    && type.Namespace == typeof(Client).Namespace
                                    && !type.IsAbstract
                                 select type;

            var storeSettingsEntryList = new List<StoreSettingsEntry>();
            foreach (var type in entityTypeList)
            {
                var entry = new StoreSettingsEntry
                {
                    Entity = type.GetTypeInfo().Name,
                    Table = configuration.GetValue<string>($"Data:StoreOptions:{type.GetTypeInfo().Name}:Table"),
                    Schema = configuration.GetValue<string>($"Data:StoreOptions:{type.GetTypeInfo().Name}:Schema")
                };
                storeSettingsEntryList.Add(entry);
            }

            var options = new ConfigurationStoreOptions
            {
                DefaultSchema = defaultSchema,
                IdentityResource = CreateTableConfiguration(storeSettingsEntryList, nameof(IdentityResource), defaultSchema),
                IdentityClaim = CreateTableConfiguration(storeSettingsEntryList, nameof(IdentityClaim), defaultSchema),
                ApiResource = CreateTableConfiguration(storeSettingsEntryList, nameof(ApiResource), defaultSchema),
                ApiClaim = CreateTableConfiguration(storeSettingsEntryList, nameof(ApiResourceClaim), defaultSchema),
                ApiSecret = CreateTableConfiguration(storeSettingsEntryList, nameof(ApiSecret), defaultSchema),
                ApiScope = CreateTableConfiguration(storeSettingsEntryList, nameof(ApiScope), defaultSchema),
                ApiScopeClaim = CreateTableConfiguration(storeSettingsEntryList, nameof(ApiScopeClaim), defaultSchema),
                Client = CreateTableConfiguration(storeSettingsEntryList, nameof(Client), defaultSchema),
                ClientGrantType = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientGrantType), defaultSchema),
                ClientRedirectUri = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientRedirectUri), defaultSchema),
                ClientPostLogoutRedirectUri = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientPostLogoutRedirectUri), defaultSchema),
                ClientScopes = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientScope), defaultSchema),
                ClientSecret = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientSecret), defaultSchema),
                ClientClaim = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientClaim), defaultSchema),
                ClientIdPRestriction = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientIdPRestriction), defaultSchema),
                ClientCorsOrigin = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientCorsOrigin), defaultSchema),
                ClientProperty = CreateTableConfiguration(storeSettingsEntryList, nameof(ClientProperty), defaultSchema),
            };

            return options;
        }

        public static OperationalStoreOptions GetOperationalStoreOptions(IConfigurationRoot configuration)
        {
            var schema = GetDefaultSchema(configuration);

            var persistedGrantTable = configuration.GetValue<string>($"Data:StoreOptions:{nameof(PersistedGrant)}:Table");
            var persistedGrantSchema = configuration.GetValue<string>($"Data:StoreOptions:{nameof(PersistedGrant)}:Schema");
            var enableTokenCleanup = configuration.GetValue<bool?>($"Data:StoreOptions:{nameof(OperationalStoreOptions.EnableTokenCleanup)}");
            var tokenCleanupInterval = configuration.GetValue<int?>($"Data:StoreOptions:{nameof(OperationalStoreOptions.TokenCleanupInterval)}");
            var tokenCleanupBatchSize = configuration.GetValue<int?>($"Data:StoreOptions:{nameof(OperationalStoreOptions.TokenCleanupBatchSize)}");

            var operationalStoreOptions = new OperationalStoreOptions
            {
                DefaultSchema = schema,
                PersistedGrants = new TableConfiguration(TableName(persistedGrantTable, nameof(PersistedGrant)), SchemaName(persistedGrantSchema, schema)),
                EnableTokenCleanup = enableTokenCleanup ?? false,
                TokenCleanupBatchSize = tokenCleanupBatchSize ?? 3600,
                TokenCleanupInterval = tokenCleanupInterval ?? 100
            };

            return operationalStoreOptions;
        }

        private static TableConfiguration CreateTableConfiguration(IEnumerable<StoreSettingsEntry> entryList, string entityName, string defaultSchema)
        {
            var storeSettings = entryList.First(x => x.Entity == entityName);
            var table = TableName(storeSettings.Table, entityName);
            var schema = SchemaName(storeSettings.Schema, defaultSchema);
            var configuration = new TableConfiguration(table, schema);
            return configuration;
        }

        private static string GetDefaultIfStringIsNullOrEmpty(string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        private static string TableName(string configName, string defaultName)
        {
            return GetDefaultIfStringIsNullOrEmpty(configName, defaultName);
        }

        private static string SchemaName(string configName, string defaultSchema)
        {
            return GetDefaultIfStringIsNullOrEmpty(configName, defaultSchema);
        }
    }
}