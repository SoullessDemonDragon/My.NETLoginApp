using Microsoft.AspNetCore.Mvc;
using UserAuthCaller.DTO;
using UserAuthCaller.Models;

namespace UserAuthCaller.Repository
{
    public interface IUpdateController
    {
        Task<IActionResult> UpdateUser(UserUpdateDto user);
        IActionResult UpdateUser1(UserData user);
        Task<IActionResult> AdminUpdateUser(AdminUserUpdateDto user);
        IActionResult AdminUpdateUser1(UserData user);
        Task<IActionResult> UpdatePassword(AuthUpdateDto dto);
        IActionResult UpdatePassword1(int id);
    }
}
