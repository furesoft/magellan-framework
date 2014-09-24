using iPhone.Infrastructure.Filters;
using Magellan.Framework;

namespace iPhone.Applications.Settings.Controllers
{
    public class SettingsController : Controller
    {
        [RoleFilter("Administrators")]
        public ActionResult Index()
        {
            return Page();
        }
    }
}
