using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entitites;
using DAL.InterFaces;

namespace DAL.Utilities
{
    public class UnitUtilities : IUnit
    {
        public async Task AddOrUpdateUnit(Unit unit)
        {
            using (var db = new Context())
            {
                db.Units.AddOrUpdate(unit);
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<Unit>> GetAllUnits()
        {
            using (var db = new Context())
            {
                return db.Units.ToList();
            }
        }

        public Unit GetUnitByName(string name)
        {
            using (var db = new Context())
            {
                return db.Units.FirstOrDefault(sh => sh.name == name);
            }
        }

        public Unit GetUnitByID(string id)
        {
            using (var db =  new Context())
            {
                return db.Units.FirstOrDefault(sh => sh.ID == id);
            }
        }
    }
}