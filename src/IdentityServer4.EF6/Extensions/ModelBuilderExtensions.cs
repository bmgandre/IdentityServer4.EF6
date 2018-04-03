// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.EF6.Entities;
using IdentityServer4.EF6.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace IdentityServer4.EF6.Extensions
{
    /// <summary>
    /// Extension methods to define the database schema for the configuration and operational data stores.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        private static EntityMappingConfiguration<TEntity> ToTable<TEntity>(this EntityMappingConfiguration<TEntity> entityTypeBuilder, TableConfiguration configuration)
            where TEntity : class
        {
            return string.IsNullOrWhiteSpace(configuration.Schema) ? entityTypeBuilder.ToTable(configuration.Name) : entityTypeBuilder.ToTable(configuration.Name, configuration.Schema);
        }

        /// <summary>
        /// Configures the client context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigureClientContext(this DbModelBuilder modelBuilder, ConfigurationStoreOptions storeOptions)
        {
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema)) modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<Client>().Map(client =>
            {
                client.ToTable(storeOptions.Client);

                modelBuilder.Entity<Client>().HasKey(x => x.Id);
                modelBuilder.Entity<Client>().Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<Client>().Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<Client>().Property(x => x.ClientName).HasMaxLength(200);
                modelBuilder.Entity<Client>().Property(x => x.ClientUri).HasMaxLength(2000);
                modelBuilder.Entity<Client>().Property(x => x.LogoUri).HasMaxLength(2000);
                modelBuilder.Entity<Client>().Property(x => x.Description).HasMaxLength(1000);
                modelBuilder.Entity<Client>().Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
                modelBuilder.Entity<Client>().Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
                modelBuilder.Entity<Client>().Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
                modelBuilder.Entity<Client>().Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);
                modelBuilder.Entity<Client>().HasIndex(x => x.ClientId).IsUnique();
            });

            modelBuilder.Entity<ClientGrantType>().Map(grantType =>
            {
                grantType.ToTable(storeOptions.ClientGrantType);

                modelBuilder.Entity<ClientGrantType>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientGrantType>().Property(x => x.GrantType).HasMaxLength(250).IsRequired();
                modelBuilder.Entity<ClientGrantType>().HasRequired(x => x.Client).WithMany(x => x.AllowedGrantTypes).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientGrantType>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientRedirectUri>().Map(redirectUri =>
            {
                redirectUri.ToTable(storeOptions.ClientRedirectUri);

                modelBuilder.Entity<ClientRedirectUri>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientRedirectUri>().Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();
                modelBuilder.Entity<ClientRedirectUri>().HasRequired(x => x.Client).WithMany(x => x.RedirectUris).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientRedirectUri>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>().Map(postLogoutRedirectUri =>
            {
                postLogoutRedirectUri.ToTable(storeOptions.ClientPostLogoutRedirectUri);

                modelBuilder.Entity<ClientPostLogoutRedirectUri>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientPostLogoutRedirectUri>().Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();
                modelBuilder.Entity<ClientPostLogoutRedirectUri>().HasRequired(x => x.Client).WithMany(x => x.PostLogoutRedirectUris).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientPostLogoutRedirectUri>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientScope>().Map(scope =>
            {
                scope.ToTable(storeOptions.ClientScopes);

                modelBuilder.Entity<ClientScope>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientScope>().Property(x => x.Scope).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<ClientScope>().HasRequired(x => x.Client).WithMany(x => x.AllowedScopes).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientScope>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientSecret>().Map(secret =>
            {
                secret.ToTable(storeOptions.ClientSecret);

                modelBuilder.Entity<ClientSecret>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientSecret>().Property(x => x.Value).HasMaxLength(2000).IsRequired();
                modelBuilder.Entity<ClientSecret>().Property(x => x.Type).HasMaxLength(250);
                modelBuilder.Entity<ClientSecret>().Property(x => x.Description).HasMaxLength(2000);
                modelBuilder.Entity<ClientSecret>().HasRequired(x => x.Client).WithMany(x => x.ClientSecrets).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientSecret>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientClaim>().Map(claim =>
            {
                claim.ToTable(storeOptions.ClientClaim);

                modelBuilder.Entity<ClientClaim>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientClaim>().Property(x => x.Type).HasMaxLength(250).IsRequired();
                modelBuilder.Entity<ClientClaim>().Property(x => x.Value).HasMaxLength(250).IsRequired();
                modelBuilder.Entity<ClientClaim>().HasRequired(x => x.Client).WithMany(x => x.Claims).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientClaim>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientIdPRestriction>().Map(idPRestriction =>
            {
                idPRestriction.ToTable(storeOptions.ClientIdPRestriction);

                modelBuilder.Entity<ClientIdPRestriction>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientIdPRestriction>().Property(x => x.Provider).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<ClientIdPRestriction>().HasRequired(x => x.Client).WithMany(x => x.IdentityProviderRestrictions).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientIdPRestriction>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientCorsOrigin>().Map(corsOrigin =>
            {
                corsOrigin.ToTable(storeOptions.ClientCorsOrigin);

                modelBuilder.Entity<ClientCorsOrigin>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientCorsOrigin>().Property(x => x.Origin).HasMaxLength(150).IsRequired();
                modelBuilder.Entity<ClientCorsOrigin>().HasRequired(x => x.Client).WithMany(x => x.AllowedCorsOrigins).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientCorsOrigin>().HasKey(x => new { x.Id, x.ClientId });
            });

            modelBuilder.Entity<ClientProperty>().Map(property =>
            {
                property.ToTable(storeOptions.ClientProperty);

                modelBuilder.Entity<ClientProperty>().Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                modelBuilder.Entity<ClientProperty>().Property(x => x.Key).HasMaxLength(250).IsRequired();
                modelBuilder.Entity<ClientProperty>().Property(x => x.Value).HasMaxLength(2000).IsRequired();
                modelBuilder.Entity<ClientProperty>().HasRequired(x => x.Client).WithMany(x => x.Properties).WillCascadeOnDelete(true);
                modelBuilder.Entity<ClientProperty>().HasKey(x => new { x.Id, x.ClientId });
            });
        }

        /// <summary>
        /// Configures the persisted grant context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigurePersistedGrantContext(this DbModelBuilder modelBuilder, OperationalStoreOptions storeOptions)
        {
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema)) modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<PersistedGrant>().Map(grant =>
            {
                grant.ToTable(storeOptions.PersistedGrants);

                modelBuilder.Entity<PersistedGrant>().Property(x => x.Key).HasMaxLength(200).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
                modelBuilder.Entity<PersistedGrant>().Property(x => x.Type).HasMaxLength(50).IsRequired();
                modelBuilder.Entity<PersistedGrant>().Property(x => x.SubjectId).HasMaxLength(200);
                modelBuilder.Entity<PersistedGrant>().Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<PersistedGrant>().Property(x => x.CreationTime).IsRequired();
                // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
                // apparently anything over 4K converts to nvarchar(max) on SqlServer
                modelBuilder.Entity<PersistedGrant>().Property(x => x.Data).HasMaxLength(50000).IsRequired();
                modelBuilder.Entity<PersistedGrant>().HasKey(x => x.Key);
                modelBuilder.Entity<PersistedGrant>().HasIndex(x => new { x.SubjectId, x.ClientId, x.Type });
            });
        }

        /// <summary>
        /// Configures the resources context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigureResourcesContext(this DbModelBuilder modelBuilder, ConfigurationStoreOptions storeOptions)
        {
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema)) modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<IdentityResource>().Map(identityResource =>
            {
                identityResource.ToTable(storeOptions.IdentityResource);

                modelBuilder.Entity<IdentityResource>().HasKey(x => x.Id);
                modelBuilder.Entity<IdentityResource>().Property(x => x.Name).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<IdentityResource>().Property(x => x.DisplayName).HasMaxLength(200);
                modelBuilder.Entity<IdentityResource>().Property(x => x.Description).HasMaxLength(1000);
                modelBuilder.Entity<IdentityResource>().HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<IdentityClaim>().Map(claim =>
            {
                claim.ToTable(storeOptions.IdentityClaim);

                modelBuilder.Entity<IdentityClaim>().HasKey(x => x.Id);
                modelBuilder.Entity<IdentityClaim>().Property(x => x.Type).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<IdentityClaim>().HasRequired(x => x.IdentityResource).WithMany(x => x.UserClaims).WillCascadeOnDelete(true);
            });

            modelBuilder.Entity<ApiResource>().Map(apiResource =>
            {
                apiResource.ToTable(storeOptions.ApiResource);

                modelBuilder.Entity<ApiResource>().HasKey(x => x.Id);
                modelBuilder.Entity<ApiResource>().Property(x => x.Name).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<ApiResource>().Property(x => x.DisplayName).HasMaxLength(200);
                modelBuilder.Entity<ApiResource>().Property(x => x.Description).HasMaxLength(1000);
                modelBuilder.Entity<ApiResource>().HasIndex(x => x.Name).IsUnique();
            });

            modelBuilder.Entity<ApiSecret>().Map(apiSecret =>
            {
                apiSecret.ToTable(storeOptions.ApiSecret);

                modelBuilder.Entity<ApiSecret>().HasKey(x => x.Id);
                modelBuilder.Entity<ApiSecret>().Property(x => x.Description).HasMaxLength(1000);
                modelBuilder.Entity<ApiSecret>().Property(x => x.Value).HasMaxLength(2000);
                modelBuilder.Entity<ApiSecret>().Property(x => x.Type).HasMaxLength(250);
                modelBuilder.Entity<ApiSecret>().HasRequired(x => x.ApiResource).WithMany(x => x.Secrets).WillCascadeOnDelete(true);
            });

            modelBuilder.Entity<ApiResourceClaim>().Map(apiClaim =>
            {
                apiClaim.ToTable(storeOptions.ApiClaim);

                modelBuilder.Entity<ApiResourceClaim>().HasKey(x => x.Id);
                modelBuilder.Entity<ApiResourceClaim>().Property(x => x.Type).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<ApiResourceClaim>().HasRequired(x => x.ApiResource).WithMany(x => x.UserClaims).WillCascadeOnDelete(true);
            });

            modelBuilder.Entity<ApiScope>().Map(apiScope =>
            {
                apiScope.ToTable(storeOptions.ApiScope);

                modelBuilder.Entity<ApiScope>().HasKey(x => x.Id);
                modelBuilder.Entity<ApiScope>().Property(x => x.Name).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<ApiScope>().Property(x => x.DisplayName).HasMaxLength(200);
                modelBuilder.Entity<ApiScope>().Property(x => x.Description).HasMaxLength(1000);
                modelBuilder.Entity<ApiScope>().HasIndex(x => x.Name).IsUnique();
                modelBuilder.Entity<ApiScope>().HasRequired(x => x.ApiResource).WithMany(x => x.Scopes).WillCascadeOnDelete(true);
            });

            modelBuilder.Entity<ApiScopeClaim>().Map(apiScopeClaim =>
            {
                apiScopeClaim.ToTable(storeOptions.ApiScopeClaim);

                modelBuilder.Entity<ApiScopeClaim>().HasKey(x => x.Id);
                modelBuilder.Entity<ApiScopeClaim>().Property(x => x.Type).HasMaxLength(200).IsRequired();
                modelBuilder.Entity<ApiScopeClaim>().HasRequired(x => x.ApiScope).WithMany(x => x.UserClaims).WillCascadeOnDelete(true);
            });
        }
    }
}