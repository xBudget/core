using System.ComponentModel.DataAnnotations;

namespace xBudget.Identity.Api.Models
{
    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Property {0} is required.")]
        [EmailAddress(ErrorMessage = "The value for property {0} is invalid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Property {0} is required")]
        public string Password { get; set; }

        [Compare("Password")]
        public string PasswordConfirmation { get; set; }
    }
}
