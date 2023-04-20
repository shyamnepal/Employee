using System.ComponentModel.DataAnnotations;

namespace Emplloyees.Models
{
    public class UserProfileInfo
    {
        [Required(ErrorMessage ="TemporaryAddress is required")]
        public string TemporaryAddress { get; set; }
        [Required(ErrorMessage = "ParmanentAddress is required")]
        public string ParmanentAddress { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string  Country { get; set; }
        [Required(ErrorMessage = "PostalCode is required")]
        public int PostalCode { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string  State { get; set; }
        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Sex is required")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }
    }
}
