using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using iPhone.Applications.Phone.Model;

namespace iPhone.Applications.Phone.Views
{
    public class GroupViewModel
    {
        private readonly IEnumerable<Contact> _contacts;
        private readonly ObservableCollection<Contact> _visibleContacts = new ObservableCollection<Contact>();
        private string _searchText;
        
        public GroupViewModel(string groupName, IEnumerable<Contact> contacts)
        {
            _contacts = contacts;
            GroupName = groupName;
            SearchText = "";
        }

        public string GroupName { get; set; }
        
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                Filter();
            }
        }

        private void Filter()
        {
            _visibleContacts.Clear();
            foreach (var item in _contacts)
            {
                if (_searchText.Length == 0 
                    || item.FirstName.IndexOf(_searchText, StringComparison.CurrentCultureIgnoreCase) >= 0 
                    || item.LastName.IndexOf(_searchText, StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    _visibleContacts.Add(item);
                }
            }
        }

        public IEnumerable<Contact> VisibleContacts 
        {
            get { return _visibleContacts; }
        }
    }
}
