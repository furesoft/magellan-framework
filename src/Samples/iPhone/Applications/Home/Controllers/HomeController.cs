using Magellan.Framework;

namespace iPhone.Applications.Home.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            return Page("Home", true);
        }
    }
}
