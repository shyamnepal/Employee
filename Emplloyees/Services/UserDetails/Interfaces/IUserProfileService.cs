using Emplloyees.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Emplloyees.Services.UserDetails.Interfaces
{
    public interface IUserProfileService
    {
      
        UserInfo GetInfo();
        string ImageUrl(string id);
        List<Profile> AdditionalInfo(string UserId);
        void InsertUserAddress(UserProfileInfo profile);

        string GetAddressId();

        void InsertProfile(UserProfileInfo profile, string addressId);

        void UpdateAddress(UserInfo profile);
        void UpdateProfile(UserInfo profile);
        void UpdateteUser(UserInfo profile);

        void UpdateProfileForImageSelect(UserInfo profile, IFormFile imageFile);

        User GetLoginUserInfo();

        void UpdatePassword(string UserId, string PasswordHash);

        

    }


    public interface UserSession
    {
        string userName { get; set; }
        string profilePicture { get; set; }
        string userId { get; set; }
    }
}
