using Emplloyees.Models;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Emplloyees.Validation;
using AspNetCoreHero.ToastNotification.Abstractions;
using Emplloyees.Services.Account.Interfaces;

namespace Emplloyees.Controllers
{
    public class UserRegisterController : Controller
    {
        private readonly EmployeeContext _employeeContext;
        private readonly INotyfService _notyf;
        private readonly IUserAccountServices _userAccount;
        public UserRegisterController(EmployeeContext employeeContext, INotyfService notyf, IUserAccountServices userAccount)
        {
            _employeeContext = employeeContext;
            _notyf = notyf;
            _userAccount = userAccount;
        }

        public IActionResult Index()
        {
            return View();
        }

       public IActionResult UserCreate([Bind(include: "UserName, Email, FirstName, LastName, Password, PhoneNumber, ConfirmPassword")]AccountCreate model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                 
                    return View("Index");


                }
                   

                //check the user or email is exist 
                var userName = _employeeContext.Users.Any(x => x.UserName == model.UserName);
                var email = _employeeContext.Users.Any(x => x.Email == model.Email);
                if( userName || email)
                {
                    _notyf.Error("UserName or Email is Already Exist");
                    return RedirectToAction("Index");

                }

                //Hash the password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);



                //var registerUser = new User
                //{
                //    UserName = model.UserName,
                //    Email = model.Email,
                //    Password = passwordHash,
                //    FirstName = model.FirstName,
                //    LastName = model.LastName,
                //    CreatedAt = DateTime.Now,
                //    CreatedBy = model.UserName,
                //    PhoneNumber = model.PhoneNumber

                //};

                //Create user for login the system.
                _userAccount.createUser(model, passwordHash);

                //_employeeContext.Users.Add(registerUser);
                //_employeeContext.SaveChanges();
                _notyf.Success("Successfully create Users");
                return RedirectToAction("Index", "UserLogin");

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


    }
}
