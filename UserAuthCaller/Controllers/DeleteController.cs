using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using UserAuthCaller.DTO;
using UserAuthCaller.Models;
using UserAuthCaller.Repository;

namespace UserAuthCaller.Controllers
{
    public class DeleteController : Controller,IDeleteController
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        public DeleteController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
            _apiSettings = apiSettings.Value;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(UserDeleteDto user, bool isAdminDelete = false)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    Content = content,
                    RequestUri = new Uri(_apiSettings.DeleteUser, UriKind.Relative)
                };

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "User Deleted Successfully";
                    ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();

                    if (isAdminDelete)
                    {
                        return RedirectToAction("GetAllUser", "UserDataMvc");
                    }
                    else
                    {
                        return RedirectToAction("Logout", "Logout");
                    }
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
        public IActionResult DeleteUser1(UserData user)
        {
            try
            {
                var x = new UserDeleteDto()
                {
                    id = user.id,
                    password = user.password
                };
                return View("DeleteForSure", x);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AdminDeleteUser1(UserData user)
        {
            try
            {
                var x = new UserDeleteDto()
                {
                    id = user.id,
                    password = user.password
                };
                return View("AdminDeleteForSure", x);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }
    }
}
