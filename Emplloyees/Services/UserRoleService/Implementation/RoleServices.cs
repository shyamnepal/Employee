using Emplloyees.Models;
using Emplloyees.Services.UserRoleService.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using System.Data;

namespace Emplloyees.Services.UserRoleService.Implementation
{
    public class RoleServices : IRoleServices
    {
        private readonly EmployeeContext _employeeContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public RoleServices(EmployeeContext employeeContext, IHttpContextAccessor contextAccessor)
        {
            _employeeContext = employeeContext;
            _contextAccessor = contextAccessor;
        }

        //Assign Role to the user.
        public void AssignUserRole(string UserId, string RoleId)
        {
            //Get userName of the Login user.
            var UserName = _contextAccessor.HttpContext.User.Identity.Name;

            //Assign the role to the User.
            var parameter = $"EXEC AssignRole @UserId='{UserId}', @RoleId='{RoleId}', @CreatedAT='{DateTime.Now}', @CreatedBy='{UserName}' ";
            _employeeContext.Database.ExecuteSqlRaw(parameter);
        }

        //Check the same role assign to the user.
        public bool CheckRoleAlreadyAssignToUser(string UserId, string RoleId)
        {
           
            var roleIdParam = new SqlParameter("@RoleId", RoleId);
            var userIdParam = new SqlParameter("@UserId", UserId);
            var sameRoleCheck = _employeeContext.UserRoles.FromSqlRaw("EXEC spAlreadyRoleAssign @UserId, @RoleId", userIdParam, roleIdParam).ToList();
            if(sameRoleCheck!=null && sameRoleCheck.Count> 0)
            {
                return true;
            }
            return false;
        }

        //Delete the user assign role from the database. 
        public void DeleteUserRole(string RoleId, string UserId)
        {
            _employeeContext.Database.ExecuteSqlRaw("EXEC DeleteUserRole @UserId, @RoleId",
                new SqlParameter("UserId", UserId),
                new SqlParameter("@RoleId", RoleId));
        }

        //Get All Role
        public List<Role> GetAllRole()
        {
           var allRole= _employeeContext.Roles.FromSqlRaw("EXEC getAllRole").ToList();
            if (allRole != null && allRole.Count>0)
            {
                return allRole;
            }
            return null;
        }

        //Get all the user with role. 
        public List<GetAllUserWithRole> GetAllUserWithRole()
        {
            var allUserRole=_employeeContext.GetAllUserWithRoles.FromSqlRaw("EXEC GetAllUserWithRole").ToList();
            if(allUserRole!=null && allUserRole.Count> 0)
            {
                return allUserRole;
            }
            return null;
        }

        //Get the Role of the specific user.
        public GetAllUserWithRole GetUserRoleById(string RoleId, string UserId)
        {
            var roleIdParam = new SqlParameter("@RoleId", RoleId);
            var userIdParam = new SqlParameter("@UserId", UserId);

            //Get the value by userid and role for edit
            var getUserRole = _employeeContext.GetAllUserWithRoles.FromSqlRaw("EXEC GetUserWithRole @UserId, @RoleId", roleIdParam, userIdParam).ToList();
            if(getUserRole!=null && getUserRole.Count > 0)
            {
                return getUserRole.FirstOrDefault();
            }
            return null;
        }


        //Create Role in and insert into the database. 
        public void InsertRole(string RoleName)
        {
            var UserName = _contextAccessor.HttpContext.User.Identity.Name;
            //Insert role to the user.
            string parameter = $"spInsertRole @RoleName='{RoleName}', @CreatedAt='{DateTime.Now}', @CreatedBy='{UserName}'";
            _employeeContext.Database.ExecuteSqlRaw(parameter);
        }


        //Check the Role is already exist in the databse. 
        public bool RoleAlreadyExist(string RoleName)
        {
          var role=  _employeeContext.Roles.FromSqlRaw("exec checkRole @p0", RoleName).ToList();
            if(role!=null && role.Count> 0)
            {
                return true;
            }
            return false;
        }

        public void UpdateUserRoleAssign(EditUserRole edit)
        {
            //Sql parameter.
            var roleIdParam = new SqlParameter("@RoleId", edit.RoleId);
            var userIdParam = new SqlParameter("@UserId", edit.UserId);
            var OldRoleIdParam = new SqlParameter("@OldRoleId", edit.OldRoleId);

            var NewroleIdParam = new SqlParameter("@NewRoleId", edit.RoleId);
            _employeeContext.Database.ExecuteSqlRaw(
                "EXEC EditUserRole @UserId, @OldRoleId, @NewRoleId", userIdParam, OldRoleIdParam, NewroleIdParam);
        }

    }
}
