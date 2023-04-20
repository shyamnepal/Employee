namespace Emplloyees.Models
{
    public class GetAllUserWithRole
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public Guid RoleId { get; set; }

    }
}
