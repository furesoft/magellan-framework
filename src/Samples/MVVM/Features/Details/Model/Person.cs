using System.ComponentModel;

namespace Sample.Features.Details.Model
{
    public class Person
    {
        public int Id { get; set; }
        [DisplayName("First name")]
        public string FirstName { get; set; }
        [DisplayName("Surname")]
        public string LastName { get; set; }
        public string Title { get; set; }
        public bool IsOverdue { get; set; }
    }
}
