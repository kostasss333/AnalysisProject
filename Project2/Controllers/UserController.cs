using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL.Entitites;
using LOGIC;
using Project2.Models;

namespace Project2.Controllers
{
    public class UserController : Controller
    {
        private readonly PublishmentLogic lPublish = new PublishmentLogic();
        private readonly UnitLogic lUnit = new UnitLogic();
        private readonly UserLogic LUser = new UserLogic();

        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public async Task<ActionResult> Profile()
        {
            var user = await LUser.returnUser(User.Identity.Name);
            var writer = await LUser.returnWriter(User.Identity.Name);
            if (writer == null)
            {
                writer = new Writer();
                writer.orchidUrl = "";
                writer.privateUrl = "";
                writer.writerRole = "";
            }

            var profile = new WriterModel
            {
                fname = user.fname, lname = user.lname, mail = user.email, orchidURL = writer.orchidUrl,
                privateURL = writer.privateUrl, username = user.username, writerRole = writer.writerRole, ID = user.ID,
                coutner = lPublish.PublishmentsCount(User.Identity.Name),
                AllUnits = await lUnit.GetAllUnits(), UserRole = user.role, unit_that_belongs = user.unit_that_belongs
            };

            return View(profile);
        }

        public async Task<JsonResult> RegisterUser(UserModel model)
        {
            var user = new User
            {
                fname = model.fname, email = model.mail, ID = Guid.NewGuid().ToString(), lname = model.lname,
                role = "dim",
                username = model.username
            };
            var result = await LUser.NewUser(user, model.password);
            return Json(new {success = true, message = result});
        }


        public async Task<JsonResult> LoginUser(UserModel model)
        {
            return Json(new {success = await LUser.LogInUser(model.username, model.password)});
        }

        public async Task<RedirectResult> SignOut()
        {
            if (await LUser.LogOutUser()) return Redirect("~/User/Login");

            return null;
            //else print error message
        }

        public async Task<JsonResult> UpdateProfile(WriterModel model)
        {
            var user = new User
            {
                ID = model.ID, email = model.mail, fname = model.fname, lname = model.lname, role = model.UserRole,
                username = model.username, unit_that_belongs = model.unit_that_belongs
            };
            var writer = new Writer
            {
                ID = model.ID, orchidUrl = model.orchidURL, privateUrl = model.privateURL, writerRole = model.writerRole
            };
            var res = LUser.UpdateUser(user, writer).Result;
            return Json(new {success = res});
        }
    }
}