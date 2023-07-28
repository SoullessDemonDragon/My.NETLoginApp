using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using UserAuthCaller.DTO;
using UserAuthCaller.Models;
using UserAuthCaller.Repository;

namespace UserAuthCaller.Controllers
{
    public class UpdateController : Controller,IUpdateController
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        public UpdateController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
            _apiSettings = apiSettings.Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserUpdateDto user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_apiSettings.UpdateUser, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var updatedUser = JsonConvert.DeserializeObject<UserData>(responseBody);
                    TempData["ToastMessage"] = "User Updated Successfully.";
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                    if (updatedUser.designation == "Admin")
                    {
                        return View("~/Views/Login/Admin/AdminProfile.cshtml", updatedUser);
                    }
                    return View("~/Views/Login/User/UserProfile.cshtml", updatedUser);
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
                    return RedirectToAction("UpdateUser", user);
                }
                else
                {
                    TempData["ToastMessage"] = "Some Unknown Error occured. Pls Try Again.";
                    return RedirectToAction("UpdateUser", user);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdateUser1(UserData user)
        {
            try
            {
                var x = new UserUpdateDto()
                {
                    id = user.id,
                    userName = user.userName,
                    age = user.age,
                    name = user.name,
                    email = user.email,
                    phonenumber = user.phonenumber,
                    designation = user.designation,
                    status = user.status,
                };
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("UpdateUser", x);
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminUpdateUser(AdminUserUpdateDto user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_apiSettings.UpdateUser, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "User updated successfully!";
                    return RedirectToAction("GetAllUser", "UserDataMvc");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
                    return RedirectToAction("AdminUpdateUser", user);
                }
                else
                {
                    return View("Error", response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        [HttpPost]
        public IActionResult AdminUpdateUser1(UserData user)
        {
            try
            {
                var x = new AdminUserUpdateDto()
                {
                    id = user.id,
                    userName = user.userName,
                    age = user.age,
                    name = user.name,
                    email = user.email,
                    phonenumber = user.phonenumber,
                    designation = user.designation,
                    status = user.status
                };
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("AdminUpdateUser", x);
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePassword(AuthUpdateDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_apiSettings.UpdatePassword, content);
                if (response.IsSuccessStatusCode)
                {
                    return View("PasswordUpdateResponse");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["UpdateMessage"] = "Wrong Current Password. Pls Try Again.";
                    return RedirectToAction("UpdatePassword1", dto.userId);
                }
                else return BadRequest(response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UpdatePassword1(int id)
        {
            var x = new AuthUpdateDto { userId = id };
            ViewBag.ToastMessage = TempData["UpdateMessage"]?.ToString();
            return View("UpdatePassword", x);
        }
    }
}
