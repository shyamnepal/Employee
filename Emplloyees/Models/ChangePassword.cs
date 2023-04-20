using Emplloyees.Validation;
using System.ComponentModel.DataAnnotations;

namespace Emplloyees.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage ="Old Password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage ="New Password is required")]
        [DataType(DataType.Password)]
        [CustomPasswordValidationAttribue]
        public string NewPassword { get; set; }
        [Required(ErrorMessage ="confirmation password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password does not match to new Password")]
        public string ConfirmPassword { get; set; }

    }
}
