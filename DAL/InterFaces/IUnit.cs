using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entitites;

namespace DAL.InterFaces
{
    public interface IUnit
    {
        Task AddOrUpdateUnit(Unit unit);
        Task<List<Unit>> GetAllUnits();
        Unit GetUnitByName(string name);

        Unit GetUnitByID(string id);
    }
}