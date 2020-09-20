using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entitites;

namespace DAL.InterFaces
{
    public interface IPublishments
    {
        Task NewInproceeding(Inproceeding inpr);
        Task NewBook(Book book);
        Task NewInbook(Inbook inbook);
        Task NewPublishment(Publishment pub);
        Task NewArticle(Article ar);
        Task<bool> IsANew(string title);
        List<Publishment> GetUserPublishments(string id);
        Task<List<Publishment>> GetAllPublishments();
        Task<Publishment> GetPublishmentById(string id);
        Task<Inproceeding> GetInproceeding(string id);
        Task<Book> GetBook(string id);
        Task<Article> GetArticle(string id);
        Task DeletePublishment(string id);
        Task DeleteArticle(string id);
        Task DeleteInrpoceeding(string id);
        Task DeleteBook(string id);
        Task DeleteInBook(string id);
        Task<Inbook> GetInbook(string id);
    }
}