using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entitites;
using DAL.InterFaces;

namespace DAL.Utilities
{
    public class PublishmentUtilities : IPublishments
    {
        public async Task NewPublishment(Publishment pub)
        {
            using (var db = new Context())
            {
                db.Publishments.AddOrUpdate(pub);
                await db.SaveChangesAsync();
            }
        }

        public async Task NewArticle(Article ar)
        {
            try
            {
                using (var db = new Context())
                {
                    db.Articles.AddOrUpdate(ar);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task NewBook(Book book)
        {
            using (var db = new Context())
            {
                db.Books.AddOrUpdate(book);
                await db.SaveChangesAsync();
            }
        }

        public async Task NewInproceeding(Inproceeding inpr)
        {
            using (var db = new Context())
            {
                db.Inproceedings.AddOrUpdate(inpr);
                await db.SaveChangesAsync();
            }
        }

        public async Task NewInbook(Inbook inbook)
        {
            using (var db = new Context())
            {
                db.Inbooks.AddOrUpdate(inbook);
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> IsANew(string title)
        {
            var pub = new Publishment();
            using (var db = new Context())
            {
                pub = await db.Publishments.FirstOrDefaultAsync(sh => sh.title == title);
            }

            if (pub == null)
                return true;
            return false;
        }

        public List<Publishment> GetUserPublishments(string id)
        {
            using (var db = new Context())
            {
                return db.Publishments.Where(sh => sh.uploadedby == id).ToList();
            }
        }

        public async Task<List<Publishment>> GetAllPublishments()
        {
            using (var db = new Context())
            {
                return await db.Publishments.ToListAsync();
            }
        }

        public async Task<Publishment> GetPublishmentById(string id)
        {
            using (var db = new Context())
            {
                return await db.Publishments.FirstAsync(sh => sh.ID == id);
            }
        }

        public async Task<Article> GetArticle(string id)
        {
            using (var db = new Context())
            {
                return await db.Articles.FirstAsync(sh => sh.ID == id);
            }
        }

        public async Task<Inproceeding> GetInproceeding(string id)
        {
            using (var db = new Context())
            {
                return await db.Inproceedings.FirstAsync(sh => sh.ID == id);
            }
        }

        public async Task<Book> GetBook(string id)
        {
            using (var db = new Context())
            {
                return await db.Books.FirstAsync(sh => sh.ID == id);
            }
        }

        public async Task<Inbook> GetInbook(string id)
        {
            using (var db = new Context())
            {
                return await db.Inbooks.FirstAsync(sh => sh.ID == id);
            }
        }

        public async Task DeletePublishment(string id)
        {
            using (var db = new Context())
            {
                var pub = db.Publishments.First(sh => sh.ID == id);
                db.Publishments.Remove(pub);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteArticle(string id)
        {
            using (var db = new Context())
            {
                var pub = db.Articles.First(sh => sh.ID == id);
                db.Articles.Remove(pub);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteInrpoceeding(string id)
        {
            using (var db = new Context())
            {
                var pub = db.Inproceedings.First(sh => sh.ID == id);
                db.Inproceedings.Remove(pub);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteBook(string id)
        {
            using (var db = new Context())
            {
                var pub = db.Books.First(sh => sh.ID == id);
                db.Books.Remove(pub);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteInBook(string id)
        {
            using (var db = new Context())
            {
                var pub = db.Inbooks.First(sh => sh.ID == id);
                db.Inbooks.Remove(pub);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteInbook(string id)
        {
            using (var db = new Context())
            {
                var inbook = db.Inbooks.First(sh => sh.ID == id);
                db.Inbooks.Remove(inbook);
                await db.SaveChangesAsync();
            }
        }
    }
}