using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using DAL.Entitites;
using DAL.InterFaces;
using DAL.Utilities;

namespace LOGIC
{
    public class UserLogic
    {
        private readonly IUnit iUnit = new UnitUtilities();
        private readonly IUser iUser = new UserUtilities();

        public async Task<string> NewUser(User user, string password)
        {
            return await iUser.CreateUser(user, password);
        }

        public async Task<bool> LogInUser(string username, string password)
        {
            return await iUser.SignIn(username, password);
        }

        public async Task<bool> LogOutUser()
        {
            try
            {
                await iUser.SignOut();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<User> returnUser(string username)
        {
            var user = new User();
            try
            {
                user = await iUser.GetLoggedUser(username);
            }
            catch (Exception e)
            {
                return null;
            }

            return user;
        }

        public async Task<Writer> returnWriter(string username)
        {
            try
            {
                var user = await iUser.GetLoggedUser(username);
                var userID = user.ID;
                return await iUser.GetWriterUser(userID);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateUser(User user, Writer writer)
        {
            try
            {
                //Before we apply a unit change we should check if a user is a member of a unit and if that differs from our selected option
                var _olduser = iUser.ReturnUserById(user.ID);
                var unit = iUnit.GetUnitByName(user.unit_that_belongs);
                user.role = "dim";
                if (unit != null) //Xtypouse ama o user htan null giati den mporouse na kanei thn sigrisi parakatw
                {
                    if (_olduser.unit_that_belongs != unit.ID)
                        user.unit_that_belongs = unit.ID;
                }

                //Update User Items
                iUser.CreateSystemUser(user);

                //Update Writer Items Adds Automatically If its a new profile
                writer.isRealUser = true;
                iUser.CreateWriter(writer);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsWriter(string id)
        {
            try
            {
                var result = await iUser.GetWriterUser(id);
                if (result == null)
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await iUser.GetAllUsers();
        }

        public async Task CreateGeneratedWriter(User us)
        {
            iUser.CreateSystemUser(us);
            var writer = new Writer
            {
                ID = us.ID, isRealUser = false
            };
            iUser.CreateWriter(writer);
        }

        public async Task<string> getUsername(string id)
        {
            var user = iUser.ReturnUserById(id);
            return user.username;
        }
    }
}