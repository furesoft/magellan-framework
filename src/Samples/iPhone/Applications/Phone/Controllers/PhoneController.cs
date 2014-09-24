using iPhone.Applications.Phone.Model;
using iPhone.Applications.Phone.Repositories;
using iPhone.Applications.Phone.Views;
using iPhone.Infrastructure.Filters;
using Magellan.Framework;

namespace iPhone.Applications.Phone.Controllers
{
    public class PhoneController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public PhoneController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [Sleep]
        public ActionResult Index()
        {
            return Page();
        }

        [Sleep]
        public ActionResult Groups()
        {
            var groups = _contactRepository.GetGroups();

            return Page(new GroupsViewModel(groups));
        }

        [Sleep]
        public ActionResult Group(Group group)
        {
            var contacts = _contactRepository.GetContacts(group);

            return Page(new GroupViewModel(group.Name, contacts));
        }

        [Sleep]
        public ActionResult Contact(Contact contact)
        {
            return Page(new ContactViewModel(contact));
        }

        public ActionResult Keypad()
        {
            return Page();
        }

        public ActionResult Call()
        {
            return Page();
        }
    }
}
