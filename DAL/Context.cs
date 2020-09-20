using System.Data.Entity;
using DAL.Entitites;

namespace DAL
{
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<Unit> Units { get; set; }

        public DbSet<Publishment> Publishments { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Inbook> Inbooks { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Inproceeding> Inproceedings { get; set; }
    }
}