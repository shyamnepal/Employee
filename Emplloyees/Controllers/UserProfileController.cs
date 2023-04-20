using AspNetCoreHero.ToastNotification.Abstractions;
using Emplloyees.Models;
using Emplloyees.Services.UserDetails.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;

namespace Emplloyees.Controllers
{
    public class UserProfileController : Controller
    {
        public readonly EmployeeContext _employeeContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyf;
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(EmployeeContext employeeContext, IWebHostEnvironment webHostEnvironment, INotyfService notyf, IUserProfileService userProfileService)
        {
            _employeeContext = employeeContext;
            _webHostEnvironment = webHostEnvironment;
            _notyf = notyf;
            _userProfileService = userProfileService;
        }
        [Authorize]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



                //first check the user add the Addition Profile information
                var profile = _userProfileService.AdditionalInfo(userId);
                if (profile.Count == 0)
                {
                    return View("AddProfile");
                }

                //get All the profile information to show in profile page
                if (userName != null)
                {
                    UserInfo userData = _userProfileService.GetInfo();

                    return View(userData);
                }

            }
            return View();
        }

        //Get the login User Information 
        [Authorize]
        public async  Task<ActionResult> AddProfile(UserProfileInfo profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    
                    return View();
                }

                //Add the User info for profile. 
                _userProfileService.InsertUserAddress(profile);

                //Get Address Id of the User. 

                string  addressId = _userProfileService.GetAddressId();


                //Insert into profile.
                _userProfileService.InsertProfile(profile, addressId);
                

                _notyf.Success("Successfully add addition user info");
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _notyf.Error(ex.ToString());
                return View("AddProfile");
                
            }
            

        }

        [Authorize]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                var message = "Model is invalid";
                _notyf.Error(message);
                return RedirectToAction("Index");
            }

            //Get the login  user for userId and password
            var user = _userProfileService.GetLoginUserInfo();

                string UserPassword = user.Password;
                string UserId = user.Id.ToString();
           
               //Verified Password using BCrypt.
                if (BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, UserPassword))
                {
                    string passworHash = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword); 

                //Updatte  the password in  the database. 
                   _userProfileService.UpdatePassword(UserId, passworHash);

                    _notyf.Success("Password Change successfully");
                    return RedirectToAction("Index");
                    
                }
            _notyf.Error("Password change failed");
            return RedirectToAction("Index");
        }


        public IActionResult EditProfile(UserInfo profile, IFormFile imageFile)
        {
            ModelState.Remove("imageFile");
            if (ModelState.IsValid)
            {
                try
                {
           
                    //If the image is not null update the image form database and delete from images folder.
                    if (imageFile != null)
                    {
                        //Updatae the Profile with new image and delete the existing image from source file and Database image path.
                        _userProfileService.UpdateProfileForImageSelect(profile, imageFile);


                        //Update the User table 
                        _userProfileService.UpdateteUser(profile);

                        //Update the address of the user.
                        _userProfileService.UpdateteUser(profile);

                        _notyf.Success("Successfully Edit User Profile");
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        //Update the address of the user.
                        _userProfileService.UpdateAddress(profile);

                        //Update the profile of the user 
                        _userProfileService.UpdateProfile(profile);


                        //Update the User table 
                        _userProfileService.UpdateteUser(profile);

                        _notyf.Success("Successfully Edit User Profile");
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    _notyf.Error(ex.ToString());
                    return RedirectToAction("Index");
                }     
            }

            var message = string.Join(" | ", ModelState.Values

            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));

            return RedirectToAction("Index");
        }

      


    }
}
