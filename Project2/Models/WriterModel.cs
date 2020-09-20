using System.Collections.Generic;
using DAL.Entitites;

namespace Project2.Models
{
    public class WriterModel
    {
        public string ID { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string username { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public string orchidURL { get; set; }
        public string privateURL { get; set; }
        public string writerRole { get; set; }
        public int coutner { get; set; }
        public string UserRole { get; set; }
        public List<Unit> AllUnits { get; set; }

        public string unit_that_belongs { get; set; }
    }
}