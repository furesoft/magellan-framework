using System;
namespace iPhone.Applications.Phone.Model
{
    public class Contact
    {
        public Contact(string firstName, string lastName, string jobTitle)
        {
            FirstName = firstName;
            LastName = lastName;
            JobTitle = jobTitle;
            HomeNumber = Math.Abs(firstName.GetHashCode()).ToString("#### ### ###");
            MobileNumber = Math.Abs(lastName.GetHashCode()).ToString("#### ### ###");
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string HomeNumber { get; set; }
        public string MobileNumber { get; set; }
    }
}
