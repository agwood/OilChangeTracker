using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace OilChangeTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Tracking oil changes since 2019!";

            return View();
        }

        
        public ActionResult Contact()
        {
            ViewBag.Message = "Feel free to drop me a line!";

            return View();
        }
    }
}
