using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using DAL;
using DAL.Entitites;
using DAL.InterFaces;
using DAL.Utilities;

namespace LOGIC
{
    public class PublishmentLogic
    {
        //We use those global lists to save the tag names and the values of each element we need.
        private readonly List<string> publishedElements = new List<string>();
        private readonly List<string> publishedValues = new List<string>();
        private readonly IPublishments iPublishments = new PublishmentUtilities();
        private readonly IUser iUser = new UserUtilities();
        private readonly UserLogic LUser = new UserLogic();

        //We use XElement library to split XML file to smaller nodes that contains a publishment. Then we categorize them to each element 
        public async Task XMLSolve(string link)
        {
            try
            {
                var element = XElement.Load(link);
                var nodes = element.Descendants()
                    .ToList(); //a list with all nodes of the element "each publishment of the xml plus the head"
                await Analyze(nodes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public (List<Article>, List<Book>, List<Inproceeding>, List<Inbook>) OrganizeNodes(string link)
        {
            XMLSolve(link).Wait();
            var articles = new List<Article>();
            var books = new List<Book>();
            var inproceedings = new List<Inproceeding>();
            var inbooks = new List<Inbook>();
            for (var i = 0; i < publishedElements.Count(); i++)
            {
                if (publishedElements[i].StartsWith("article"))
                {
                    var article = new Article();
                    do
                    {
                        i++;
                        if (publishedElements[i].StartsWith("author")) article.author += "," + publishedValues[i];

                        if (publishedElements[i].StartsWith("title")) article.title = publishedValues[i];

                        if (publishedElements[i].StartsWith("pages")) article.pages += publishedValues[i];

                        if (publishedElements[i].StartsWith("volume")) article.volume += publishedValues[i];

                        if (publishedElements[i].StartsWith("url")) article.url += publishedValues[i];

                        if (publishedElements[i].StartsWith("year")) article.year += publishedValues[i];

                        if (publishedElements[i].StartsWith("journal")) article.mag_name += publishedValues[i];
                    } while (publishedElements[i] != "end");

                    articles.Add(article);
                }

                if (publishedElements[i].Equals("book"))
                {
                    var book = new Book();
                    do
                    {
                        i++;

                        if (publishedElements[i].StartsWith("title")) book.title += publishedValues[i];

                        if (publishedElements[i].StartsWith("volume")) book.volume += publishedValues[i];

                        if (publishedElements[i].StartsWith("publisher")) book.publisher += publishedValues[i];

                        if (publishedElements[i].StartsWith("number")) book.number += int.Parse(publishedValues[i]);

                        if (publishedElements[i].StartsWith("year")) book.year += publishedValues[i];

                        if (publishedElements[i].StartsWith("edition")) book.edition += publishedValues[i];

                        if (publishedElements[i].StartsWith("address")) book.address += publishedValues[i];

                        if (publishedElements[i].StartsWith("isbn")) book.ISBN += publishedValues[i];
                        if (publishedElements[i].StartsWith("issn")) book.ISSN += publishedValues[i];
                    } while (publishedElements[i] != "end");

                    books.Add(book);
                }

                if (publishedElements[i].StartsWith("inproceedings"))
                {
                    var inpr = new Inproceeding();
                    do
                    {
                        i++;
                        if (publishedElements[i].StartsWith("author")) inpr.editor += "," + publishedValues[i];


                        if (publishedElements[i].StartsWith("title")) inpr.title = publishedValues[i];

                        if (publishedElements[i].StartsWith("booktitle")) inpr.booktitle += publishedValues[i];


                        if (publishedElements[i].StartsWith("pages")) inpr.pages += "" + publishedValues[i];

                        if (publishedElements[i].StartsWith("ee")) inpr.address += "" + publishedValues[i];

                        if (publishedElements[i].StartsWith("url")) inpr.url += publishedValues[i];
                        if (publishedElements[i].StartsWith("year")) inpr.year += "" + publishedValues[i];
                    } while (publishedElements[i] != "end");

                    inproceedings.Add(inpr);
                }
            }

            return (articles, books, inproceedings, inbooks);
        }

        public async Task DeleteArticle(string iD)
        {
            await iPublishments.DeletePublishment(iD);
            await iPublishments.DeleteArticle(iD);
        }

        public async Task DeleteInproceeding(string iD)
        {
            await iPublishments.DeletePublishment(iD);
            await iPublishments.DeleteInrpoceeding(iD);
        }

        private async Task Analyze(List<XElement> nodes)
        {
            //Now we take each node one by one and check its inside elements 
            foreach (var item in nodes)
                //In our xml meants that we change element
                if (item.Name == "r")
                {
                    var inNode = item.NextNode;
                    var elements =
                        (inNode as XElement).Descendants()
                        .ToList(); //we use the same proccess as before to make an "organize nod" with the insisde elements
                    foreach (var clue in elements)
                    {
                        //We make to parallels list inn the first will be all the tag names and in the other all the values. Later with those lists we will make our models.
                        publishedElements.Add(clue.Name.ToString());
                        publishedValues.Add(clue.Value);
                    }

                    publishedElements.Add("end");
                    publishedValues.Add("end");
                }
        }

        public async Task AddToDatabase(string link, string identityName)
        {
            var (l1, l2, l3, l4) = OrganizeNodes(link);
            await AddArticles(l1, identityName);
            await AddBooks(l2, identityName);
            await AddInproceedings(l3, identityName);
            await AddInbooks(l4, identityName);
        }

        public async Task AddInbooks(List<Inbook> l4, string identityName)
        {
            var username = await iUser.GetLoggedUser(identityName);
            foreach (var item in l4)
                if (await iPublishments.IsANew(item.title))
                {
                    var ID = Guid.NewGuid().ToString();
                    var pub = new Publishment
                    {
                        ID = ID,
                        title = item.title,
                        type = "inbook",
                        uploadedby = username.ID,
                        year = item.year,
                        authors = item.editor
                    };
                    item.ID = ID;
                    await iPublishments.NewInbook(item);
                    await iPublishments.NewPublishment(pub);
                    await UpdateWriters(item.editor);
                }
        }

        public async Task AddInproceedings(List<Inproceeding> l3, string identityName)
        {
            var username = await iUser.GetLoggedUser(identityName);
            foreach (var item in l3)
                if (await iPublishments.IsANew(item.title))
                {
                    var ID = Guid.NewGuid().ToString();
                    var pub = new Publishment
                    {
                        ID = ID,
                        title = item.title,
                        type = "inproceeding",
                        uploadedby = username.ID,
                        year = item.year,
                        authors = item.editor
                    };
                    item.ID = ID;
                    await iPublishments.NewInproceeding(item);
                    await iPublishments.NewPublishment(pub);
                    await UpdateWriters(item.editor);
                }
        }

        public async Task AddBooks(List<Book> l2, string identityName)
        {
            var username = await iUser.GetLoggedUser(identityName);
            foreach (var item in l2)
                if (await iPublishments.IsANew(item.title))
                {
                    var ID = Guid.NewGuid().ToString();
                    var pub = new Publishment
                    {
                        ID = ID,
                        title = item.title,
                        type = "book",
                        uploadedby = username.ID,
                        year = item.year,
                        authors = item.publisher
                    };
                    item.ID = ID;
                    await iPublishments.NewBook(item);
                    await iPublishments.NewPublishment(pub);
                    await UpdateWriters(item.publisher);
                }
        }


        public async Task AddArticles(List<Article> l1, string identityName)
        {
            try
            {
                var lUser = new UserLogic();
                var user = await lUser.returnUser(identityName).ConfigureAwait(false);
                Console.WriteLine();
                foreach (var item in l1)

                    if (await iPublishments.IsANew(item.title))
                    {
                        var ID = Guid.NewGuid().ToString();
                        var pub = new Publishment
                        {
                            ID = ID,
                            title = item.title,
                            type = "article",
                            uploadedby = user.ID,
                            year = item.year,
                            authors = item.author
                        };
                        item.ID = ID;
                        await iPublishments.NewArticle(item);
                        await iPublishments.NewPublishment(pub);
                        await UpdateWriters(item.author);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task UpdateWriters(string itemAuthor)
        {
            //We split all the Writers from the XML with comma.
            //Then we make an array with all of the writers names in order to check if they exist and finally send them to database.
            //Publishments will have the new Writer ID that belongs to. If ever a writer decides to join the system. This will match the profile with the system users.


            var writers = itemAuthor.Split(',');
            foreach (var item in writers)
            {
                if (item == "") continue;
                var user = new User();
                user.ID = Guid.NewGuid().ToString();
                user.fname = item.Split(' ')[0];
                user.lname = "";
                try
                {
                    user.lname = item.Split(' ')[1];
                }
                catch (Exception E)
                {
                    user.lname = "";
                }

                if (iUser.IsANewName(user)) await LUser.CreateGeneratedWriter(user);
            }
        }

        public async Task<List<Publishment>> GetUserPublishments(string identityName)
        {
            try
            {
                var user = await LUser.returnUser(identityName);
                return iPublishments.GetUserPublishments(user.ID);
            }
            catch (System.NullReferenceException e)

            {
                return null;
            }
          
           
        }

        public async Task<List<Publishment>> SearchResults(string key)
        {
            var pub = new List<Publishment>();
            try
            {
                var load = await iPublishments.GetAllPublishments().ConfigureAwait(true);
                pub.AddRange(load.FindAll(x => x.title.Contains(key)));
                pub.AddRange(load.FindAll(x => x.year.Contains(key)));
                pub.AddRange(load.FindAll(x => x.authors.Contains(key)));
                pub.AddRange(load.FindAll(x => x.type.Contains(key)));
                return pub.Distinct().ToList();
            }
            catch (Exception e)
            {
                
            }
            return pub.Distinct().ToList();

        }

        public async Task<Publishment> GetPublishmnet(string id)
        {
            try
            {
                return await iPublishments.GetPublishmentById(id);
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        public async Task<Article> GetArticle(string id)
        {
            return await iPublishments.GetArticle(id);
        }

        public async Task<Inproceeding> GetInproceeding(string id)
        {
            return await iPublishments.GetInproceeding(id);
        }

        public async Task<Book> GetBook(string id)
        {
            return await iPublishments.GetBook(id);
        }

        public async Task UpdateArticle(Article ar)
        {
            await iPublishments.NewArticle(ar);
            var pub = await iPublishments.GetPublishmentById(ar.ID);
            pub.authors = ar.author;
            pub.year = ar.year;
            await iPublishments.NewPublishment(pub);
        }

        public async Task UpdateInproceeding(Inproceeding inpr)
        {
            await iPublishments.NewInproceeding(inpr);
            var pub = await iPublishments.GetPublishmentById(inpr.ID);
            pub.authors = inpr.editor;
            pub.year = inpr.year;
            await iPublishments.NewPublishment(pub);
        }

        public async Task UpdateBook(Book book)
        {
            await iPublishments.NewBook(book);
            var pub = await iPublishments.GetPublishmentById(book.ID);
            pub.authors = book.publisher;
            pub.year = book.year;
            await iPublishments.NewPublishment(pub);
        }

        public int PublishmentsCount(string name)
        {
            return GetUserPublishments(name).Result.Count();
        }

        public async Task DeleteInbook(string modelId)
        {
            await iPublishments.DeletePublishment(modelId);
            await iPublishments.DeleteInBook(modelId);
        }

        public async Task DeleteBook(string modelId)
        {
            await iPublishments.DeletePublishment(modelId);
            await iPublishments.DeleteBook(modelId);
        }

        public async Task UpdateInBook(Inbook inbook)
        {
            await iPublishments.NewInbook(inbook);
            var pub = await iPublishments.GetPublishmentById(inbook.ID);
            pub.authors = inbook.publisher;
            pub.year = inbook.year;
            await iPublishments.NewPublishment(pub);
        }

        public async Task<Inbook> GetInbook(string pubId)
        {
            return await iPublishments.GetInbook(pubId);
        }
    }
}