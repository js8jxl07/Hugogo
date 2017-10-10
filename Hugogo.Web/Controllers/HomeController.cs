using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hugogo.IBusiness.Tables;
using Hugogo.Injector;

namespace Hugogo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserBusiness _userBusiness = DependencyInjector.GetInstance<IUserBusiness>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var x = _userBusiness.GetUserByUserId("222");
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}