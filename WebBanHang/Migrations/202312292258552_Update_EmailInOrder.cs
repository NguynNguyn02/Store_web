namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_EmailInOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Email");
        }
    }
}
