using Magellan.Framework;

namespace Quickstart.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Model = "Hello";
            return Page("Index");
        }

        public ActionResult Patient(int patientId)
        {
            Model = "Hello";
            return Page("About");
        }

        public ActionResult About()
        {
            return Page("About");
        }

        public ActionResult Options()
        {
            return ChildWindow("Options");
        }
    }
}
