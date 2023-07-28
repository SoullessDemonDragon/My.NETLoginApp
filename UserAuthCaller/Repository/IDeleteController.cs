using Microsoft.AspNetCore.Mvc;
using UserAuthCaller.DTO;
using UserAuthCaller.Models;

namespace UserAuthCaller.Repository
{
    public interface IDeleteController
    {
        Task<IActionResult> DeleteUser(UserDeleteDto user, bool isAdminDelete = false);
        IActionResult DeleteUser1(UserData user);
        IActionResult AdminDeleteUser1(UserData user);
    }
}
