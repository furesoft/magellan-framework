using System.Collections.Generic;
using iPhone.Applications.Phone.Model;

namespace iPhone.Applications.Phone.Repositories.InMemory
{
    internal class InMemoryContactRepository : IContactRepository
    {
        public IEnumerable<Group> GetGroups()
        {
            yield return new Group() {Name="Exchange"};
            yield return new Group() {Name="Other"};
        }

        public IEnumerable<Contact> GetContacts(Group group)
        {
            if (group.Name == "Exchange")
            {
                yield return new Contact("Nicole", "Kidman", "Actor");
                yield return new Contact("Russel", "Crowe", "Actor");
                yield return new Contact("Geoffrey", "Rush", "Actor");
                yield return new Contact("Mel", "Gibson", "Actor");
                yield return new Contact("Sam", "Neil", "Actor");
                yield return new Contact("Hugh", "Jackman", "Actor");
                yield return new Contact("Eric", "Bana", "Actor");
                yield return new Contact("Cate", "Blanchett", "Actor");
                yield return new Contact("Rachel", "Griffiths", "Actor");
                yield return new Contact("Naomi", "Watts", "Actor");
                yield return new Contact("Miranda", "Otto", "Actor");
                yield return new Contact("Errol", "Flynn", "Actor");
            }
            else
            {
                yield return new Contact("Ray", "Meagher", "Actor");
                yield return new Contact("Kate", "Ritchie", "Actor");
                yield return new Contact("Lynne", "McGranger", "Actor");
                yield return new Contact("Norman", "Coburn", "Actor");
                yield return new Contact("Chloe", "Marshall", "Actor");
            }
        }
    }
}
