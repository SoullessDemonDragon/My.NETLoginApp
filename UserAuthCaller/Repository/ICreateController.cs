using Microsoft.AspNetCore.Mvc;
using UserAuthCaller.DTO;

namespace UserAuthCaller.Repository
{
    public interface ICreateController
    {
        Task<IActionResult> CreateUser(UserCreateDto user);
        IActionResult CreateUser();
    }
}
