using Emplloyees.Models;
using Emplloyees.Services.Account.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Emplloyees.Services.Account.Implementations
{
    public class UserAccountServices : IUserAccountServices
    {
        private readonly EmployeeContext _employeeContext;
        
        public UserAccountServices( EmployeeContext employeeContext)
        {
            
            _employeeContext = employeeContext;

        }
        //Register  the user. 
        public void createUser(AccountCreate model, string PasswordHash)
        {
            string parameter = $"spCreateUser @UserName='{model.UserName}',@Email='{model.Email}', @Password='{PasswordHash}', @FirstName='{model.FirstName}', @LastName= '{model.LastName}'," +
                 $"@CreatedAt='{DateTime.Now}', @CreatedBy='{model.UserName}', @PhoneNumber='{model.PhoneNumber}' ";

            _employeeContext.Database.ExecuteSqlRaw(parameter);
        }
    }
}
