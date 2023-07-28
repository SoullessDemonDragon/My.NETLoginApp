using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using UserAuthCaller.DTO;
using UserAuthCaller.Models;
using UserAuthCaller.Repository;

namespace UserAuthCaller.Controllers
{
    public class LoginController : Controller, ILoginController
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        public LoginController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
            _apiSettings = apiSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserGetByEmailDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiSettings.GetUserByEmail, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserData>(responseBody);
                    return await HandleLogin(user);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["ToastMessage"] = "Wrong Password. Please Try Again.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ToastMessage"] = "This Email is not in the database. Please Try Again.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception)
            {
                TempData["ToastMessage"] = "Server Not Responding. Please Try Again Later.";
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            try
            {
                if (Request.Cookies.TryGetValue("UserData", out var responseBody))
                {
                    var dto = JsonConvert.DeserializeObject<UserData>(responseBody);
                    return await HandleLogin(dto);
                }
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("Login", new UserGetByEmailDto());
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        private async Task<IActionResult> HandleLogin(UserData user)
        {
            try
            {
                if (user.designation == "Admin")
                {
                    var identity = new ClaimsIdentity("MyAuthScheme");
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.name));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.designation));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("MyAuthScheme", principal);
                    Response.Cookies.Append("UserData", JsonConvert.SerializeObject(user));
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    return View("Admin/AdminProfile", user);
                }
                Response.Cookies.Append("UserData", JsonConvert.SerializeObject(user));
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("User/UserProfile", user);
            }
            catch (Exception)
            {
                TempData["ToastMessage"] = "Server Not Responding. Please Try Again Later.";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginByUserName(UserGetByUserNameDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiSettings.GetUserByUserName, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserData>(responseBody);
                    return await HandleLogin(user); // Call HandleLogin with the user object
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["ToastMessage"] = "Wrong Password. Pls Try Again.";
                    return View("LoginByUserName");
                }
                else
                {
                    TempData["ToastMessage"] = "This User Name is not in the database. Pls Try Again.";
                    return View("LoginByUserName");
                }
            }
            catch (Exception)
            {
                TempData["ToastMessage"] = "Server Not Responding. Pls Try Again Later.";
                return View("LoginByUserName");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoginByUserName()
        {
            try
            {
                if (Request.Cookies.TryGetValue("UserData", out var responseBody))
                {
                    var dto = JsonConvert.DeserializeObject<UserData>(responseBody);
                    return await HandleLogin(dto); // Call HandleLogin with the dto object
                }
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("LoginByUserName", new UserGetByUserNameDto());
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error: {ex.Message}";
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("LoginByUserName");
            }
        }

    }
}
