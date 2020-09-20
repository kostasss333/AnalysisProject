using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL;
using DAL.Entitites;
using DAL.InterFaces;
using DAL.Utilities;
using LOGIC.Models;

namespace LOGIC
{
    public class StatisticsLogic
    {
        IUnit Iunits = new UnitUtilities();
        static IPublishments Ipublishments = new PublishmentUtilities();
        IUser Iusers = new UserUtilities();

        //First We Load all the necessary assets that we need in order to create our statistics records
        List<DAL.Entitites.Publishment> publishments = new List<Publishment>();
        List<Article> articles = new List<Article>();
        List<Inbook> inbooks = new List<Inbook>();
        List<Book> books = new List<Book>();
        List<Inproceeding> inproceedings = new List<Inproceeding>();
        List<Unit> units = new List<Unit>();
        private Dictionary<string, string> PublicationsWithUnit= new Dictionary<string, string>();

        public async Task<StatisticsModel> MakeStatistics()
        {
            await FillPublishments();
            await FillTheRest();
            FillTheDictionary();
            Console.WriteLine();
            StatisticsModel model = new StatisticsModel()
            {
                articles = articles, average = this.ReturnAverage(), books = books, inproceedings = inproceedings,
                inbooks = inbooks, pubs = publishments, unitdict = this.DepartmentStats(),piedict = this.PieYearsDictionary(),
                dictionary = this.YearsDictionary()
            };
            return model;
        }

        private Dictionary<string, int> YearsDictionary()
        {
            var resultDict =publishments.GroupBy(f => f.year).Select(g => new { year = g.Key, count = g.Count() })
                .ToDictionary(k => k.year, i => i.count);
            return resultDict;
        }
    


    public async Task FillPublishments()
        {
            var temp = await Ipublishments.GetAllPublishments().ConfigureAwait(false);
            publishments =temp;
            var temp2 = await Iunits.GetAllUnits().ConfigureAwait(false);
            units = temp2;
        }

        public async Task FillTheRest()
        {
            foreach (var item in publishments)
            {
                switch (item.type)
                {
                    case "inproceeding":
                        Inproceeding inpr = await Ipublishments.GetInproceeding(item.ID).ConfigureAwait(false);
                        inproceedings.Add(inpr);
                        break;
                    case "article":
                        var article = await Ipublishments.GetArticle(item.ID).ConfigureAwait(false);
                        articles.Add(article);
                        break;
                    case "inbook":
                        var inbook = await Ipublishments.GetInbook(item.ID).ConfigureAwait(false);
                        inbooks.Add(inbook);
                        break;
                    case "book":
                        var book = await Ipublishments.GetBook(item.ID).ConfigureAwait(false);
                        books.Add(book);
                        break;
                }
            }
        }
       

        private void FillTheDictionary()
        {
            //First we need to get the User that the post is uploaded by
            //We check if the user belongs to a unit .?yes => we add the pub with the unit name or key .?no => do nothing
            foreach (var i in publishments)
            {
                //Locate user
                var user = Iusers.ReturnUserById(i.uploadedby);
                //Find the unit that belongs to
               
                try
                {
                    var unit = Iunits.GetUnitByID(user.unit_that_belongs);
                    PublicationsWithUnit.Add(i.title,unit.ID);
                }
                catch (Exception e)
                {
                    PublicationsWithUnit.Add(i.title, null);
                }
            }
        }


        public int ReturnAverage()
        {
            var counter = 0;
            foreach (var item in articles)
                counter = counter + item.author.Split(',').Length - 1;
            foreach (var item in books)
                counter = counter + item.publisher.Split(',').Length - 1;
            foreach (var item in inbooks)
                counter = counter + item.publisher.Split(',').Length - 1;
            foreach (var item in inproceedings)
                counter = counter + item.editor.Split(',').Length - 1;

            return counter / publishments.Count;
        }

        public Dictionary<string, int> DepartmentStats()
        {
            //We make a new Dictionary with all the units and the number of Publishments they have 
            var counter = 0;
            var toreturn = new Dictionary<string,int>();
            foreach (var unit in units)
            {
                //Every new unit is a new Pair in dictionary
                toreturn.Add(unit.name,0);
                foreach (var item in PublicationsWithUnit)
                {
                    if (item.Value == unit.ID)
                    {
                        counter++;
                        toreturn[unit.name] += 1;
                    }
                }
               
            }
            toreturn.Add("Δημοσιεύσεις απο χρήστες που δεν ανήκουν σε εργαστήριο", publishments.Count - counter);
            return toreturn;
        }

        public Dictionary<string, int> PieYearsDictionary()
        {
            var piedict = new Dictionary<string, int>
            {
                {"2000-2004", 0},
                {"2005-2009", 0},
                {"2010-2014", 0},
                {"2015-2020", 0}
            };
            foreach (var item in publishments)
            {
                try
                {
                    if (int.Parse(item.year) < 2005)
                        piedict["2000-2004"] = piedict["2000-2004"]+1;
                    else if (int.Parse(item.year) < 2010)
                        piedict["2005-2009"] = piedict["2005-2009"]+1;
                    else if (int.Parse(item.year) < 2015)
                        piedict["2010-2014"] = piedict["2010-2014"]+1;
                    else 
                        piedict["2015-2020"] = piedict["2015-2020"]+1;
                }
                catch (Exception e)
                {
                   continue;
                }
            }

            return piedict;
        }





    }
}

