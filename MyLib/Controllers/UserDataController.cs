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
    public class UserDataController : ControllerBase
    {
        private readonly MyDbContext? _context;

        public UserDataController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/<UserDataController>
        [HttpGet]
        [DisplayName("Get All Users")]
        public async Task<ActionResult<IEnumerable<UserData>>> GetUser()
        {
            try
            {
                var users = await _context.userDatas.ToListAsync();
                if (users == null) return BadRequest();
                if (!ModelState.IsValid) return BadRequest();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<UserDataController>/5
        [HttpPost("id")]
        [DisplayName("Get User By Id")]
        public async Task<ActionResult<UserData>> GetUserById(UserGetByIdDto userDto)
        {
            try
            {
                var authData = await _context.authDatas.FirstOrDefaultAsync(a => a.userId == userDto.id);
                if (authData == null) return NotFound("AuthData not found.");
                if (authData.password != userDto.password) return Unauthorized();
                var user = await _context.userDatas.FindAsync(userDto.id);
                if (user.status == "Deactivated") return NotFound();
                if (user == null) return NotFound();

                if (!ModelState.IsValid) return BadRequest();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("email")]
        [DisplayName("Get User By Email")]
        public async Task<ActionResult<UserData>> GetUserByEmail(UserGetByEmailDto userDto)
        {
            try
            {
                var user = await _context.userDatas.FirstOrDefaultAsync(u => u.email == userDto.email);
                if (user == null) return NotFound();
                if (user.status == "Deactivated") return NotFound();

                var authData = await _context.authDatas.FirstOrDefaultAsync(a => a.userId == user.id);
                if (authData == null) return NotFound("AuthData not found.");
                if (authData.password != userDto.password) return Unauthorized();

                if (!ModelState.IsValid) return BadRequest();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("userName")]
        [DisplayName("Get User By UserName")]
        public async Task<ActionResult<UserData>> GetUserByUserName(UserGetByUserNameDto userDto)
        {
            try
            {
                var user = await _context.userDatas.FirstOrDefaultAsync(u => u.userName == userDto.userName);
                if (user == null) return NotFound();
                if (user.status == "Deactivated") return NotFound();

                var authData = await _context.authDatas.FirstOrDefaultAsync(a => a.userId == user.id);
                if (authData == null) return NotFound("AuthData not found.");
                if (authData.password != userDto.password) return Unauthorized();

                if (!ModelState.IsValid) return BadRequest();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{username}")]
        public async Task<ActionResult<UserData>> GetUserByUName(string? username)
        {
            try
            {
                var user = await _context.userDatas.FirstOrDefaultAsync(u => u.userName == username);
                if (user == null) return NotFound();
                if (!ModelState.IsValid) return BadRequest();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<UserDataController>
        [HttpPost]
        [DisplayName("Create New User")]
        public async Task<ActionResult<UserData>> CreateUser(UserCreateDto userDto)
        {
            try
            {
                var existingUserMail = await _context.userDatas.FirstOrDefaultAsync(u => u.email == userDto.email);
                var existingUserName = await _context.userDatas.FirstOrDefaultAsync(u => u.userName == userDto.userName);
                if (existingUserMail != null)
                {
                    // If a user with the same email already exists, return a conflict response.
                    return Conflict("Email Already Exists");
                }
                if (existingUserName != null)
                {
                    return Conflict("User Name Already Exists");
                }
                var user = new UserData
                {
                    userName = userDto.userName,
                    name = userDto.name,
                    email = userDto.email,
                    age = userDto.age,
                    phonenumber = userDto.phonenumber,
                    password = userDto.password,
                    guid = Guid.NewGuid(),
                    designation = userDto.designation,
                    status = "Activated" 
                };
                

                if (user == null) return BadRequest();
                _context.userDatas.Add(user);
                await _context.SaveChangesAsync();
                var authData = new AuthData
                {
                    userId = user.id,
                    password = user.password
                };
                _context.authDatas.Add(authData);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserDataController>/5
        [HttpPut]
        [DisplayName("Edit User By Id")]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userDto)
        {
            var olduser = await _context.userDatas.FindAsync(userDto.id);
            if (olduser == null) return NotFound();
            
            var existingUserMail = await _context.userDatas.FirstOrDefaultAsync(u => u.email == userDto.email && u.id != userDto.id);
            var existingUserName = await _context.userDatas.FirstOrDefaultAsync(u => u.userName == userDto.userName && u.id != userDto.id);
            if (existingUserMail != null)
            {
                // If a user with the same email already exists (excluding the current user being updated), return a conflict response.
                return Conflict("User with the same email already exists.");
            }
            if (existingUserName != null)
            {
                return Conflict("User Name Already Exists.");
            }
            olduser.id = userDto.id;
            olduser.userName = userDto.userName;
            olduser.name = userDto.name;
            olduser.email = userDto.email;
            olduser.age = userDto.age;
            olduser.phonenumber = userDto.phonenumber;
            olduser.status = userDto.status;
            if(userDto.designation == null)
            {
                olduser.designation = "User";
            }
            else olduser.designation = userDto.designation;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                if(!UserExists(userDto.id)) return NotFound();
                return BadRequest(ex.Message);
            }
            return Ok(olduser);
        }

        private bool UserExists(int id)
        {
            return _context.userDatas.Any(x => x.id == id);
        }

        // DELETE api/<UserDataController>/5
        [HttpDelete]
        [DisplayName("Delete User By Id")]
        public async Task<ActionResult> DeleteUser(UserDeleteDto userDto)
        {
            try
            {
                var authData = await _context.authDatas.FirstOrDefaultAsync(a => a.userId == userDto.id);
                if (authData == null) return NotFound("AuthData not found.");
                if (authData.password != userDto.password) return Unauthorized();
                var user = await _context.userDatas.FindAsync(userDto.id);
                if (user == null) return NotFound();
                user.status = "Deactivated";
                //_context.authDatas.Remove(authData);
                //_context.userDatas.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("User Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
