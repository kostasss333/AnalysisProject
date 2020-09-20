using System.Collections.Generic;
using DAL.Entitites;

namespace Project2.Models
{
    public class XMLModel
    {
        public List<Book> Books { get; set; }
        public List<Article> Articles { get; set; }
        public List<Inproceeding> Inproceedings { get; set; }
        public List<Inbook> Inbooks { get; set; }
    }
}