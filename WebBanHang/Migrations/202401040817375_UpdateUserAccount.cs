namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CreateByDay", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CreateByDay");
        }
    }
}
