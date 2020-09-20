using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DAL.Entitites;
using LOGIC;
using Project2.Models;

namespace Project2.Controllers
{
    public class PublishmentController : Controller
    {
        private readonly PublishmentLogic lPublish = new PublishmentLogic();

        private readonly UserLogic lUser = new UserLogic();
        private readonly StatisticsLogic lStats = new StatisticsLogic();

        // GET: Publishment
        public ActionResult AddNew()
        {
            return View();
        }

        public async Task<ActionResult> Index(string id)
        {
            var _toPublish = new PublishmentModel();
            try
            {
               
                var pub = await lPublish.GetPublishmnet(id);
                _toPublish.publisher_name = await lUser.getUsername(pub.uploadedby);
                switch (pub.type)
                {
                    //prepei na traviksei ta dedomena apo tin basi kai na ftiaksei to pyblshmodel poy ta exei ola
                    case "article":
                        var ar = await lPublish.GetArticle(pub.ID);
                        _toPublish.ID = ar.ID;
                        _toPublish.author = ar.author;
                        _toPublish.volume = ar.volume;
                        _toPublish.pages = ar.pages;
                        _toPublish.title = ar.title;
                        _toPublish.year = ar.year;
                        _toPublish.address = ar.url;
                        _toPublish.mag_name = ar.mag_name;
                        _toPublish.type = "article";
                        break;
                    case "inproceeding":
                        var inpr = await lPublish.GetInproceeding(pub.ID);
                        _toPublish.ID = inpr.ID;
                        _toPublish.author = inpr.editor;
                        _toPublish.booktitle = inpr.booktitle;
                        _toPublish.pages = inpr.pages;
                        _toPublish.address = inpr.address;
                        _toPublish.title = inpr.title;
                        _toPublish.year = inpr.year;
                        _toPublish.type = "inproceeding";

                        break;
                    case "book":
                        var book = await lPublish.GetBook(pub.ID);
                        _toPublish.title = book.title;
                        _toPublish.year = book.year;
                        _toPublish.ID = book.ID;
                        _toPublish.author = book.publisher;
                        _toPublish.volume = book.volume;
                        _toPublish.number = book.number;
                        _toPublish.address = book.address;
                        _toPublish.editor = book.edition;
                        _toPublish.ISBN = book.ISBN;
                        _toPublish.ISSN = book.ISSN;
                        _toPublish.type = "book";
                        break;
                    case "inbook":
                        var inbook = await lPublish.GetInbook(pub.ID);
                        _toPublish.ID = inbook.ID;
                        _toPublish.author = inbook.publisher;
                        _toPublish.chapter = inbook.chapter;
                        _toPublish.volume = inbook.volume;
                        _toPublish.ISBN = inbook.ISBN;
                        _toPublish.ISSN = inbook.ISSN;
                        _toPublish.title = inbook.title;
                        _toPublish.year = inbook.year;
                        _toPublish.editor = inbook.editor;
                        _toPublish.type = "inbook";
                        _toPublish.pages = inbook.page;
                        break;
                }

                return View(_toPublish);
            }
            catch (Exception e)
            {
                _toPublish.type = "deleted";
                return View(_toPublish);
            }
          
        }

        public async Task<ActionResult> SearchResults(string key)
        {
            return View(await lPublish.SearchResults(key));
        }

        public ActionResult MyPublishments()
        {
            return View(lPublish.GetUserPublishments(User.Identity.Name));
        }

        public ActionResult XmlResults(string link)
        {
            var (l1, l2, l3, l4) = lPublish.OrganizeNodes(link);
            var results = new XMLModel {Articles = l1, Books = l2, Inproceedings = l3, Inbooks = l4};
            return View(results);
        }

        public async Task<JsonResult> AddtoDb(string link)
        {
            await lPublish.AddToDatabase(link, User.Identity.Name);
            return Json(new { });
        }


