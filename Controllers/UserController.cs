using Microsoft.AspNetCore.Mvc;
using ATMBank.Data;  // Đảm bảo sử dụng đúng namespace của ATMContext
using ATMBank.Models; // Đảm bảo sử dụng đúng namespace của User model
using Microsoft.EntityFrameworkCore;

namespace ATMBank.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ATMContext _context;

        public UserController(ATMContext context)
        {
            _context = context;
        }
         [HttpGet]//getall users
        public async Task<IActionResult> GetAllUsers(){
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        // API lấy thông tin người dùng (bao gồm tên người dùng)
        [HttpGet("profile")]
        public IActionResult GetUserProfile(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Trả về chỉ tên người dùng
            var userProfile = new
            {
                Name = user.Name,
                Password = user.Password,
            };

            return Ok(userProfile);
        }
        [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        if (user == null)
        {
            return NotFound("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "User deleted successfully" });
    }
}
}
