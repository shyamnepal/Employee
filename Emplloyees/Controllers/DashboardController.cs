using Emplloyees.Models;
using Emplloyees.Services.UserDetails.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Emplloyees.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private EmployeeContext _employeeContext;
        private IUserProfileService _UserProfile;
        public DashboardController(EmployeeContext employeeContext, IUserProfileService userProfile)
        {
            _employeeContext = employeeContext;
            _UserProfile = userProfile;
            
        }
        public IActionResult Index()
        { 
                return View();
        }
        
    }
}
