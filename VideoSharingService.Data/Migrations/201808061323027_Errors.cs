namespace VideoSharingService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Errors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExceptionType = c.String(),
                        ExceptionText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.AspNetUsers", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropTable("dbo.Errors");
        }
    }
}
