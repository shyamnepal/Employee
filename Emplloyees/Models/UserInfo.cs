namespace Emplloyees.Models
{
    public class UserInfo
    {
        public Guid ID { get; set; }
        public Guid AddressId { get; set; }
        public Guid ProfileID { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string TemporaryAddress { get; set; }
        public string ParmanentAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public int PostalCode { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string sex { get; set; }
    }
}
