using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using UserAuthCaller.DTO;

namespace UserAuthCaller.Controllers
{
    public class AuthDataMvcController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthDataMvcController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAuthData(AuthVerifyDto data)
        {
            try
            {
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/AuthData/VerifyAuthData/verify", content);
                if (response.IsSuccessStatusCode)
                {
                    var x = new UserGetByIdDto { id = data.userId, password = data.password };
                    return RedirectToAction("GetUserById", "UserDataMvc", x);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized) { return BadRequest("Incorrect UserId Or Password "); }
                else { return BadRequest(response.ReasonPhrase); }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
        public IActionResult VerifyAuthData()
        {
            return View("Index", new AuthVerifyDto());
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(AuthUpdateDto dto)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/AuthData/UpdatePassword/updatepassword", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["UpdateMessage"] = "Password Updated Successfully.";
                    return RedirectToAction("Login", "Login");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    TempData["UpdateMessage"] = "Wrong Current Password. Pls Try Again.";
                    return RedirectToAction("UpdatePassword", dto.userId);
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
