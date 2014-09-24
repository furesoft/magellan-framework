using System.Web.Mvc;

namespace Magellan.SilverlightSamples.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Quickstart()
        {
            return View();
        }
    }
}
