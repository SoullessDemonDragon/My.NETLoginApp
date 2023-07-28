using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyLib.Data;
using MyLib.DTO;
using MyLib.Models;
using System.ComponentModel;

namespace MyLib.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthDataController : ControllerBase
    {
        private readonly MyDbContext? _context;

        public AuthDataController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("verify")]
        [DisplayName("Verify User")]
        public async Task<ActionResult> VerifyAuthData(AuthData data)
        {
            try
            {
                var storedData = await _context.authDatas.FirstOrDefaultAsync(a => a.userId == data.userId);

                if (storedData == null) return NotFound();

                if (storedData.password != data.password)
                {
                    return Unauthorized();
                }
                var user = await _context.userDatas.FindAsync(data.userId);
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updatepassword")]
        [DisplayName("Update Password")]
        public async Task<IActionResult> UpdatePassword(AuthUpdateDto authDto)
        {
            try
            {
                var storedAuthData = await _context.authDatas.FirstOrDefaultAsync(a => a.userId == authDto.userId);
                if (storedAuthData == null) return NotFound();
                if (storedAuthData.password != authDto.currentPassword) { return Unauthorized(); }
                storedAuthData.password = authDto.newPassword;
                var user = await _context.userDatas.FindAsync(authDto.userId);
                if (user != null)
                {
                    user.password = authDto.newPassword;
                }
                else { return NotFound("User Not in Database"); }
                await _context.SaveChangesAsync();
                return Ok("Password Updated");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
