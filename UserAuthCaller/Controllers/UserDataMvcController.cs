using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using UserAuthCaller.Models;

namespace UserAuthCaller.Controllers
{
    public class UserDataMvcController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public UserDataMvcController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
            _apiSettings = apiSettings.Value;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly", AuthenticationSchemes = "MyAuthScheme", Roles = "Admin")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_apiSettings.GetUser);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    var users = JsonConvert.DeserializeObject<List<UserData>>(responseBody);
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    return View("GetAllUsers", users);
                }
                else { return View("Error", response.ReasonPhrase); }
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> GetUserById(UserGetByIdDto userDto)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(userDto);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PostAsync("api/UserData/GetUserById/id", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = response.Content.ReadAsStringAsync().Result;
        //            var user = JsonConvert.DeserializeObject<UserData>(responseBody);
        //            if (user.designation == "Admin")
        //                return View("AdminUserProfile", user);
        //            return View("UserProfile", user);
        //        }
        //        else
        //        {
        //            return View("Error", response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetUserByEmail(UserGetByEmailDto dto)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(dto);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PostAsync("api/UserData/GetUserByEmail/email", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            var user = JsonConvert.DeserializeObject<UserData>(responseBody);

        //            if (user.designation == "Admin")
        //            {
        //                var identity = new ClaimsIdentity("MyAuthScheme");
        //                identity.AddClaim(new Claim(ClaimTypes.Name, user.name));
        //                identity.AddClaim(new Claim(ClaimTypes.Role, user.designation));
        //                var principal = new ClaimsPrincipal(identity);
        //                await HttpContext.SignInAsync("MyAuthScheme", principal);
        //                Response.Cookies.Append("UserData", responseBody);
        //                return View("AdminUserProfile", user);
        //            }
        //            Response.Cookies.Append("UserData", responseBody);
        //            return View("UserProfile", user);
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            TempData["ToastMessage"] = "Wrong Password. Pls Try Again.";
        //            return RedirectToAction("GetUserByEmail");
        //        }
        //        else
        //        {
        //            TempData["ToastMessage"] = "This Email is not in the database. Pls Try Again.";
        //            return RedirectToAction("GetUserByEmail");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData["ToastMessage"] = "Server Not Responding. Pls Try Again Later.";
        //        return RedirectToAction("GetUserByEmail");
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetUserByEmail()
        //{
        //    try
        //    {
        //        if (Request.Cookies.TryGetValue("UserData", out var responseBody))
        //        {
        //            var dto = JsonConvert.DeserializeObject<UserData>(responseBody);

        //            if (dto.designation == "Admin")
        //                return View("AdminUserProfile", dto);
        //            return View("UserProfile", dto);
        //        }
        //        ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //        return View("GetUserByEmail", new UserGetByEmailDto());
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetUserByUserName(UserGetByUserNameDto dto)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(dto);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PostAsync("api/UserData/GetUserByUserName/userName", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            var user = JsonConvert.DeserializeObject<UserData>(responseBody);

        //            if (user.designation == "Admin")
        //            {
        //                var identity = new ClaimsIdentity("MyAuthScheme");
        //                identity.AddClaim(new Claim(ClaimTypes.Name, user.name));
        //                identity.AddClaim(new Claim(ClaimTypes.Role, user.designation));
        //                var principal = new ClaimsPrincipal(identity);
        //                await HttpContext.SignInAsync("MyAuthScheme", principal);
        //                Response.Cookies.Append("UserData", responseBody);
        //                return View("AdminUserProfile", user);
        //            }
        //            Response.Cookies.Append("UserData", responseBody);
        //            return View("UserProfile", user);
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            TempData["ToastMessage"] = "Wrong Password. Pls Try Again.";
        //            return RedirectToAction("GetUserByUserName");
        //        }
        //        else
        //        {
        //            TempData["ToastMessage"] = "This User Name is not in the database. Pls Try Again.";
        //            return RedirectToAction("GetUserByUserName");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        TempData["ToastMessage"] = "Server Not Responding. Pls Try Again Later.";
        //        return RedirectToAction("GetUserByUserName");
        //    }
        //}

        //[HttpGet]
        //public IActionResult GetUserByUserName()
        //{
        //    try
        //    {
        //        if (Request.Cookies.TryGetValue("UserData", out var responseBody))
        //        {
        //            var dto = JsonConvert.DeserializeObject<UserData>(responseBody);

        //            if (dto.designation == "Admin")
        //                return View("AdminUserProfile", dto);
        //            return View("UserProfile", dto);
        //        }
        //        ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //        return View("GetUserByUserName", new UserGetByUserNameDto());
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //[AutoValidateAntiforgeryToken]
        //public async Task<IActionResult> CreateUser(UserCreateDto user)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(user);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PostAsync("api/UserData/CreateUser", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["ToastMessage"] = "User Created successfully!";
        //            return RedirectToAction(nameof(GetAllUser));
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Conflict)
        //        {
        //            TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
        //            return RedirectToAction(nameof(CreateUser), user);
        //        }
        //        else
        //        {
        //            return View("Error", response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Authorize(Policy = "AdminOnly", AuthenticationSchemes = "MyAuthScheme", Roles = "Admin")]
        //public IActionResult CreateUser()
        //{
        //    try
        //    {
        //        ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //        return View("CreateUser", new UserCreateDto());
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateUser(UserUpdateDto user)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(user);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PutAsync("api/UserData/UpdateUser", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            var updatedUser = JsonConvert.DeserializeObject<UserData>(responseBody);
        //            TempData["ToastMessage"] = "User Updated Successfully.";
        //            ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //            if (updatedUser.designation == "Admin")
        //            {
        //                return View("~/Views/Login/Admin/AdminProfile.cshtml", updatedUser);
        //            }
        //            return View("~/Views/Login/User/UserProfile.cshtml", updatedUser);
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Conflict)
        //        {
        //            TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
        //            return RedirectToAction("UpdateUser", user);
        //        }
        //        else
        //        {
        //            return View("Error", response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //public IActionResult UpdateUser1(UserData user)
        //{
        //    try
        //    {
        //        var x = new UserUpdateDto()
        //        {
        //            id = user.id,
        //            userName = user.userName,
        //            age = user.age,
        //            name = user.name,
        //            email = user.email,
        //            phonenumber = user.phonenumber,
        //            designation = user.designation,
        //        };
        //        ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //        return View("UpdateUser", x);
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AdminUpdateUser(AdminUserUpdateDto user)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(user);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PutAsync("api/UserData/UpdateUser", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["ToastMessage"] = "User updated successfully!";
        //            return RedirectToAction("GetAllUser");
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Conflict)
        //        {
        //            TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
        //            return RedirectToAction("AdminUpdateUser", user);
        //        }
        //        else
        //        {
        //            return View("Error", response.ReasonPhrase);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //public IActionResult AdminUpdateUser1(UserData user)
        //{
        //    try
        //    {
        //        var x = new AdminUserUpdateDto()
        //        {
        //            id = user.id,
        //            userName = user.userName,
        //            age = user.age,
        //            name = user.name,
        //            email = user.email,
        //            phonenumber = user.phonenumber,
        //            designation = user.designation,
        //            status = user.status
        //        };
        //        ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //        return View("AdminUpdateUser", x);
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error", ex.Message);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteUser(UserDeleteDto user)
        //{
        //    try
        //    {
        //        var json = JsonConvert.SerializeObject(user);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Delete,
        //            Content = content,
        //            RequestUri = new Uri("api/UserData/DeleteUser", UriKind.Relative)
        //        };
        //        var response = await _httpClient.SendAsync(request);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["ToastMessage"] = "User Deleted Successfully";
        //            ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
        //            return RedirectToAction("GetAllUser");
        //        }
        //        else { return View("Error", response.ReasonPhrase); }
        //    }
        //    catch (Exception ex) { return View("Error", ex.Message); }
        //}

        //[HttpPost]
        //public IActionResult DeleteUser1(UserData user)
        //{
        //    try
        //    {
        //        var x = new UserDeleteDto()
        //        {
        //            id = user.id,
        //            password = user.password
        //        };
        //        return View("DeleteForSure", x);
        //    }
        //    catch (Exception ex)
        //    {

        //        return View("Error", ex.Message);
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View("Privacy");
        }
    }
}