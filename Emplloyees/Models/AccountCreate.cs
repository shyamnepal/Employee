using Emplloyees.Validation;
using System.ComponentModel.DataAnnotations;

namespace Emplloyees.Models
{
    public class AccountCreate
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; } 
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } 
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } 
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } 
        [Required(ErrorMessage = "Password is required")]
        [CustomPasswordValidationAttribue]
        public string Password { get; set; } 
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Compare("Password", ErrorMessage = "password does not match ")]
        public string ConfirmPassword { get; set; }

    }
}
