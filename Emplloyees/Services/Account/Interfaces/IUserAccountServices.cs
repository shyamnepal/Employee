using Emplloyees.Models;

namespace Emplloyees.Services.Account.Interfaces
{
    public interface IUserAccountServices
    {
        void createUser(AccountCreate model, string PasswordHash);
    }
}
