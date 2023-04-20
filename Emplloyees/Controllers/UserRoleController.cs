using AspNetCoreHero.ToastNotification.Abstractions;
using Emplloyees.Models;
using Emplloyees.Services.UserRoleService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Emplloyees.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly EmployeeContext _employeeContext;
        private readonly INotyfService _notyf;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleServices _roleServices;
        public UserRoleController(EmployeeContext employeeContext, INotyfService notyf, IHttpContextAccessor httpContextAccessor, IRoleServices roleServices)
        {
            _employeeContext = employeeContext;
            _notyf = notyf;
            _httpContextAccessor = httpContextAccessor;
            _roleServices = roleServices;
        }
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult CreateRole(string role)
        {
            try
            {
                if (role == null)
                {
                    _notyf.Error("Role is Empty");
                    return RedirectToAction("Index");
                }

                //Check the role exist in the database.
                bool checkRole = _roleServices.RoleAlreadyExist(role);
                if (checkRole)
                {
                    _notyf.Error($"{role} Role is already exist");
                    return RedirectToAction("Index");
                }

                //Insert the role 
                _roleServices.InsertRole(role);
                _notyf.Success($"Successfully Create Role Name {role}");


                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }



        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult RoleAssignPage()
        {
            //Get All the Users.
            var allUser = _employeeContext.Users.FromSqlRaw("EXEC GetAllUsers").ToList();
            if (allUser.Count > 0)
            {
                var dropdown = allUser.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.UserName


                }).ToList();
                ViewData["allUser"] = dropdown;
            }

            //Get All the Role.
            var allRole = _roleServices.GetAllRole();
            if(allRole.Count> 0)
            {
                var roleDropDown = allRole.Select(x => new SelectListItem()
                {
                    Value = x.RoleId.ToString(),
                    Text = x.RoleName
                }).ToList();
                ViewData["allRole"] = roleDropDown;
            }
            return View();
        }

        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult RoleAssign(string UserId, string RoleId)
        {
            try
            {
                //First check the RoleId and  UserId is not null.
                if (UserId != null && RoleId != null)
                {

                    //Check if the role aready assign.
                   bool sameRoleCheck= _roleServices.CheckRoleAlreadyAssignToUser(UserId, RoleId);


                    if(sameRoleCheck)
                    {
                        _notyf.Error("The Role Id is already assign to the user");
                        return RedirectToAction("RoleAssignPage");
                    }

                    //Assign the role to the user. 
                    _roleServices.AssignUserRole(UserId, RoleId);

                    _notyf.Success($"Role is assign to the user Id {UserId}", 5);
                    return RedirectToAction("RoleAssignPage");


                }

                _notyf.Error("User id or Role Name is Empty", 5);
                return RedirectToAction("RoleAssignPage");
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.ToString(), 5);
                return RedirectToAction("RoleAssignPage");
            }


        }

        //Get all User that is assign role. 
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult GetRole()
        {
            try
            {
                var getAllRoleWithUser = _roleServices.GetAllUserWithRole();
                return View(getAllRoleWithUser);
            }
            catch(Exception ex)
            {
                _notyf.Error(ex.ToString());
                return View();
            }
         

        }
        
        public IActionResult EditRole(string RoleId, string UserId)
        {
            try
            {
                //Get the user role information for edit.
                var user = _roleServices.GetUserRoleById(RoleId, UserId);

                //Get All the Role.
                var allRole = _roleServices.GetAllRole();
                if (allRole.Count > 0)
                {
                    var roleDropDown = allRole.Select(x => new SelectListItem()
                    {
                        Value = x.RoleId.ToString(),
                        Text = x.RoleName
                    }).ToList();
                    ViewData["allRole"] = roleDropDown;
                    
                }
                
                return View(user);
            }
            catch(Exception ex)
            {
                _notyf.Error(ex.ToString());
                return RedirectToAction("GetRole");
            }
            
        }

        //Edit Role of the User
        [Authorize(Roles = "SupperAdmin, Admin")]
         public IActionResult Edit(EditUserRole edit)
         {
            try
            {
             

                //Check the same role assign to the user.
                var sameRoleCheck = _roleServices.CheckRoleAlreadyAssignToUser(edit.UserId.ToString(), edit.RoleId.ToString());


                if (sameRoleCheck)
                {
                    _notyf.Error("The Role Id is already assign to the user");
                    return RedirectToAction("GetRole");
                }

              //Update the Assign role of the user.

                _notyf.Success("Successfully update the role");
                return RedirectToAction("GetRole");

            }catch(Exception ex)
            {
                _notyf.Error(ex.ToString());
                return RedirectToAction("GetRole");
            }
         }

        //Delte the User Role 
        [Authorize(Roles = "SupperAdmin, Admin")]
        public IActionResult DeleteUserRole(string UserId, string RoleId)
        {
            try
            {
                _roleServices.DeleteUserRole(UserId, RoleId);

                _notyf.Success("Successfully Delete the role of the user.");
                return RedirectToAction("GetRole");

            }
            catch(Exception ex)
            {
                _notyf.Error(ex.ToString());
                return RedirectToAction("GetRole");
            }
        }

    }
}
