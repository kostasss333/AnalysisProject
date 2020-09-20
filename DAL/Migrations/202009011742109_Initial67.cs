using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class Initial67 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inbooks", "page", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Inbooks", "page");
        }
    }
}