        public async Task<ActionResult> UpdatePublishment(PublishmentModel model)
        {
            switch (model.type)
            {
                case "article":
                    var ar = new Article
                    {
                        author = model.author, ID = model.ID, mag_name = model.mag_name, pages = model.pages,
                        url = model.address, title = model.title, volume = model.volume, year = model.year
                    };
                    await lPublish.UpdateArticle(ar);
                    break;
                case "inproceeding":
                    var inpr = new Inproceeding
                    {
                        address = model.address, booktitle = model.booktitle, editor = model.author, ID = model.ID,
                        pages = model.pages, title = model.title, url = model.url, year = model.year
                    };
                    await lPublish.UpdateInproceeding(inpr);
                    break;
                case "book":
                    var book = new Book
                    {
                        address = model.address, edition = model.edition, ID = model.ID, ISBN = model.ISBN,
                        ISSN = model.ISSN,
                        number = model.number, publisher = model.publisher, series = model.series, title = model.title,
                        volume = model.volume
                    };
                    await lPublish.UpdateBook(book);
                    break;
                case "inbook":
                    var inbook = new Inbook
                    {
                        title = model.title, chapter = model.chapter, publisher = model.author, ID = model.ID,
                        ISBN = model.ISBN,
                        ISSN = model.ISSN, editor = model.publisher, volume = model.volume, year = model.year,
                        page = model.pages
                    };
                    await lPublish.UpdateInBook(inbook);
                    break;
            }

            return Json(new {success = true});
        }


        public async Task<ActionResult> DeletePublishment(PublishmentModel model)
        {
            switch (model.type)
            {
                case "article":
                    await lPublish.DeleteArticle(model.ID);
                    break;
                case "inproceeding":
                    await lPublish.DeleteInproceeding(model.ID);
                    break;
                case "book":
                    await lPublish.DeleteBook(model.ID);
                    break;
                case "inbook":
                    await lPublish.DeleteInbook(model.ID);
                    break;
            }

            return Json(new {success = true});
        }

        public ActionResult ManualAdd()
        {
            return View();
        }


        public async Task<ActionResult> NewPublishment(PublishmentModel model)
        {
            switch (model.type)
            {
                case "Article":
                    var ar = new Article
                    {
                        author = model.author, ID = model.ID, mag_name = model.mag_name, pages = model.pages,
                        url = model.address, title = model.title, volume = model.volume, year = model.year
                    };
                    var temp = new List<Article>();
                    temp.Add(ar);
                    await lPublish.AddArticles(temp, User.Identity.Name);
                    break;
                case "Inrpoceeding":
                    var inpr = new Inproceeding
                    {
                        address = model.address,
                        booktitle = model.booktitle,
                        editor = model.author,
                        ID = model.ID,
                        pages = model.pages,
                        title = model.title,
                        url = model.url,
                        year = model.year
                    };
                    var l2 = new List<Inproceeding>();
                    l2.Add(inpr);
                    await lPublish.AddInproceedings(l2, User.Identity.Name);
                    break;
                case "Book":
                    var book = new Book
                    {
                        address = model.address,
                        edition = model.edition,
                        ID = model.ID,
                        ISBN = model.ISBN,
                        ISSN = model.ISSN,
                        number = model.number,
                        publisher = model.author,
                        series = model.series,
                        title = model.title,
                        volume = model.volume,
                        year=model.year,
                        
                    };
                    var l3 = new List<Book>();
                    l3.Add(book);
                    await lPublish.AddBooks(l3, User.Identity.Name);
                    break;
                case "Inbook":
                    var inbook = new Inbook
                    {
                        ID = model.ID, publisher = model.author, chapter = model.chapter, ISBN = model.ISBN,
                        ISSN = model.ISSN, title = model.title, volume = model.volume, year = model.year,
                        editor = model.editor
                    };
                    var l4 = new List<Inbook>();
                    l4.Add(inbook);
                    await lPublish.AddInbooks(l4, User.Identity.Name);
                    break;
            }

            return Json(new {success = true});
        }

        public async Task<ActionResult> Statistics()
        {
            return View(await lStats.MakeStatistics());
        }
    }
}