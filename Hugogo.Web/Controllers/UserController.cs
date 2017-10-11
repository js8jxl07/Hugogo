using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hugogo.IBusiness.Tables;
using Hugogo.Injector;
using Hugogo.Model.Tables;

namespace Hugogo.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBusiness _userBusiness = DependencyInjector.GetInstance<IUserBusiness>();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserEdit(int type, User user)
        {
            return null;
        }
    }
}