using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace UserAuthCaller.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("MyAuthScheme");
            // Perform logout logic, such as clearing session or authentication cookies
            Response.Cookies.Delete("UserData");
            // Redirect the user to the login page
            return RedirectToAction("Login", "Login");
        }
    }
}
