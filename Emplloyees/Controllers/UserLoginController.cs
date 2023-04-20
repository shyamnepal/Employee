using Emplloyees.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Data.SqlClient;
using Emplloyees.Services.UserDetails.Interfaces;

namespace Emplloyees.Controllers
{
    public class UserLoginController : Controller
    {
        private readonly EmployeeContext _employeeContext;
        private readonly INotyfService _notyf;
        private IUserProfileService _UserProfile;
        public UserLoginController(EmployeeContext employeeContext, INotyfService notyf, IUserProfileService userProfile)
        {
            _employeeContext = employeeContext;
            _notyf = notyf;
            _UserProfile = userProfile;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginModel login)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            var user = _employeeContext.Users.FromSqlRaw("exec spGetUser @p0", login.UserName).ToList();
            //_employeeContext.Users.FirstOrDefault(x => x.UserName == userName);
            if (user.Count==0)
            {
                _notyf.Error("User Not found", 5);
                return RedirectToAction("Index");
            }

            string UserPassword = null;
            string UserId = null;

            

            foreach (var item in user)
            {
                UserPassword = item.Password;
                UserId = item.Id.ToString();
            }

            if (BCrypt.Net.BCrypt.Verify(login.Password, UserPassword))
            {
                // Create the claims for the authenticated user
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, login.UserName),
                    new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
                    

                // Add additional claims as needed
            };
                var userIdParam = new SqlParameter("@UserId", UserId);

                var roleName = _employeeContext.UserRoles.FromSqlRaw("EXEC GetRoleNameById @UserId", userIdParam).ToList();
                string RoleId = null;
                if (roleName.Count > 0)
                {
                    foreach (var r in roleName)
                    {
                        RoleId = r.RoleId.ToString();
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, r.UserId.ToString()));

                    }
                }
                var roleIdParam = new SqlParameter("@RoleId", RoleId);
                if (RoleId != null)
                {
                    var name = _employeeContext.Roles.FromSqlRaw("EXEC getRoleName @RoleId", roleIdParam).ToList();
                    if (name.Count > 0)
                    {
                        foreach (var r in name)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, r.RoleName));
                        }
                    }
                }

                //get the image url from the database. 

                string userData = _UserProfile.ImageUrl(UserId);
                //Add ImageUrl to the claim
                if (userData != null)
                {
                    claims.Add(new Claim("ImageUrl", userData));
                }


                // Create the authentication properties
                var authProperties = new AuthenticationProperties
                {
                    // Set whether the cookie should be persistent
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Set the expiration time for the cookie
                };

                // Create the claims identity
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);


                //password match and successfull login 
                _notyf.Success("Successful Login", 5);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                _notyf.Error("Password does not match", 5);
                return RedirectToAction("Index");
            }
        }
    }
}
