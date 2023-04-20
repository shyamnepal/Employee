namespace Emplloyees.Models
{
    public class EditUserRole
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid OldRoleId { get; set; }
    }
}
