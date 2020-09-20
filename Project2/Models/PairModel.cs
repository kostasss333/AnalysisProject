using System.Collections.Generic;
using DAL.Entitites;

namespace Project2.Models
{
    public class PairModel
    {
        public List<User> Users { get; set; }
        public List<Unit> Units { get; set; }
    }
}