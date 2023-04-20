using Emplloyees.Models;

namespace Emplloyees.Services.UserRoleService.Interfaces
{
    public interface IRoleServices
    {
        bool RoleAlreadyExist(string RoleName);
        void InsertRole(string RoleName);
        List<Role> GetAllRole();
        bool CheckRoleAlreadyAssignToUser(string UserId, string RoleId);

        void AssignUserRole(string UserId, string RoleId);

        List<GetAllUserWithRole> GetAllUserWithRole();

        GetAllUserWithRole GetUserRoleById(string RoleId, string UserId);

        void UpdateUserRoleAssign(EditUserRole edit);

        void DeleteUserRole(string RoleId, string UserId);
    }
}
