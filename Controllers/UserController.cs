using Microsoft.AspNetCore.Mvc;
using ATMBank.Data;  // Đảm bảo sử dụng đúng namespace của ATMContext
using ATMBank.Models; // Đảm bảo sử dụng đúng namespace của User model
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography; // Để sử dụng SHA256
using System.Text;                  // Để sử dụng Encoding
 using System.Linq;
 
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
    
     [HttpPost("{userId}/change-password")]
        public IActionResult ChangePassword(int userId, [FromBody] ChangePasswordDto changePasswordDto)
        {
            // Tìm người dùng theo UserId
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Kiểm tra mật khẩu hiện tại
            var currentPasswordHash = HashPassword(changePasswordDto.CurrentPassword);
            if (user.Password != currentPasswordHash)
            {
                return BadRequest("Mật khẩu hiện tại không chính xác.");
            }

            // Cập nhật mật khẩu mới
            user.Password = HashPassword(changePasswordDto.NewPassword);
            _context.SaveChanges();

            return Ok("Đổi mật khẩu thành công.");
        }
        [HttpGet("info/{username}")]
         public IActionResult GetUserInfo(string username)
         {
    // Tìm người dùng theo tên
         var user = _context.Users.FirstOrDefault(u => u.Name == username);
         if (user == null)
        {
        return NotFound(new { Message = "User not found" });
        }

    // Trả về thông tin chi tiết của người dùng
        var userInfo = new
       {
        UserId = user.UserId,
        Name = user.Name,
        Email = user.Email,
        // Giả sử bạn có trường CreatedAt
        };

    return Ok(userInfo);
}


        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }

    // DTO để thay đổi mật khẩu
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
