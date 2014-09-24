using System.ComponentModel.DataAnnotations;

namespace Wizard.Model
{
    public class NewAccount
    {
        [Display(Name = "Full name", Prompt = "John Smith")]
        [Required]
        [StringLength(30)]
        public string FullName { get; set; }

        [Display(Name = "Email address", Prompt = "john.smith@aol.com")]
        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }
        
        public string Password { get; set; }
        
        public ServerSettings Server { get; set; }
    }
}