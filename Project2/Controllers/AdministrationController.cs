using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL.Entitites;
using LOGIC;
using Project2.Models;

namespace Project2.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly UnitLogic lUnit = new UnitLogic();

        private readonly UserLogic lUser = new UserLogic();

        // GET: Administration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNew()
        {
            return View();
        }

        public async Task<JsonResult> NewUnit(Unit model)
        {
            return Json(new {success = await lUnit.NewUnit(model)});
        }

        public async Task<ActionResult> Units()
        {
            var pair = new PairModel
            {
                Units = await lUnit.GetAllUnits(),
                Users = await lUser.GetAllUsers()
            };
            return View(pair);
        }

        public ActionResult UnitMaster(string model)
        {
            var uum = new UnitUsersModel
            {
                Unit = lUnit.GetUnit(model).Result,
                FreeUsers = lUser.GetAllUsers().Result
            };
            return View(uum);
        }

        public async Task<JsonResult> MakeMaster(Unit model)
        {
            return Json(new {success = await lUnit.UpdateUnit(model)});
        }

        public async Task<ActionResult> Users()
        {
            var users = new List<UserManager>();
            foreach (var item in await lUser.GetAllUsers())
                users.Add(new UserManager {user = item, iswriter = await lUser.IsWriter(item.ID)});
            return View(users);
        }

        public async Task<ActionResult> EditUser(string model)
        {
            var us = await lUser.returnUser(model);
            var wr = new Writer();
            if (await lUser.IsWriter(us.ID)) wr = await lUser.returnWriter(model);

            var wrModel = new WriterModel
            {
                fname = us.fname,
                ID = us.ID,
                lname = us.lname,
                mail = us.email,
                orchidURL = wr.orchidUrl,
                privateURL = wr.privateUrl,
                username = us.username,
                writerRole = wr.writerRole
            };
            return View(wrModel);
        }
    }
}