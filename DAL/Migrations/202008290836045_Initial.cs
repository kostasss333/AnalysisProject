using System.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Articles",
                    c => new
                    {
                        ID = c.String(false, 128),
                        mag_name = c.String(),
                        volume = c.String(),
                        pages = c.String(),
                        author = c.String(),
                        title = c.String(),
                        year = c.String(),
                        url = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Books",
                    c => new
                    {
                        ID = c.String(false, 128),
                        title = c.String(),
                        year = c.String(),
                        publisher = c.String(),
                        volume = c.String(),
                        number = c.Int(false),
                        series = c.String(),
                        address = c.String(),
                        edition = c.String(),
                        ISBN = c.String(),
                        ISSN = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Inbooks",
                    c => new
                    {
                        ID = c.String(false, 128),
                        publisher = c.String(),
                        chapter = c.String(),
                        volume = c.String(),
                        ISBN = c.String(),
                        ISSN = c.String(),
                        title = c.String(),
                        year = c.String(),
                        editor = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Inproceedings",
                    c => new
                    {
                        ID = c.String(false, 128),
                        booktitle = c.String(),
                        editor = c.String(),
                        pages = c.String(),
                        address = c.String(),
                        title = c.String(),
                        url = c.String(),
                        year = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Publishments",
                    c => new
                    {
                        ID = c.String(false, 128),
                        type = c.String(),
                        title = c.String(),
                        year = c.String(),
                        uploadedby = c.String(),
                        authors = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Units",
                    c => new
                    {
                        ID = c.String(false, 128),
                        name = c.String(),
                        type = c.String(),
                        institute = c.String(),
                        description = c.String(),
                        url = c.String(),
                        ownded_by = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Users",
                    c => new
                    {
                        ID = c.String(false, 128),
                        fname = c.String(),
                        lname = c.String(),
                        username = c.String(),
                        email = c.String(),
                        role = c.String(),
                        unit_that_belongs = c.String()
                    })
                .PrimaryKey(t => t.ID);

            CreateTable(
                    "dbo.Writers",
                    c => new
                    {
                        ID = c.String(false, 128),
                        orchidUrl = c.String(),
                        privateUrl = c.String(),
                        writerRole = c.String(),
                        isRealUser = c.Boolean(false)
                    })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.Writers");
            DropTable("dbo.Users");
            DropTable("dbo.Units");
            DropTable("dbo.Publishments");
            DropTable("dbo.Inproceedings");
            DropTable("dbo.Inbooks");
            DropTable("dbo.Books");
            DropTable("dbo.Articles");
        }
    }
}