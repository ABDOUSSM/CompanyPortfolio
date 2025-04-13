using System.ComponentModel.DataAnnotations;

namespace CompanyPortfolio.Models
{
    public class ContactFormModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}