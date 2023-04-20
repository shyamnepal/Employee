using AspNetCoreHero.ToastNotification.Abstractions;
using Emplloyees.Models;
using Emplloyees.Services.UserDetails.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Emplloyees.Services.UserDetails.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly EmployeeContext _employeeContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyf;
        public UserProfileService(EmployeeContext employeeContext, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment, INotyfService notyf)
        {
            _employeeContext = employeeContext;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _notyf = notyf;
        }

        //Additional user info check of the user for add profile.
        public List<Profile> AdditionalInfo(string UserId)
        {
            var userIdParam = new SqlParameter("@UserId", UserId);
            var info = _employeeContext.Profiles.FromSqlRaw("EXEC getUserProfile @UserId", userIdParam).ToList();
            return info;
        }

      

        //Get All the UserProfile.
        public UserInfo GetInfo()
        {
            string userName = _contextAccessor.HttpContext.User.Identity.Name;
            var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //get All the profile information to show in profile page
            if (userName != null && userId != null)
            {
                var UserIdParam = new SqlParameter("@UserId", userId);
                var getUserInfo1 = _employeeContext.UserBasicInfo.FromSqlRaw("EXEC GetFullProfile @UserId", UserIdParam).ToList();

                if (getUserInfo1 != null && getUserInfo1.Count > 0)
                {
                    return getUserInfo1.FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }
            return null;
        }

        //Get the imageUrl.
        public string ImageUrl(string id)
        {
            var UserIdParam = new SqlParameter("@UserId", id);
            var imgUrl = _employeeContext.Profiles.FromSqlRaw("EXEC ImageUrl @UserId", UserIdParam).ToList();
            if (imgUrl != null && imgUrl.Count > 0)
            {
                return imgUrl[0].ImageUrl;
            }
            return null;

        }

        //Insert Address of the User.
        public void InsertUserAddress(UserProfileInfo profile)
        {
            string userId = null;
            string userName = null;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userName = _contextAccessor.HttpContext.User.Identity.Name;
                userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
               
            }

            string parameter = $"SpInsertAddress @TemporaryAddress='{profile.TemporaryAddress}', @ParmanentAddress='{profile.ParmanentAddress}', @City='{profile.City}', @Country='{profile.Country}', @PostalCode= '{profile.PostalCode}', @State='{profile.State}'," +
                    $"@CreatedAt='{DateTime.Now}', @CreatedBy='{userName}', @UserId='{userId}' ";

             _employeeContext.Database.ExecuteSqlRawAsync(parameter);
        }

        public string GetAddressId()
        {
           string userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserIdParam = new SqlParameter("@UserId", userId);
            var getAddressId = _employeeContext.Addresses.FromSqlRaw("EXEC GetAddressByUserId @UserId", UserIdParam).ToList();
            string addressId = null;
            if (getAddressId.Count > 0)
            {

                addressId = getAddressId[0].AddressId.ToString();
                return addressId;
            }
            return null;


        }

        public void InsertProfile(UserProfileInfo profile, string addressId)
        {

            //store image and get the image name
            string imageurl = UploadedFile(profile.Image);

            string userId = null;
            string userName = null;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userName = _contextAccessor.HttpContext.User.Identity.Name;
                userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            }
            string ProfileParameter = $"SpInsertProfile @UserId='{userId}',@AddressId='{addressId}', @DateOfBirth='{profile.DateOfBirth}', @Sex='{profile.Sex}', @ImageUrl= '{imageurl}'," +
                       $"@CreatedAt='{DateTime.Now}', @CreatedBy='{userName}' ";
             _employeeContext.Database.ExecuteSqlRawAsync(ProfileParameter);
        }


        //Update the Address of the User.
        public void UpdateAddress(UserInfo profile)
        {
            string userName =_contextAccessor.HttpContext.User.Identity.Name;
            var AddressParameter = $"UpdateAddress @AddressId='{profile.AddressId}', @TemporaryAddress='{profile.TemporaryAddress}', @ParmanentAddress='{profile.ParmanentAddress}',@City='{profile.City}', " +
            $"@Country='{profile.Country}', @PostalCode='{profile.PostalCode}', @State='{profile.State}', @UpdatedAt='{DateTime.Now}', @UpdatedBy='{userName}' ";

            //Update the address of the user.
            _employeeContext.Database.ExecuteSqlRaw(AddressParameter);
        }

        public void UpdateProfile(UserInfo profile)
        {
            string userName = _contextAccessor.HttpContext.User.Identity.Name;
            var profileParameter = $"UpdateProfile @ProfileId='{profile.ProfileID}', @DateOfBirth='{profile.DateOfBirth}', @Sex='{profile.sex}', @ImageUrl='{profile.ImageUrl}', @UpdatedAt='{DateTime.Now}', " +
                                            $"@UpdatedBy='{userName}'";
            //Update the profile of the user 
            _employeeContext.Database.ExecuteSqlRaw(profileParameter);

        }

        public void UpdateteUser(UserInfo profile)
        {
            string userName = _contextAccessor.HttpContext.User.Identity.Name;
            var UserParameter = $"updateUser @UserId ='{profile.ID}',@Email='{profile.Email}', @FirstName= '{profile.FirstName}', @LastName='{profile.LastName}',  @UpdatedAt='{DateTime.Now}', @UpdatedBy='{userName}', @PhoneNumber='{profile.PhoneNumber}'";
            //Update the User table 
            _employeeContext.Database.ExecuteSqlRaw(UserParameter);
        }

        public void UpdateProfileForImageSelect(UserInfo profile, IFormFile imageFile)
        {
            string userName = _contextAccessor.HttpContext.User.Identity.Name;

            //Store image and get the image name.

            string imageurl = UploadedFile(imageFile);

            //Delete the existing image from the  folder.
            DeleteImage(profile.ImageUrl);


            //Update the iamge in database.
            var profileParameterForImageSelect = $"UpdateProfile @ProfileId='{profile.ProfileID}', @DateOfBirth='{profile.DateOfBirth}', @Sex='{profile.sex}', @ImageUrl='{imageurl}', @UpdatedAt='{DateTime.Now}', " +
                                $"@UpdatedBy='{userName}'";
            _employeeContext.Database.ExecuteSqlRawAsync(profileParameterForImageSelect);
        }



        //Get the unique Name of the Image.
        private string UploadedFile(IFormFile Image)
        {
            string uniqueFileName = null;
            if (Image != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        public void DeleteImage(string fileName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(imagePath))
            {
                try
                {
                    System.IO.File.Delete(imagePath);
                }
                catch (Exception ex)
                {
                    _notyf.Error("Error while Delete the image form the system");
                }
            }

        }

        public User GetLoginUserInfo()
        {
           string userName= _contextAccessor.HttpContext.User.Identity.Name;
            var user = _employeeContext.Users.FromSqlRaw("exec spGetUser @p0", userName).ToList();
            if(user!=null && user.Count > 0)
            {
               return user.FirstOrDefault();
            }
            return null;
        }

        public void UpdatePassword(string UserId, string PasswordHash)
        {
            string parameter = $"ChangePassword @UserId='{UserId}', @PasswordHash='{PasswordHash}'";
             _employeeContext.Database.ExecuteSqlRaw(parameter);
        }
    }
}
