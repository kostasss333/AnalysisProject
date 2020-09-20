using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DAL.Entitites;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace DAL.Utilities
{
    public class UserUtilities : IUser
    {
        public async Task<string> CreateUser(User us, string password)
        {
            var userstore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userstore);

            var user = new IdentityUser {UserName = us.username};
            var result = manager.Create(user, password);
            if (result.Succeeded)
            {
                await CreateSystemUser(us);
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);

                return "OK";
            }

            return result.Errors.FirstOrDefault();
        }

        public async Task<bool> SignIn(string username, string password)
        {
            var userstore = new UserStore<IdentityUser>();
            var usermanager = new UserManager<IdentityUser>(userstore);
            var user = usermanager.Find(username, password);

            if (user != null)
            {
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = usermanager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties {IsPersistent = false}, userIdentity);
                return true;
            }

            return false;
        }

        public bool IsANewName(User us)
        {
            var user = new User();
            using (var db = new Context())
            {
                user = db.Users.FirstOrDefault(sh => sh.fname == us.fname && sh.lname == us.lname);
            }

            if (user == null)
                return true;
            return false;
        }

        public async Task SignOut()
        {
            var authenticationMnaager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationMnaager.SignOut();
        }

        public async Task CreateSystemUser(User us)
        {
            using (var db = new Context())
            {
                db.Users.AddOrUpdate(us);
                await db.SaveChangesAsync();
            }
        }

        public async Task<User> GetLoggedUser(string username)
        {
            try
            {
                using (var db = new Context())
                {
                    return db.Users.FirstOrDefault(sh => sh.username == username);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public async Task<Writer> GetWriterUser(string id)
        {
            using (var db = new Context())
            {
                return await db.Writers.FirstOrDefaultAsync(sh => sh.ID == id);
            }
        }

        public async Task CreateWriter(Writer writer)
        {
            using (var db = new Context())
            {
                db.Writers.AddOrUpdate(writer);
                await db.SaveChangesAsync();
            }
        }

        public User ReturnUserById(string id)
        {
            using (var db = new Context())
            {
                return db.Users.First(sh => sh.ID == id);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            using (var db = new Context())
            {
                return db.Users.ToList();
            }
        }

       
    }
}