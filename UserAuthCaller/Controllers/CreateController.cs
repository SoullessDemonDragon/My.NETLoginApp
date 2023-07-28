using Microsoft.AspNetCore.Authorization;
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
    public class CreateController : Controller,ICreateController
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        public CreateController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClientFactory.CreateClient("MyApiClient");
            _apiSettings = apiSettings.Value;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateUser(UserCreateDto user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiSettings.CreateUser, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "User Created successfully!";
                    return RedirectToAction("GetAllUser", "UserDataMvc");
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["ToastMessage"] = "User Email or User Name Already Exists. Pls Try Again.";
                    return RedirectToAction(nameof(CreateUser), user);
                }
                else
                {
                    TempData["ToastMessage"] = $"Error: {response.ReasonPhrase}";
                    return RedirectToAction(nameof(CreateUser), user);
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(CreateUser), user);
            }
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly", AuthenticationSchemes = "MyAuthScheme", Roles = "Admin")]
        public IActionResult CreateUser()
        {
            try
            {
                ViewBag.ToastMessage = TempData["ToastMessage"]?.ToString();
                return View("CreateUser", new UserCreateDto());
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }

    }
}
