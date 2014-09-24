using System.Collections.Generic;
using iPhone.Applications.Phone.Model;

namespace iPhone.Applications.Phone.Repositories
{
    public interface IContactRepository
    {
        IEnumerable<Group> GetGroups();
        IEnumerable<Contact> GetContacts(Group group);
    }
}
