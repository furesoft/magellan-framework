using iPhone.Applications.Phone.Model;

namespace iPhone.Applications.Phone.Views
{
    public class ContactViewModel
    {
        public ContactViewModel(Contact contact)
        {
            Contact = contact;
        }

        public Contact Contact { get; set; }
    }
}
