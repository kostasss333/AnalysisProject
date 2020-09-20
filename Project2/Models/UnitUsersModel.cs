using System.Collections.Generic;
using DAL.Entitites;

namespace Project2.Models
{
    public class UnitUsersModel
    {
        public Unit Unit { get; set; }
        public List<User> FreeUsers { get; set; }
    }
}