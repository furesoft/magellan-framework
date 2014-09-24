using System.Collections.Generic;
using iPhone.Applications.Phone.Model;

namespace iPhone.Applications.Phone.Views
{
    public class GroupsViewModel
    {
        public GroupsViewModel(IEnumerable<Group> groups)
        {
            Groups = groups;
        }

        public IEnumerable<Group> Groups { get; set; }
    }
}
