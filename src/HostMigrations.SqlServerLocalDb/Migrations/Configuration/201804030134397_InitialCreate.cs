namespace HostMigrations.SqlServerLocalDb.Configuration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiResources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        DisplayName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ApiScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        DisplayName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        Required = c.Boolean(nullable: false),
                        Emphasize = c.Boolean(nullable: false),
                        ShowInDiscoveryDocument = c.Boolean(nullable: false),
                        ApiResource_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiResources", t => t.ApiResource_Id, cascadeDelete: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.ApiResource_Id);
            
            CreateTable(
                "dbo.ApiScopeClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 200),
                        ApiScope_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiScopes", t => t.ApiScope_Id, cascadeDelete: true)
                .Index(t => t.ApiScope_Id);
            
            CreateTable(
                "dbo.ApiSecrets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1000),
                        Value = c.String(maxLength: 2000),
                        Expiration = c.DateTime(),
                        Type = c.String(maxLength: 250),
                        ApiResource_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiResources", t => t.ApiResource_Id, cascadeDelete: true)
                .Index(t => t.ApiResource_Id);
            
            CreateTable(
                "dbo.ApiClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 200),
                        ApiResource_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiResources", t => t.ApiResource_Id, cascadeDelete: true)
                .Index(t => t.ApiResource_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        ClientId = c.String(nullable: false, maxLength: 200),
                        ProtocolType = c.String(nullable: false, maxLength: 200),
                        RequireClientSecret = c.Boolean(nullable: false),
                        ClientName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        ClientUri = c.String(maxLength: 2000),
                        LogoUri = c.String(maxLength: 2000),
                        RequireConsent = c.Boolean(nullable: false),
                        AllowRememberConsent = c.Boolean(nullable: false),
                        AlwaysIncludeUserClaimsInIdToken = c.Boolean(nullable: false),
                        RequirePkce = c.Boolean(nullable: false),
                        AllowPlainTextPkce = c.Boolean(nullable: false),
                        AllowAccessTokensViaBrowser = c.Boolean(nullable: false),
                        FrontChannelLogoutUri = c.String(maxLength: 2000),
                        FrontChannelLogoutSessionRequired = c.Boolean(nullable: false),
                        BackChannelLogoutUri = c.String(maxLength: 2000),
                        BackChannelLogoutSessionRequired = c.Boolean(nullable: false),
                        AllowOfflineAccess = c.Boolean(nullable: false),
                        IdentityTokenLifetime = c.Int(nullable: false),
                        AccessTokenLifetime = c.Int(nullable: false),
                        AuthorizationCodeLifetime = c.Int(nullable: false),
                        ConsentLifetime = c.Int(),
                        AbsoluteRefreshTokenLifetime = c.Int(nullable: false),
                        SlidingRefreshTokenLifetime = c.Int(nullable: false),
                        RefreshTokenUsage = c.Int(nullable: false),
                        UpdateAccessTokenClaimsOnRefresh = c.Boolean(nullable: false),
                        RefreshTokenExpiration = c.Int(nullable: false),
                        AccessTokenType = c.Int(nullable: false),
                        EnableLocalLogin = c.Boolean(nullable: false),
                        IncludeJwtId = c.Boolean(nullable: false),
                        AlwaysSendClientClaims = c.Boolean(nullable: false),
                        ClientClaimsPrefix = c.String(maxLength: 200),
                        PairWiseSubjectSalt = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ClientId, unique: true);
            
            CreateTable(
                "dbo.ClientCorsOrigins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Origin = c.String(nullable: false, maxLength: 150),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientGrantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrantType = c.String(nullable: false, maxLength: 250),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Scope = c.String(nullable: false, maxLength: 200),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 250),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientSecrets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Description = c.String(maxLength: 2000),
                        Value = c.String(nullable: false, maxLength: 2000),
                        Expiration = c.DateTime(),
                        Type = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientIdPRestrictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Provider = c.String(nullable: false, maxLength: 200),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientPostLogoutRedirectUris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostLogoutRedirectUri = c.String(nullable: false, maxLength: 2000),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 2000),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientRedirectUris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RedirectUri = c.String(nullable: false, maxLength: 2000),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.IdentityResources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        DisplayName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        Required = c.Boolean(nullable: false),
                        Emphasize = c.Boolean(nullable: false),
                        ShowInDiscoveryDocument = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.IdentityClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 200),
                        IdentityResource_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityResources", t => t.IdentityResource_Id, cascadeDelete: true)
                .Index(t => t.IdentityResource_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityClaims", "IdentityResource_Id", "dbo.IdentityResources");
            DropForeignKey("dbo.ClientRedirectUris", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientProperties", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientPostLogoutRedirectUris", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientIdPRestrictions", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientSecrets", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientClaims", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientScopes", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientGrantTypes", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientCorsOrigins", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ApiClaims", "ApiResource_Id", "dbo.ApiResources");
            DropForeignKey("dbo.ApiSecrets", "ApiResource_Id", "dbo.ApiResources");
            DropForeignKey("dbo.ApiScopeClaims", "ApiScope_Id", "dbo.ApiScopes");
            DropForeignKey("dbo.ApiScopes", "ApiResource_Id", "dbo.ApiResources");
            DropIndex("dbo.IdentityClaims", new[] { "IdentityResource_Id" });
            DropIndex("dbo.IdentityResources", new[] { "Name" });
            DropIndex("dbo.ClientRedirectUris", new[] { "ClientId" });
            DropIndex("dbo.ClientProperties", new[] { "ClientId" });
            DropIndex("dbo.ClientPostLogoutRedirectUris", new[] { "ClientId" });
            DropIndex("dbo.ClientIdPRestrictions", new[] { "ClientId" });
            DropIndex("dbo.ClientSecrets", new[] { "ClientId" });
            DropIndex("dbo.ClientClaims", new[] { "ClientId" });
            DropIndex("dbo.ClientScopes", new[] { "ClientId" });
            DropIndex("dbo.ClientGrantTypes", new[] { "ClientId" });
            DropIndex("dbo.ClientCorsOrigins", new[] { "ClientId" });
            DropIndex("dbo.Clients", new[] { "ClientId" });
            DropIndex("dbo.ApiClaims", new[] { "ApiResource_Id" });
            DropIndex("dbo.ApiSecrets", new[] { "ApiResource_Id" });
            DropIndex("dbo.ApiScopeClaims", new[] { "ApiScope_Id" });
            DropIndex("dbo.ApiScopes", new[] { "ApiResource_Id" });
            DropIndex("dbo.ApiScopes", new[] { "Name" });
            DropIndex("dbo.ApiResources", new[] { "Name" });
            DropTable("dbo.IdentityClaims");
            DropTable("dbo.IdentityResources");
            DropTable("dbo.ClientRedirectUris");
            DropTable("dbo.ClientProperties");
            DropTable("dbo.ClientPostLogoutRedirectUris");
            DropTable("dbo.ClientIdPRestrictions");
            DropTable("dbo.ClientSecrets");
            DropTable("dbo.ClientClaims");
            DropTable("dbo.ClientScopes");
            DropTable("dbo.ClientGrantTypes");
            DropTable("dbo.ClientCorsOrigins");
            DropTable("dbo.Clients");
            DropTable("dbo.ApiClaims");
            DropTable("dbo.ApiSecrets");
            DropTable("dbo.ApiScopeClaims");
            DropTable("dbo.ApiScopes");
            DropTable("dbo.ApiResources");
        }
    }
}
