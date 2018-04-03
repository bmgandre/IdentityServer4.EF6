namespace HostMigrations.SqlServerLocalDb.PersistedGrant
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersistedGrants",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 200),
                        Type = c.String(nullable: false, maxLength: 50),
                        SubjectId = c.String(maxLength: 200),
                        ClientId = c.String(nullable: false, maxLength: 200),
                        CreationTime = c.DateTime(nullable: false),
                        Expiration = c.DateTime(),
                        Data = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .Index(t => new { t.SubjectId, t.ClientId, t.Type });
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PersistedGrants", new[] { "SubjectId", "ClientId", "Type" });
            DropTable("dbo.PersistedGrants");
        }
    }
}
