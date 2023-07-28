using Microsoft.AspNetCore.Mvc;
using UserAuthCaller.DTO;

namespace UserAuthCaller.Repository
{
    public interface ILoginController
    {
        Task<IActionResult> Login(UserGetByEmailDto dto);
        Task<IActionResult> Login();
        Task<IActionResult> LoginByUserName(UserGetByUserNameDto dto);
        Task<IActionResult> LoginByUserName();
    }
}
