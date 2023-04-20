using Microsoft.AspNetCore.Mvc;

namespace Emplloyees.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult LogoutUser()
        {
            Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Index", "UserLogin");
        }
    }
}
