using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Entitites;
using DAL.InterFaces;
using DAL.Utilities;

namespace LOGIC
{
    public class UnitLogic
    {
        public IUnit IUnit = new UnitUtilities();
        public IUser IUser = new UserUtilities();

        public async Task<bool> NewUnit(Unit unit)
        {
            try
            {
                unit.ID = Guid.NewGuid().ToString();
                unit.ownded_by = "free";
                await IUnit.AddOrUpdateUnit(unit);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Unit>> GetAllUnits()
        {
            return await IUnit.GetAllUnits();
        }

        public async Task<Unit> GetUnit(string givenId)
        {
            return GetAllUnits().Result.First(sh => sh.ID == givenId);
        }

        public async Task<bool> UpdateUnit(Unit model)
        {
            var temp = model.ownded_by; //we save the username before we change it to the new type
            //MUST ADD IF WE REMOVE SOMEONE FROM CHIEF//
            try
            {
                //We take the user ID via its username. Then we add that ID to owned_by shell 
                var userId = await IUser.GetLoggedUser(model.ownded_by);

                model.ownded_by = userId.ID;
                await IUnit.AddOrUpdateUnit(model);

                //After updating Unit With the owner we must change and users tags. Note that if someone is an owner of a unit its also a part of it
                var user = await IUser.GetLoggedUser(temp);
                user.role = "yp";
                user.unit_that_belongs = model.ID;
                await IUser.CreateSystemUser(user);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}